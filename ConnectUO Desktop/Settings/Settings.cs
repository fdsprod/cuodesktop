using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Win32;

namespace CUODesktop
{	
	public class Settings
	{
		public bool AllowSaving = true;
		public virtual string FilePath { get { return ""; } }

		protected Dictionary<string, object> _settingsTable = new Dictionary<string, object>();

		public Dictionary<string, object> SettingsTable { get { return _settingsTable; } set { _settingsTable = value; } } 

		public bool TryGet<T>(string name, out T value)
		{
			value = default(T);

			if (_settingsTable.ContainsKey(name))
			{
				SettingsEntry<T> entry = (SettingsEntry<T>)_settingsTable[name];
				value = entry.Value;
				return true;
			}

			return false;
		}

		public T Get<T>(string name)
		{
			if (_settingsTable.ContainsKey(name))
			{
				SettingsEntry<T> entry = (SettingsEntry<T>)_settingsTable[name];
				return entry.Value;
			}
			else
				return default(T);
		}

		public void Set<T>(string name, T value)
		{
			if (_settingsTable.ContainsKey(name))
			{
				SettingsEntry<T> entry = (SettingsEntry<T>)_settingsTable[name];
				entry.Value = value;
			}
			else
			{
				SettingsEntry<T> entry = new SettingsEntry<T>(default(T));
				entry.Value = value;
				_settingsTable.Add(name, entry);
				entry.OnSettingChanged += new EventHandler(OnSettingChanged);
			}
		}

		private void OnSettingChanged(object sender, EventArgs e)
		{
			if (AllowSaving)
				Save(FilePath);
		}

		public Settings() { }

		public virtual void OnAfterLoad() { }

		public virtual void Load(string filename)
		{
			StreamReader reader = null;

			if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
				Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

			try
			{
				reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read));
			}
			catch 
			{
				Save(filename);
				return;
			}

			string line;
			while ((line = reader.ReadLine()) != null)
			{
				string[] parts = line.Split('=');

				if (parts.Length != 2)
					continue;

				string key = parts[0].Trim();
				string value = parts[1].Trim();

				if (_settingsTable.ContainsKey(key))
				{
					object entry = _settingsTable[key];
					Type genericType = entry.GetType();
					Type type = genericType.GetGenericArguments()[0];

					object parsedValue = Parse(type, value);
					if (parsedValue != null)
					{
						AllowSaving = false;
						PropertyInfo propertyInfo = genericType.GetProperty("Value", type);
						propertyInfo.SetValue(entry, parsedValue, null);
						AllowSaving = true;
					}
				}
			}

			reader.Close();
		}

		public virtual void Save()
		{
			Save(FilePath);
		}

		public virtual void Save(string filename)
		{
			bool prevState = AllowSaving;
			AllowSaving = true;

			StreamWriter writer = null;
			try
			{
				if (!Directory.Exists(Path.GetFullPath(Path.GetDirectoryName(filename))))
					Directory.CreateDirectory(Path.GetFullPath(Path.GetDirectoryName(filename)));

				writer = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write));
			}
			catch
			{
				return;
			}

			List<string> list = new List<string>();

			foreach (string key in _settingsTable.Keys)
				list.Add(key);

			list.Sort();

			foreach (string key in list)
			{
				object entry = _settingsTable[key];
				Type genericType = entry.GetType();
				Type type = genericType.GetGenericArguments()[0];

				PropertyInfo propertyInfo = genericType.GetProperty("Value", type);

				object value = propertyInfo.GetValue(entry, null);

				writer.WriteLine("{0}={1}", key, value == null ? "" : value.ToString());
			}

			writer.Close();

			AllowSaving = prevState;
		}

		public static object Parse(Type type, string value)
		{
			try
			{
				if (IsEnum(type))
				{
					return Enum.Parse(type, value, true);
				}
				else if (IsParsable(type))
				{
					return ParseParsable(type, value);
				}
				else
				{
					object obj = value;

					if (value != null && value.StartsWith("0x"))
					{
						if (IsSignedNumeric(type))
							obj = Convert.ToInt64(value.Substring(2), 16);
						else if (IsUnsignedNumeric(type))
							obj = Convert.ToUInt64(value.Substring(2), 16);

						obj = Convert.ToInt32(value.Substring(2), 16);
					}

					if (obj == null && !type.IsValueType)
						return null;
					else
						return Convert.ChangeType(obj, type);
				}
			}
			catch
			{
				return null;
			}
		}

		private static Type[] _signedNumerics = new Type[]
			{
				typeof( Int64 ),
				typeof( Int32 ),
				typeof( Int16 ),
				typeof( SByte )
			};

		public static bool IsSignedNumeric(Type type)
		{
			for (int i = 0; i < _signedNumerics.Length; ++i)
				if (type == _signedNumerics[i])
					return true;

			return false;
		}

		private static Type[] _unsignedNumerics = new Type[]
			{
				typeof( UInt64 ),
				typeof( UInt32 ),
				typeof( UInt16 ),
				typeof( Byte )
			};

		public static bool IsUnsignedNumeric(Type type)
		{
			for (int i = 0; i < _unsignedNumerics.Length; ++i)
				if (type == _unsignedNumerics[i])
					return true;

			return false;
		}

		public static bool IsEnum(Type type)
		{
			return type.IsSubclassOf(typeof(Enum));
		}

		public static bool IsParsable(Type type)
		{
			return type.GetMethod("Parse", new Type[] { typeof(string) }) != null;
		}

		public static object ParseParsable(Type type, string value)
		{
			MethodInfo method = type.GetMethod("Parse", new Type[] { typeof(string) });
			object[] _ParseArgs = new object[] { value };
			return method.Invoke(null, _ParseArgs);
		}

		public virtual void SetDefaults() { }
	}
}
