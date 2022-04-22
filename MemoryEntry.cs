using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KiraiMod.Core.UI
{
    public class MemoryEntry<T> : ConfigEntryBase
    {
        private T _typedValue;

        public T Value
        {
            get => _typedValue;
            set
            {
                value = ClampValue(value);
                if (!Equals(_typedValue, value))
                {
                    _typedValue = value;
                    OnSettingChanged(this);
                }
            }
        }

        public event EventHandler SettingChanged;

        public MemoryEntry(ConfigFile file, ConfigDefinition definition, T value, ConfigDescription description = null) : base(file, definition, typeof(T), value, description)
        {
            ((Dictionary<ConfigDefinition, ConfigEntryBase>)Hooks.Entries.GetValue(file))[definition] = this;
            file.SettingChanged += (object sender, SettingChangedEventArgs args) =>
            {
                if (args.ChangedSetting == this)
                    SettingChanged?.Invoke(sender, args);
            };
        }

        public override object BoxedValue
        {
            get => Value;
            set => Value = (T)value;
        }

        private static class Hooks
        {
            static Hooks() => Harmony.CreateAndPatchAll(typeof(Hooks));

            public static FieldInfo Entries = typeof(ConfigFile).GetField("<Entries>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

            [HarmonyPrefix, HarmonyPatch(typeof(ConfigFile), nameof(ConfigFile.Save), MethodType.Normal)]
            public static void PreSave(ConfigFile __instance, ref (KeyValuePair<ConfigDefinition, ConfigEntryBase>[], Dictionary<ConfigDefinition, ConfigEntryBase>) __state)
            {
                var entries = (Dictionary<ConfigDefinition, ConfigEntryBase>)Entries.GetValue(__instance);
                var removal = entries.Where(x => x.Value.GetType().GetGenericTypeDefinition() == typeof(MemoryEntry<>)).ToArray();

                foreach (var entry in removal)
                    entries.Remove(entry.Key);

                __state = (removal, entries);
            }

            [HarmonyPostfix, HarmonyPatch(typeof(ConfigFile), nameof(ConfigFile.Save), MethodType.Normal)]
            public static void PostSave((KeyValuePair<ConfigDefinition, ConfigEntryBase>[], Dictionary<ConfigDefinition, ConfigEntryBase>) __state)
            {
                foreach (var entry in __state.Item1)
                    __state.Item2.Add(entry.Key, entry.Value);
            }
        }
    }
}
