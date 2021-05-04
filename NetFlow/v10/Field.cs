using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IPFixCollector.Modules.Netflow.v10
{
    [Serializable]
    public class Field
    {
        private bool _pentype;
        private uint _pen;
        private ushort _type;
        private ushort _length;
        private List<byte> _value;

        private byte[] _bytes;

        public string Type
        {
            get
            {
                return FieldTypes(_type);
            }
        }

        public ushort GetTypes()
        {
            return _type;
        }

        public ushort Length
        {
            get
            {
                return _length;
            }
        }
        public bool EnterpriseType
        {
            get
            {
                return _pentype;
            }
        }
        public uint EnterpriseNumber
        {
            get
            {
                return _pen;
            }
        }

        public List<byte> Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public Field(byte[] bytes, bool _pen_type = false)
        {
            _bytes = bytes;
            _pentype = _pen_type;
            Parse();
        }

        private void Parse()
        {
            Field field = this;
            field._value = new List<byte>();
            byte[] reverse = field._bytes.Reverse().ToArray();

            if (field._bytes.Length == 4 || field._bytes.Length == 8)
            {
                if (_pentype)
                {
                    field._pen = BitConverter.ToUInt32(reverse, 0);
                    field._length = BitConverter.ToUInt16(reverse, 4);
                    Byte[] _info_element = new Byte[2];
                    Array.Copy(reverse, 6, _info_element, 0, 2);
                    BitArray _bit_array = new BitArray(_info_element);
                    _bit_array.Set(15, false);
                    var array = new int[1];
                    _bit_array.CopyTo(array, 0);
                    field._type = (ushort)array[0];
                }
                else
                {
                    field._type = BitConverter.ToUInt16(reverse, field._bytes.Length - sizeof(Int16) - 0);
                    field._length = BitConverter.ToUInt16(reverse, field._bytes.Length - sizeof(Int16) - 2);
                }
            }
        }
        public static String FieldTypes(UInt16 Types)
        {
            FieldType _type = (FieldType)Enum.Parse(typeof(FieldType), Types.ToString());
            return _type.ToString();
        }
    }
}
