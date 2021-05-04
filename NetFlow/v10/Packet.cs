using System;
using System.Collections.Generic;
using System.Linq;

namespace IPFixCollector.Modules.Netflow.v10
{
    public class V10Packet
    {
        private byte[] _bytes;

        public V10Header Header { get; private set; }
        public List<FlowSet> FlowSet { get; private set; }

        public V10Packet(byte[] bytes, TemplatesV10 templates)
        {
            _bytes = bytes;
            Parse(templates);
        }

        private void Parse(TemplatesV10 templates)
        {
            FlowSet = new List<FlowSet>();

            int length = _bytes.Length - 16;

            byte[] header = new byte[16];
            byte[] flowset = new byte[length];

            Array.Copy(_bytes, 0, header, 0, 16);
            Array.Copy(_bytes, 16, flowset, 0, length);

            Header = new V10Header(header);
            byte[] reverse = flowset.Reverse().ToArray();

            int templenght = 0;

            while ((templenght + 2) < flowset.Length)
            {
                ushort lengths = BitConverter.ToUInt16(reverse, flowset.Length - sizeof(short) - (templenght + 2));
                byte[] bflowsets = new byte[lengths];
                if (lengths <= flowset.Count())
                {
                    Array.Copy(flowset, templenght, bflowsets, 0, lengths);

                    FlowSet flowsets = new FlowSet(bflowsets, templates, Header.DomainID);
                    FlowSet.Add(flowsets);
                }
                templenght += lengths;
            }
        }

    }
}
