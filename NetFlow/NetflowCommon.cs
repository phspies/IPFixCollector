using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPFixCollector.Modules.Netflow
{
    class NetflowCommon
    {
        public ushort _version;
        private byte[] _bytes;

        public NetflowCommon(byte[] bytes)
        {
            _bytes = bytes;
            byte[] reverse = _bytes.Reverse().ToArray();
            _version = BitConverter.ToUInt16(reverse, _bytes.Length - sizeof(short) - 0);
        }
    }
}
