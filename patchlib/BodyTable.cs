using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;


namespace CUODesktop.PatchLib
{
    public class BodyTable
    {
        public static Hashtable m_Entries;

        static BodyTable()
        {
            m_Entries = new Hashtable();
			
			string uoPath;

			if( ( uoPath = Client.DetectClient(Client.TwoDClient) ) == Client.NotFound &&
				( uoPath = Client.DetectClient(Client.ThreeDClient) ) == Client.NotFound )
				throw new Exception("Ultima Online does not appear to be installed on this machine.");

            string filePath = Path.Combine(Path.GetDirectoryName(uoPath), "body.def");

            if (filePath == null)
                return;

            StreamReader def = new StreamReader(filePath);

            string line;

            while ((line = def.ReadLine()) != null)
            {
                if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                    continue;

                try
                {
                    int index1 = line.IndexOf(" {");
                    int index2 = line.IndexOf("} ");

                    string param1 = line.Substring(0, index1);
                    string param2 = line.Substring(index1 + 2, index2 - index1 - 2);
                    string param4 = line.Substring(index2 + 2);
                    //					string param5 = line.Substring( index2 + 2 );

                    int indexOf = param2.IndexOf(',');

                    if (indexOf > -1)
                        param2 = param2.Substring(0, indexOf).Trim();

                    int iParam1 = Convert.ToInt32(param1);
                    int iParam2 = Convert.ToInt32(param2);
                    int iParam4 = Convert.ToInt32(param4);
                    //					int iParam5 = Convert.ToInt32( param5 );

                    m_Entries[iParam1] = new BodyTableEntry(iParam2, iParam1, iParam4);
                }
                catch
                {
                }
            }
        }
    }
}
