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
        private Byte[] _bytes;

        public NetflowCommon(Byte[] bytes)
        {
            this._bytes = bytes;
            byte[] reverse = this._bytes.Reverse().ToArray();
            this._version = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 0);
        }
    }
}
