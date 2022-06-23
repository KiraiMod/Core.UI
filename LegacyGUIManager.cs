using KiraiMod.Core.ModuleAPI;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace KiraiMod.Core.UI
{
    [Module]
    public static class LegacyGUIManager
    {
        public static event Action OnLoad;
        public static event Action OnLoadLate;
        public static event Action<bool> OnUIToggle;

        public static GameObject GUI;
        public static GameObject UserInterface;
        public static GameObject Pinned;

        private static BaseInputModule inputFix;
        private static BaseInputModule inputOrig;

        [Configure<bool>("Visuals.Legacy UI", false, Saved: false)]
        public static bool Showing
        {
            get => UserInterface != null && UserInterface.active;
            set
            {
                if (Showing == value)
                    return;

                UserInterface.active = value;
                OnUIToggle?.Invoke(value);

                inputOrig.enabled = !(inputFix.enabled = value);
            }
        }

        static LegacyGUIManager()
        {
            Events.UIManagerLoaded += OnUIManagerLoaded;

            Plugin.Configuration.Bind("GUI", "Keybind", new Key[] { Key.RightShift }, "The keybind you want to use to open the GUI").Register(() => Showing ^= true);
        }

        private static void OnUIManagerLoaded()
        {
            SetupFix();
            CreateUI();
        }

        private static void SetupFix()
        {
            GameObject sys = GameObject.Find("_Application/UiEventSystem");
            inputOrig = sys.GetComponent<EventSystem>().m_SystemInputModules[0];
            (inputFix = sys.AddComponent<StandaloneInputModule>()).enabled = false;
        }
        
        private static void CreateUI()
        {
            GUI = new GameObject().DontDestroyOnLoad();
            GUI.name = "KiraiMod.GUI";
            GUI.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            GUI.AddComponent<CanvasScaler>();
            GUI.AddComponent<GraphicRaycaster>();

            UserInterface = new GameObject();
            UserInterface.name = nameof(UserInterface);
            UserInterface.transform.SetParent(GUI.transform);

            Pinned = new GameObject();
            Pinned.name = nameof(Pinned);
            Pinned.transform.SetParent(GUI.transform);

            OnLoad?.StableInvoke();

            MemberAttribute.Added += HandleMember; 
            MemberAttribute.All.ForEach(HandleMember);

            OnLoadLate?.StableInvoke();

            Plugin.Logger.LogInfo("Loaded GUI");
        }

        private static void HandleMember(MemberAttribute member)
        {
            if (member is KeybindAttribute || member.GetType().IsGenericTypeDefinition && member.GetType().GetGenericTypeDefinition() == typeof(KeybindAttribute<>))
                return;

            UIElement element;
            if (member is BaseConfigureAttribute configure)
                element = (UIElement)typeof(LegacyGUIManager)
                    .GetMethod(nameof(CreateTypedElement), BindingFlags.NonPublic | BindingFlags.Static)
                    .MakeGenericMethod(member.GetType().GenericTypeArguments[0])
                    .Invoke(null, new object[1] { configure });
            else if (member is InteractAttribute interact)
            {
                element = new(interact.Name);
                element.Changed += interact.Invoke;
            }
            else return;

            string highest = member.Parts.Length == 1 ? "Uncategorized" : member.Parts[0];

            // this needs to be rewritten
            UIGroup lowest = null;
            foreach (UIGroup group in UIManager.HighGroups)
                if (group.name == highest)
                {
                    lowest = group;
                    break;
                }

            if (lowest == null)
            {
                lowest = new(highest);
                lowest.RegisterAsHighest();
            }

            if (member.Parts.Length >= 2)
                foreach (string section in member.Parts[1..^1])
                {
                    bool found = false;

                    foreach (UIElement elem in lowest.elements)
                    {
                        if (elem is not UIElement<UIGroup> group || elem.name != section)
                            continue;
                        
                        lowest = group.Bound._value;
                        found = true;
                        break;
                    }

                    if (found) continue;
                    
                    Plugin.Logger.LogDebug("Creating new section: " + section);
                    lowest = new(section, lowest);
                }

            lowest.AddElement(element);
        }

        private static UIElement<T> CreateTypedElement<T>(BaseConfigureAttribute attribute)
        {
            UIElement<T> element = new(attribute.Name, attribute.DynamicValue);
            element.Bound.ValueChanged += value => attribute.DynamicValue = value;
            attribute.DynamicValueChanged += value => element.Bound.Value = value;
            return element;
        }
    }
}
