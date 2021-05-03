using System;
using System.Linq;

namespace IPFixCollector.Modules.Netflow.V5
{
    public class V5Header
    {

        public ushort _version;
        public ushort _count;
        public uint _uptime;
        public uint _secs;
        public uint _nsecs;
        public uint _sequence;
        public byte _type;
        public uint _id;
        public ushort _sampling_interval;

        private Byte[] _bytes;


        public UInt16 Version
        {
            get
            {
                return this._version;
            }
        }
        public UInt16 Count
        {
            get
            {
                return this._count;
            }
        }
        public TimeSpan UpTime
        {
            get
            {
                return new TimeSpan((long)this._uptime * 10000);
            }
        }
        public DateTime Secs
        {
            get
            {
                return new DateTime(1970, 1, 1).AddSeconds(this._secs);
            }
        }
        public DateTime NSecs
        {
            get
            {
                return new DateTime(1970, 1, 1).AddMilliseconds(this._nsecs);
            }
        }
        public UInt32 Sequence
        {
            get
            {
                return this._sequence;
            }
        }
        public UInt32 Type
        {
            get
            {
                return this._type;
            }
        }
        public UInt32 ID
        {
            get
            {
                return this._id;
            }
        }
        public UInt32 SampleInterval
        {
            get
            {
                return this._sampling_interval;
            }
        }

        public V5Header(Byte[] bytes)
        {
            this._bytes = bytes;
            this.Parse();
        }

        private void Parse()
        {
            if (this._bytes.Length == 24)
            {
                byte[] reverse = this._bytes.Reverse().ToArray();

                this._version = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 0);
                this._count = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 2);

                this._uptime = BitConverter.ToUInt32(reverse, this._bytes.Length - sizeof(UInt32) - 4);
                this._secs = BitConverter.ToUInt32(reverse, this._bytes.Length - sizeof(Int32) - 8);
                this._sequence = BitConverter.ToUInt32(reverse, this._bytes.Length - sizeof(Int32) - 12);
                this._id = BitConverter.ToUInt32(reverse, this._bytes.Length - sizeof(Int32) - 16);
            }
        }
    }
}
