using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace CUODesktop
{
	public class PageHandlers
	{
		private static Dictionary<string, PageHandler> _pageHandlers;

		/// <summary>
		/// Dont worry about it. You dont want to use it.
		/// </summary>
		public static Dictionary<string, PageHandler> Handlers
		{
			get { return _pageHandlers; }
		}

		static PageHandlers()
		{
			_pageHandlers = new Dictionary<string, PageHandler>();

			Register("play", new OnPageRequestedHandler(OnPlayRequested), null, true);
			Register("addlocal", new OnPageRequestedHandler(OnEditLocalRequested), null, false);
			Register("addfavorite", new OnPageRequestedHandler(OnAddFavoriteRequested), null);
			Register("removefavorite", new OnPageRequestedHandler(OnRemoveFavoriteRequested), null);
			Register("resetpatches", new OnPageRequestedHandler(OnResetPatchesRequested), null);			
		}

		private static void Register(string pageId, OnPageRequestedHandler handler, PageCompiler compiler)
		{
			_pageHandlers[pageId] = new PageHandler(pageId, handler, compiler);
		}

		private static void Register(string pageId, OnPageRequestedHandler handler, PageCompiler compiler, bool requireValidation)
		{
			_pageHandlers[pageId] = new PageHandler(pageId, handler, requireValidation, compiler);
		}

		private static void OnResetPatchesRequested(string request, PageCompiler compiler, ref Socket socket)
		{
			string id = Utility.ParseUrl(request, "id");
			IEntry entry = ServerList.GetServerById(id);

			if ( entry != null && MessageBox.Show("Are you sure you want to reset patches for " + entry.Name + "?", "ConnectUO Desktop", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
			{
				string name = Uri.EscapeDataString(entry.Name);
				string path = Path.Combine(AppSettings.Current.Get<string>("PatchingPath"), Path.Combine("Servers", name));
							
				if (Directory.Exists(path))
				{	
					bool success = true;
					try
					{
						Directory.Delete(path, true);
					}
					catch
					{
						success = false;
					}

					if (success)
						MessageBox.Show("Patches for " + entry.Name + " were reset successfully.", "ConnectUO Desktop");
					else
						MessageBox.Show("An error occurred while trying to reset the patches for " + entry.Name + ".\n This can be caused by the client still running or some other application that is currently using\n the files in the patch directory for this server.\n Please be sure that nothing is accessing these files and try again.", "ConnectUO Desktop");
				}
			}

			Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);
		}

		private static void OnPlayRequested(string request, PageCompiler compiler, ref Socket socket)
		{
			Utility.Play(Utility.ParseUrl(request, "id"), Utility.ParseUrl(request, "type"), socket);
		}

		private static void OnEditLocalRequested(string request, PageCompiler compiler, ref Socket socket)
		{
			if (Utility.ParseUrl(request, "mode") != string.Empty)
			{
				MainForm.Instance.addLocalServerToolStripMenuItem_Click(new object[] { request, socket }, null);
			}
			else
				Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);

		}

		private static void OnAddFavoriteRequested(string request, PageCompiler compiler, ref Socket socket)
		{
			Favorites.AddFavorite(Utility.ParseUrl(request, "id"));	
			Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);
		}

		private static void OnRemoveFavoriteRequested(string request, PageCompiler compiler, ref Socket socket)
		{
			Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);
			Favorites.RemoveFavorite(Uri.UnescapeDataString(Utility.ParseUrl(request, "id")));			
		}
	}
}
