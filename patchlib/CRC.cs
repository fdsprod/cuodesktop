using System;
using System.IO;
using System.Security.Cryptography;

namespace CUODesktop.PatchLib
{
    /// <summary>
    /// Class used to get a 32-bit CRC value of a file.
    /// </summary>
    public class CRC32
    {      
        private uint[] crc32Table;
        private const int BUFFER_SIZE = 1024;

        public event ProgressChangeHandler PercentCompleteChange;

        /// <summary>
        /// Returns a uint value CRC of a file
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>uint crc32</returns>
        public uint GetCrc32(System.IO.Stream stream)
        {
           unchecked
           {
                uint crc32Result;
                crc32Result = 0xFFFFFFFF;
                byte[] buffer = new byte[BUFFER_SIZE];
                int readSize = BUFFER_SIZE;

                int count = stream.Read(buffer, 0, readSize);
                while (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        crc32Result = ((crc32Result) >> 8) ^ crc32Table[(buffer[i]) ^
                        ((crc32Result) & 0x000000FF)];
                    }
                    count = stream.Read(buffer, 0, readSize);

                    if (PercentCompleteChange != null) 
                        PercentCompleteChange(this, new ProgressChangeEventArgs((int)((stream.Position * 100) / stream.Length), stream.Position, stream.Length));
                }

				stream.Close();
                return ~crc32Result;
           }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CRC32()
        {
            unchecked
            {
                UInt32 dwPolynomial = 0xEDB88320;
                UInt32 i, j;

                crc32Table = new UInt32[256];

                UInt32 dwCrc;
                for( i = 0; i < 256; i++ )
                {
                    dwCrc = i;
                    for( j = 8; j > 0; j-- )
                    {
                        if( ( dwCrc & 1 ) == 1 )
                        {
                            dwCrc = ( dwCrc >> 1 ) ^ dwPolynomial;
                        }
                        else
                        {
                            dwCrc >>= 1;
                        }
                    }
                    crc32Table[i] = dwCrc;
                }
            }
        }		
	}
}
