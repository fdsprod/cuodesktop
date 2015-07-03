using System;
using System.Collections.Generic;
using System.IO;

namespace CUODesktop
{
	public class Favorites
	{
		private static List<FavoriteEntry> _favorites;

		/// <summary>
		/// Collection of favorites servers
		/// </summary>
		public static List<FavoriteEntry> MyFavorites 
		{
			get 
			{ 
				if( _favorites == null )
					Load(); 

				return _favorites; 
			} 
		}

		private static List<CustomEntry> _customs;
		
		/// <summary>
		/// Collection of custom servers
		/// </summary>
		public static List<CustomEntry> Customs
		{
			get
			{
				if( _customs == null )
					Load();

				return _customs; 
			}
		}

		private static void Load()
		{
			_favorites = new List<FavoriteEntry>();
			_customs = new List<CustomEntry>();

			if( File.Exists( Path.Combine(Core.BaseDirectory, "favorites.bin") ) )
			{
				BinaryReader reader = new BinaryReader(new FileStream(Path.Combine(Core.BaseDirectory, "favorites.bin"), FileMode.OpenOrCreate));

				int ver = reader.ReadInt32();

				switch( ver )
				{
					case 1:
					{
						int count = reader.ReadInt32();

						for( int i = 0; i < count; i++ )
						{
							CustomEntry entry = new CustomEntry();
							entry.Deserialize(reader);
							_customs.Add(entry);
						}

						goto case 0;
					}
					case 0:
					{
						int count = reader.ReadInt32();

						for( int i = 0; i < count; i++ )
						{
							IEntry entry = ServerList.GetServerById(reader.ReadInt32().ToString());

							if( entry == null )
								continue;

							_favorites.Add(new FavoriteEntry(entry.Element));
						}

						break;
					}
				}

				reader.Close();
			}
		}

		private static void Save()
		{
			BinaryWriter writer = new BinaryWriter(new FileStream(Path.Combine(Core.BaseDirectory, "favorites.bin"), FileMode.OpenOrCreate));

			writer.Write((int)1);

			writer.Write((int)Customs.Count);
			for( int i = 0; i < Customs.Count; i++ )
				Customs[i].Serialize(writer);

			writer.Write((int)MyFavorites.Count);
			for( int i = 0; i < MyFavorites.Count; i++ )
				writer.Write(int.Parse(MyFavorites[i].Id));

			writer.Close();
		}

		/// <summary>
		/// Adds the server to the favorites list.
		/// </summary>
		/// <param name="id">server id</param>
		public static void AddFavorite(string id)
		{
			int value;

			if( Int32.TryParse(id, out value) && !FavoriteExists(value) )
			{
				IEntry entry = ServerList.GetServerById(id);

				if( entry != null )
				{
					MyFavorites.Add(new FavoriteEntry(entry.Element));
					Save();
				}
			}
		}

		private static bool FavoriteExists(int value)
		{
			int index;

			return FavoriteExists(value, out index);
		}

		private static bool FavoriteExists(int value, out int index)
		{
			index = -1;
			for( int i = 0; i < MyFavorites.Count; i++ )
				if( MyFavorites[i].Id == value.ToString() )
				{
					index = i;
					return true;
				}

			return false;
		}

		/// <summary>
		/// Adds a custom server.
		/// </summary>
		/// <param name="entry"></param>
		public static void AddCustom(CustomEntry entry)
		{
			bool add = true;

			for( int i = 0; i < Customs.Count; i++ )
				if( entry.Name == Customs[i].Name )
				{
					add = false;
					break;
				}

			if( add )
				Customs.Add(entry);

			Save();
		}

		/// <summary>
		/// Removes a custom server
		/// </summary>
		/// <param name="name"></param>
		public static void RemoveCustom(string name)
		{
			CustomEntry entry = null;

			for( int i = 0; i < Customs.Count; i++ )
				if( name == Customs[i].Name )
				{
					entry = Customs[i];
					break;
				}

			if( entry != null )
			{
				Customs.Remove(entry);
				Save();
			}
		}

		/// <summary>
		/// Removes a server from the favorites list
		/// </summary>
		/// <param name="id"></param>
		public static void RemoveFavorite(string id)
		{
			int value;
			int index = -1;

			if( Int32.TryParse(id, out value) && FavoriteExists(value, out index) && index != -1 )
			{
				MyFavorites.RemoveAt(index);
				Save();
			}
			else
				RemoveCustom(id);

		}
		
		/// <summary>
		/// Returns a custom entry based on the name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static CustomEntry GetEntryByName(string name)
		{
			name = Uri.EscapeUriString(name);
			CustomEntry entry = null;
			for( int i = 0; i < Customs.Count; i++ )
				if( Customs[i].Name == name )
				{
					entry = Customs[i];
					break;
				}

			return entry;
		}

		public static IEntry GetFavorite(string id)
		{
			for (int i = 0; i < _favorites.Count; i++)
				if (_favorites[i].Id == id)
					return _favorites[i];

			return null;
		}
	}
}
