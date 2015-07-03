using System;
using System.Collections.Generic;
using System.IO;

namespace PatchlistBuilder
{
    public class PatchlistFile
    {
        private string _fileName;
        private string _url;
        private uint _crc;
		private long _size;

		public uint CRC { get { return _crc; } }
		public long Size { get { return _size; } }
		public string Url { get { return _url; } }
        public string FileName { get { return _fileName; } }

        public PatchlistFile(string url, uint crc, long size)
        {
            _fileName = Path.GetFileName(url);
            _url = url;
            _crc = crc;
			_size = size;
        }

        public override string ToString()
        {
			return _url + "|" + _crc.ToString() + "|" + _size.ToString();
        }
    }
}
