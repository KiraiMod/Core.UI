using BepInEx.Configuration;

namespace KiraiMod.Core.UI
{
    public static class Extensions
    {
        public static MemoryEntry<T> MemoryBind<T>(this ConfigFile file, ConfigDefinition definition, T value, ConfigDescription description = null) => new(file, definition, value, description);
        public static MemoryEntry<T> MemoryBind<T>(this ConfigFile file, string section, string key, T value, string description = null) => new(file, new(section, key), value, new(description));
    }
}
