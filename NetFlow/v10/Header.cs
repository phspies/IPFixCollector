using System;
using System.Linq;

namespace IPFixCollector.Modules.Netflow.v10
{
    public class V10Header
	{
        private byte[] _bytes;

        public ushort Version { get; private set; }
        public uint DomainID { get; private set; }
        public ushort Lenght { get; private set; }
        public DateTime ExportTime
		{
			get
			{
                return new DateTime(1970, 1, 1).AddSeconds(Exporttime);
			}
		}
        public uint SequinceNumber { get; private set; }
        public uint Sequence
		{
			get
			{
                return DomainID;
			}
		}

        public uint Exporttime { get; set; }

        public V10Header(Byte[] bytes)
        {
            _bytes = bytes;
            Parse();
        }

        private void Parse()
        {
            if(_bytes.Length == 16)
            {
                byte[] reverse = _bytes.Reverse().ToArray();
                Version = BitConverter.ToUInt16(reverse, _bytes.Length - sizeof(short) - 0);
                Lenght = BitConverter.ToUInt16(reverse, _bytes.Length - sizeof(short) - 2);
                Exporttime = BitConverter.ToUInt32(reverse, _bytes.Length - sizeof(uint) - 4);
                SequinceNumber = BitConverter.ToUInt32(reverse, _bytes.Length - sizeof(int) - 8);
                DomainID = BitConverter.ToUInt32(reverse, _bytes.Length - sizeof(int) - 12);
            }
        }
	}
}
