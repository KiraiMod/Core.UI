using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KiraiMod.Core.UI
{
    public abstract class UnsavedEntry : ConfigEntryBase
    {
        protected UnsavedEntry(ConfigFile file, ConfigDefinition definition, Type type, object value, ConfigDescription description) 
            : base(file, definition, type, value, description)
        {
            ((Dictionary<ConfigDefinition, ConfigEntryBase>)Hooks.Entries.GetValue(file))[definition] = this;

            file.SettingChanged += (object sender, SettingChangedEventArgs args) =>
            {
                if (args.ChangedSetting == this)
                    SettingChanged?.Invoke(sender, args);
            };
        }

        public event EventHandler SettingChanged;

        public static class Hooks
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
