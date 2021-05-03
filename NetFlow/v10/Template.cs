using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace IPFixCollector.Modules.Netflow.v10
{
    [Serializable]
    public class Template
    {
        private ushort _id;
        private uint _domain_id;
        private ushort _count;
        private ushort _length;
        private List<Field> _field;

        private byte[] _bytes;

        public ushort ID
        {
            get
            {
                return _id;
            }
        }
        public uint DomainID
        {
            get
            {
                return _domain_id;
            }
        }
        public ushort Count
        {
            get
            {
                return _count;
            }
        }
        public ushort Length
        {
            get
            {
                return _length;
            }
        }

        public List<Field> Field
        {
            get
            {
                return _field;
            }
        }

        public ushort FieldLength
        {
            get
            {
                ushort len = 0;
                foreach (Field fields in _field)
                {
                    len += fields.Length;
                }
                return len;
            }
        }

        public Template(byte[] bytes, int _start_pointer, uint _domain_id)
        {
            _bytes = bytes;
            Parse(_start_pointer, _domain_id);
        }

        private void Parse(int _start_pointer, uint _domain_id)
        {
            byte[] reverse = _bytes.Reverse().ToArray();
            _field = new List<Field>();
            _id = BitConverter.ToUInt16(reverse, _bytes.Length - _start_pointer);
            _count = BitConverter.ToUInt16(reverse, _bytes.Length - _start_pointer - 2);
            Template template = this;
            template._domain_id = _domain_id;
            int _pointer = _start_pointer + 2;
            for (int i = 0; i < _count; i++)
            {
                byte[] _pen_byte = new byte[1];
                Array.Copy(_bytes, _pointer, _pen_byte, 0, 1);
                _ = Convert.ToString(_pen_byte[0], 2).PadLeft(8, '0').ToArray();
                if (new BitArray(_pen_byte).Get(7) == true)
                {
                    //we have a enterprise number which uses 8 bytes
                    byte[] bfield = new byte[8];
                    Array.Copy(_bytes, _pointer, bfield, 0, 8);
                    Field field = new Field(bfield, true);
                    _field.Add(field);
                    _pointer += 8;
                    _length += 8;
                }
                else
                {
                    byte[] bfield = new byte[4];
                    Array.Copy(_bytes, _pointer, bfield, 0, 4);
                    Field field = new Field(bfield, false);
                    _field.Add(field);
                    _pointer += 4;
                    _length += 4;
                }
            }
        }
    }
}
