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
        private UInt16 _id;
        private UInt32 _domain_id;
        private UInt16 _count;
        private UInt16 _length;
        private List<Field> _field;

        private Byte[] _bytes;

        public UInt16 ID
        {
            get
            {
                return this._id;
            }
        }
        public UInt32 DomainID
        {
            get
            {
                return this._domain_id;
            }
        }
        public UInt16 Count
        {
            get
            {
                return this._count;
            }
        }
        public UInt16 Length
        {
            get
            {
                return this._length;
            }
        }

        public List<Field> Field
        {
            get
            {
                return this._field;
            }
        }

        public UInt16 FieldLength
        {
            get
            {
                UInt16 len = 0;
                foreach (Field fields in this._field)
                {
                    len += fields.Length;
                }
                return len;
            }
        }

        public Template(Byte[] bytes, int _start_pointer, UInt32 _domain_id)
        {
            this._bytes = bytes;
            this.Parse(_start_pointer, _domain_id);
        }

        private void Parse(int _start_pointer, UInt32 _domain_id)
        {
            byte[] reverse = this._bytes.Reverse().ToArray();
            this._field = new List<Field>();
            this._id = BitConverter.ToUInt16(reverse, this._bytes.Length - _start_pointer);
            this._count = BitConverter.ToUInt16(reverse, this._bytes.Length - _start_pointer - 2);
            this._domain_id = _domain_id;
            int _pointer = _start_pointer + 2;
            for (int i = 0; i < this._count; i++)
            {
                Byte[] _pen_byte = new Byte[1];
                Array.Copy(this._bytes, _pointer, _pen_byte, 0, 1);
                char[] _bits = Convert.ToString(_pen_byte[0], 2).PadLeft(8, '0').ToArray();
                if (new BitArray(_pen_byte).Get(7) == true)
                {
                    //we have a enterprise number which uses 8 bytes
                    Byte[] bfield = new Byte[8];
                    Array.Copy(this._bytes, _pointer, bfield, 0, 8);
                    Field field = new Field(bfield, true);
                    this._field.Add(field);
                    _pointer += 8;
                    this._length += 8;
                }
                else
                {
                    Byte[] bfield = new Byte[4];
                    Array.Copy(this._bytes, _pointer, bfield, 0, 4);
                    Field field = new Field(bfield, false);
                    this._field.Add(field);
                    _pointer += 4;
                    this._length += 4;
                }
            }
        }
    }
}
