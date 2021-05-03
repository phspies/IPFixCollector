using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IPFixCollector.Modules.Netflow.v10
{
	public class V10Header
	{
		private UInt16 _version;
		private UInt16 _lenght;
		private UInt32 _exporttime;
		private UInt32 _sequencenumber;
		private UInt32 _obdomainid;

        private Byte[] _bytes;

		public UInt16 Version
		{
			get
			{
				return this._version;
			}
		}
        public UInt32 DomainID
        {
            get
            {
                return this._obdomainid;
            }
        }
        public UInt16 Lenght
		{
			get
			{
                return this._lenght;
			}
		}
        public DateTime ExportTime
		{
			get
			{
                return new DateTime(1970, 1, 1).AddSeconds(this._exporttime);
			}
		}
        public UInt32 SequinceNumber
		{
			get
			{
                return this._sequencenumber;
			}
		}
		public UInt32 Sequence
		{
			get
			{
                return this._obdomainid;
			}
		}

        public V10Header(Byte[] bytes)
        {
            this._bytes = bytes;
            this.Parse();
        }

        private void Parse()
        {
            if(this._bytes.Length == 16)
            {
                byte[] reverse = this._bytes.Reverse().ToArray();

                this._version = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 0);
                this._lenght = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 2);

                this._exporttime = BitConverter.ToUInt32(reverse, this._bytes.Length - sizeof(UInt32) - 4);
                this._sequencenumber = BitConverter.ToUInt32(reverse, this._bytes.Length - sizeof(Int32) - 8);
                this._obdomainid = BitConverter.ToUInt32(reverse, this._bytes.Length - sizeof(Int32) - 12);
            }
        }
	}
}
