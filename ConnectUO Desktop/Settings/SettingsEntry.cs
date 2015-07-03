using System;
using System.Collections.Generic;

namespace CUODesktop
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class SettingsEntry<T>
    {
        private T _default;
        private T _value;

		public event EventHandler OnSettingChanged;

        public SettingsEntry(T defaultValue)
        {
            _value = _default = defaultValue;
        }

        public T Default
        {
            get
            {
                return _default;
            }
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

				if (OnSettingChanged != null)
					OnSettingChanged(this, null);
            }
        }
    }
}
