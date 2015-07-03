using System;
using System.Collections.Generic;
using System.Text;

namespace CUODesktop
{
	public class Authentication
	{
		private static uint _authCode;

		internal static uint AuthCode
		{
			get
			{
				return _authCode;
			}
		}

		internal static void UpdateAuth()
		{
			_authCode = (uint)DateTime.Now.GetHashCode();
		}

		internal static bool ValidateAuth(uint auth)
		{
			return _authCode == auth;
		}
	}
}
