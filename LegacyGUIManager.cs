using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace KiraiMod.Core.UI
{
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

        public static bool Showing
        {
            get => UserInterface.active;
            set
            {
                if (UserInterface.active == value)
                    return;

                UserInterface.active = value;
                OnUIToggle?.Invoke(value);

                inputOrig.enabled = !(inputFix.enabled = value);
            }
        }

        static LegacyGUIManager()
        {
            Events.UIManagerLoaded += OnUIManagerLoaded;

            Plugin.cfg.Bind("GUI", "Keybind", new Key[] { Key.RightShift }, "The keybind you want to use to open the GUI").Register(() => Showing ^= true);
        }

        private static void OnUIManagerLoaded()
        {
            SetupFix();
            CreateUI();
        }

        private static void SetupFix()
        {
            var sys = GameObject.Find("_Application/UiEventSystem");
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
            OnLoadLate?.StableInvoke();

            Plugin.log.LogInfo("Loaded GUI");
        }
    }
}
