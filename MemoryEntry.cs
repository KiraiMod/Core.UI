using BepInEx.Configuration;
using System;

namespace KiraiMod.Core.UI
{
    public class MemoryEntry<T> : UnsavedEntry
    {
        public T _value;
        public T Value
        {
            get => _value;
            set
            {
                value = ClampValue(value);
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnSettingChanged(this);
                }
            }
        }

        public event Action<T> ValueChanged;

        public MemoryEntry(ConfigFile file, ConfigDefinition definition, T value, ConfigDescription description = null) 
            : base(file, definition, typeof(T), value, description) =>
            SettingChanged += (sender, args) => ValueChanged?.Invoke(Value);

        public override object BoxedValue
        {
            get => Value;
            set => Value = (T)value;
        }
    }
}
