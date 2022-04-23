using BepInEx.Configuration;

namespace KiraiMod.Core.UI
{
    public static class Extensions
    {
        public static MemoryEntry<T> MemoryBind<T>(this ConfigFile file, string section, string key, T value) => new(file, new(section, key), value);
        public static MemoryEntry<T> MemoryBind<T>(this ConfigFile file, string section, string key, T value, string description = null) => new(file, new(section, key), value, new(description));
    }
}
