using System;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace CUODesktop.PatchLib
{
    public class BodyConverter
    {
        private static int[] m_Table1;
        private static int[] m_Table2;
        private static int[] m_Table3;
        private static int[] m_Table4;

        private BodyConverter()
        {
        }

        static BodyConverter()
        {
			string uoPath;
			if( (uoPath = Client.DetectClient( Client.TwoDClient )) == Client.NotFound &&
				(uoPath = Client.DetectClient( Client.ThreeDClient)) == Client.NotFound )
				throw new Exception( "Ultima Online does not appear to be installed on this machine.");

            string path =  Path.Combine( Path.GetDirectoryName(uoPath), "bodyconv.def");

            if (path == null)
                return;

            ArrayList list1 = new ArrayList(), list2 = new ArrayList(), list3 = new ArrayList(), list4 = new ArrayList(), list5 = new ArrayList();
            int max1 = 0, max2 = 0, max3 = 0, max4 = 0, max5 = 0;

            using (StreamReader ip = new StreamReader(path))
            {
                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;

                    try
                    {
                        string[] split = line.Split('\t');

                        int original = System.Convert.ToInt32(split[0]);
                        int anim2 = System.Convert.ToInt32(split[1]);
                        int anim3;
                        int anim4;
                        int anim5;

                        try
                        {
                            anim3 = System.Convert.ToInt32(split[2]);
                        }
                        catch
                        {
                            anim3 = -1;
                        }

                        try
                        {
                            anim4 = System.Convert.ToInt32(split[3]);
                        }
                        catch
                        {
                            anim4 = -1;
                        }

                        try
                        {
                            anim5 = System.Convert.ToInt32(split[4]);
                        }
                        catch
                        {
                            anim5 = -1;
                        }

                        if (anim2 != -1)
                        {
                            if (anim2 == 68)
                                anim2 = 122;

                            if (original > max1)
                                max1 = original;

                            list1.Add(original);
                            list1.Add(anim2);
                        }

                        if (anim3 != -1)
                        {
                            if (original > max2)
                                max2 = original;

                            list2.Add(original);
                            list2.Add(anim3);
                        }

                        if (anim4 != -1)
                        {
                            if (original > max4)
                                max4 = original;

                            list4.Add(original);
                            list4.Add(anim4);
                        }

                        if (anim5 != -1)
                        {
                            if (original > max5)
                                max5 = original;

                            list5.Add(original);
                            list5.Add(anim4);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            m_Table1 = new int[max1 + 1];

            for (int i = 0; i < m_Table1.Length; ++i)
                m_Table1[i] = -1;

            for (int i = 0; i < list1.Count; i += 2)
                m_Table1[(int)list1[i]] = (int)list1[i + 1];

            m_Table2 = new int[max2 + 1];

            for (int i = 0; i < m_Table2.Length; ++i)
                m_Table2[i] = -1;

            for (int i = 0; i < list2.Count; i += 2)
                m_Table2[(int)list2[i]] = (int)list2[i + 1];

            m_Table3 = new int[max4 + 1];

            for (int i = 0; i < m_Table3.Length; i++)
                m_Table3[i] = -1;

            for (int i = 0; i < list4.Count; i += 2)
                m_Table3[(int)list4[i]] = (int)list4[i + 1];

            m_Table4 = new int[max5 + 1];

            for (int i = 0; i < m_Table4.Length; i++)
                m_Table4[i] = -1;

            for (int i = 0; i < list5.Count; i += 2)
                m_Table4[(int)list5[i]] = (int)list5[i + 1];
        }

        /// <summary>
        /// Checks to see if <paramref name="body" /> is contained within the mapping table.
        /// </summary>
        /// <returns>True if it is, false if not.</returns>
        public static bool Contains(int body)
        {
            if (m_Table1 != null && body >= 0 && body < m_Table1.Length && m_Table1[body] != -1)
                return true;

            if (m_Table2 != null && body >= 0 && body < m_Table2.Length && m_Table2[body] != -1)
                return true;

            if (m_Table3 != null && body >= 0 && body < m_Table3.Length && m_Table3[body] != -1)
                return true;

            if (m_Table4 != null && body >= 0 && body < m_Table4.Length && m_Table4[body] != -1)
                return true;

            return false;
        }

        /// <summary>
        /// Attempts to convert <paramref name="body" /> to a body index relative to a file subset, specified by the return value.
        /// </summary>
        /// <returns>A value indicating a file subset:
        /// <list type="table">
        /// <listheader>
        /// <term>Return Value</term>
        /// <description>File Subset</description>
        /// </listheader>
        /// <item>
        /// <term>1</term>
        /// <description>Anim.mul, Anim.idx (Standard)</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Anim2.mul, Anim2.idx (LBR)</description>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <description>Anim3.mul, Anim3.idx (AOS)</description>
        /// </item>
        /// <item>
        /// <term>4</term>
        /// <description>Anim4.mul, Anim4.idx (SE)</description>
        /// </item>
        /// <item>
        /// <term>5</term>
        /// <description>Anim5.mul, Anim5.idx (ML)</description>
        /// </item>
        /// </list>
        /// </returns>
        public static int Convert(int body)
        {
            if (m_Table1 != null && body >= 0 && body < m_Table1.Length)
            {
                int val = m_Table1[body];

                if (val != -1)
                {
                    body = val;
                    return 2;
                }
            }

            if (m_Table2 != null && body >= 0 && body < m_Table2.Length)
            {
                int val = m_Table2[body];

                if (val != -1)
                {
                    body = val;
                    return 3;
                }
            }

            if (m_Table3 != null && body >= 0 && body < m_Table3.Length)
            {
                int val = m_Table3[body];

                if (val != -1)
                {
                    body = val;
                    return 4;
                }
            }

            if (m_Table4 != null && body >= 0 && body < m_Table4.Length)
            {
                int val = m_Table4[body];

                if (val != -1)
                {
                    body = val;
                    return 5;
                }
            }

            return 1;
        }
    }
}
