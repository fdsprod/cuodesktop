using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System
{
	public class WindowProcessStream : ProcessStream
	{
		[DllImport( "User32" )]
		private static extern int IsWindow( IntPtr window );

		[DllImport( "User32" )]
		private static extern int GetWindowThreadProcessId( IntPtr window, ref IntPtr processID );

		private IntPtr m_Window, m_ProcessID;

		public IntPtr Window{ get{ return m_Window; } set{ m_Window = value; m_ProcessID = IntPtr.Zero; } }

		public WindowProcessStream( IntPtr window )
		{
			m_Window = window;
		}

		public override IntPtr ProcessID
		{
			get
			{
				if ( IsWindow( m_Window ) != 0 && m_ProcessID != IntPtr.Zero )
					return m_ProcessID;

				GetWindowThreadProcessId( m_Window, ref m_ProcessID );

				return m_ProcessID;
			}
		}
	}
}