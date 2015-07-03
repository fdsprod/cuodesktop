using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CUODesktop
{
	public class Templates
	{
		private static string _templateDirectory;
		private static Dictionary<string, Template> _templates;
		private static Template _currentTemplate;

		public static string TemplateDirectory { get { return _templateDirectory; } }
		public static Dictionary<string, Template> LoadedTemplates { get { return _templates; } }
		public static Template CurrentTemplate
		{ 
			get 
			{
				if( _currentTemplate == null )
					LoadTemplate(AppSettings.Current.Get<string>("Template"));

				return _currentTemplate; 
			} 
		}
			
		static Templates()
		{
			_templates = new Dictionary<string, Template>();
			_templateDirectory = Core.TemplatesDirectory;

			LoadTemplates();			
		}

		public static string GetHTMLList()
		{
			string html = "<option selected=\"selected\">" + CurrentTemplate.Name + "</option>";

			List<Template> templates = new List<Template>(_templates.Values);
			for( int i = 0; i < templates.Count; i++ )
				if( templates[i] != _currentTemplate )
					html += "<option>" + templates[i].Name + "</option>";

			return html;
		}

		public static void LoadTemplate(string name)
		{
			_currentTemplate = _templates[name.ToLower()];
		}

		public static void LoadTemplates()
		{
			DirectoryInfo dir = new DirectoryInfo(_templateDirectory);
			DirectoryInfo[] dirs = dir.GetDirectories();

			if( dirs.Length < 1 )
				throw new Exception("Unable to find a usable template.");

			for( int i = 0; i < dirs.Length; i++ )
				_templates.Add(dirs[i].Name.ToLower(), new Template(dirs[i].FullName, dirs[i].Name));
		}

		internal static bool IsValidTemplateName(string name)
		{
			List<Template> templates = new List<Template>(Templates.LoadedTemplates.Values);

			for (int i = 0; i < templates.Count; i++)
				if (templates[i].Name == name)
					return true;

			return false;
		}
	}	
}
