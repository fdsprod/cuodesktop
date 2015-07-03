using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CUODesktop
{
	public delegate void OnPageRequestedHandler(string request, PageCompiler compiler, ref Socket socket);

	public class PageHandler
	{
		private string _pageId;
		private OnPageRequestedHandler _onRequest;
		private bool _requireValidation;
		private PageCompiler _compiler;

		/// <summary>
		/// Page ID used to call the hander
		/// </summary>
		public string PageId { get { return _pageId; } }
		/// <summary>
		/// Handler delegate for the PageHandler
		/// </summary>
		public OnPageRequestedHandler OnRequest { get { return _onRequest; } }
		/// <summary>
		/// Shhhhh, secrets... :)
		/// </summary>
		public bool RequireValidation { get { return _requireValidation; } }
		/// <summary>
		/// Compiler used to compile the specified page
		/// </summary>
		public PageCompiler Compiler { get { return _compiler; } }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="handler"></param>
		/// <param name="compiler"></param>
		public PageHandler(string pageId, OnPageRequestedHandler handler, PageCompiler compiler)
			: this(pageId, handler, false, compiler)
		{
			_pageId = pageId;
			_onRequest = handler;
			_compiler = compiler;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="handler"></param>
		/// <param name="requireValidation"></param>
		/// <param name="compiler"></param>
		public PageHandler(string pageId, OnPageRequestedHandler handler, bool requireValidation, PageCompiler compiler)
		{
			_pageId = pageId;
			_onRequest = handler;
			_requireValidation = requireValidation;
			_compiler = compiler;
		}

		/// <summary>
		/// Returns true/false based on Validation Authentication
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public bool Validate(string request)
		{
			if( _requireValidation )
			{
				uint auth;

				uint.TryParse(Utility.ParseUrl(request, "auth"), out auth);

				return ( Authentication.ValidateAuth(auth) );
			}

			return true;
		}
	}
}