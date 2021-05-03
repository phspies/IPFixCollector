using System;
using System.Linq;

namespace IPFixCollector.Modules.Netflow.V5
{
    public class PacketV5
    {
        private uint _srcAddr;    /* source IP address */
        private uint _dstAddr;    /* destination IP address */
        private uint _nextHop;    /* next hop router's IP address */
        private ushort _in_if;        /* Input interface index */
        private ushort _out_if;       /* Output interface index */
        private uint _pkts;       /* packets sent in duration */
        private uint _octets;     /* octets sent in duration */
        private uint _uptime_first;      /* SysUptime at start of flow */
        private uint _uptime_last;       /* and of Last packet of flow */
        private ushort _srcPort;  /* TCP/UDP source port number or equivalent */
        private ushort _dstPort;  /* TCP/UDP dest port number or equivalent */
        private byte _pad;
        private byte _tcp_flags;  /* bitwise OR of all TCP flags in flow; 0x10 */
                                  /*	for non-TCP flows */
        private byte _prot;       /* IP Protocol, e.g., 6=TCP, 17=UDP, ... */
        private byte _tos;        /* IP Type-of-Service */
        //private ushort _dst_as;       /* originating AS of destination address */
        //private ushort _src_as;       /* originating AS of source address */
        //private byte _dst_mask;   /* destination address prefix mask bits */
        //private byte _src_mask;   /* source address prefix mask bits */
        //private ushort _reserved;

        private V5Header _header;
        private Byte[] _bytes;


        public uint SrcAddr { get { return this._srcAddr; } }
        public uint DstAddr { get { return this._dstAddr; } }
        public uint NextHop { get { return this._nextHop; } }
        public uint InInf { get { return this._in_if; } }
        public uint OutInf { get { return this._out_if; } }
        public uint Packets { get { return this._pkts; } }
        public uint Octets { get { return this._octets; } }
        public long UptimeFirst { get { return this._uptime_first; } }
        public long UptimeLast { get { return this._uptime_last; } }
        public uint SrcPort { get { return this._srcPort; } }
        public int DstPort { get { return this._dstPort; } }
        public uint TcpFlags { get { return this._tcp_flags; } }
        public uint Prot { get { return this._prot; } }
        public uint TOS { get { return this._tos; } }
        //public uint DstAS { get { return this._dst_as; } }
        //public uint SrcAS { get { return this._src_as; } }
        //public uint DstMask { get { return this._dst_mask; } }
        //public uint SrcMask { get { return this._src_mask; } }


        public V5Header Header { get { return this._header; } }

        public PacketV5(Byte[] bytes)
        {
            this._bytes = bytes;

            Int32 length = _bytes.Length - 48;

            Byte[] header = new Byte[24];
            Byte[] packet = new Byte[length];

            Array.Copy(_bytes, 0, header, 0, 24);
            Array.Copy(_bytes, 24, packet, 0, length);

            this._header = new V5Header(header);

            byte[] reverse = packet.Reverse().ToArray();

            this._srcAddr = BitConverter.ToUInt32(reverse, packet.Length - sizeof(UInt32) - 0);
            this._dstAddr = BitConverter.ToUInt32(reverse, packet.Length - sizeof(UInt32) - 4);
            this._nextHop = BitConverter.ToUInt32(reverse, packet.Length - sizeof(UInt32) - 8);
            this._in_if = BitConverter.ToUInt16(reverse, packet.Length - sizeof(UInt16) - 12);
            this._out_if = BitConverter.ToUInt16(reverse, packet.Length - sizeof(UInt16) - 14);
            this._pkts = BitConverter.ToUInt32(reverse, packet.Length - sizeof(UInt32) - 16);
            this._octets = BitConverter.ToUInt32(reverse, packet.Length - sizeof(UInt32) - 20);
            if (reverse.Length > 24)
            {
                this._uptime_first = BitConverter.ToUInt32(reverse, packet.Length - sizeof(UInt32) - 24);
                this._uptime_last = BitConverter.ToUInt32(reverse, packet.Length - sizeof(UInt32) - 28);
                this._srcPort = BitConverter.ToUInt16(reverse, packet.Length - sizeof(UInt16) - 32);
                this._dstPort = BitConverter.ToUInt16(reverse, packet.Length - sizeof(UInt16) - 34);
                this._pad = reverse[36];
                this._tcp_flags = reverse[37];
                this._prot = reverse[38];
                this._tos = reverse[39];
            }
        }
    };
}
