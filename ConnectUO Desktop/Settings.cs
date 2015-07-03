using System;
using System.Collections.Generic;
using System.Text;

namespace CUODesktop
{
    public class Settings
    {
		/// <summary>
		/// Gets the max number of servers to be shown per page
		/// </summary>
        public static int ServersPerPage
        {
            get { return Properties.Settings.Default.ServersPerPage; }
        }
    }
}
