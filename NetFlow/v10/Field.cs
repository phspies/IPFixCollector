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
        private UInt32 _pen;
        private UInt16 _type;
		private UInt16 _length;
		private List<Byte> _value;

        private Byte[] _bytes;

		public String Type
		{
			get
			{
                return Field.FieldTypes(this._type);
			}
		}

        public UInt16 GetTypes()
        {
            return this._type;
        }

		public UInt16 Length
		{
			get
			{
                return this._length;
			}
		}
        public Boolean EnterpriseType
        {
            get
            {
                return this._pentype;
            }
        }
        public UInt32 EnterpriseNumber
        {
            get
            {
                return this._pen;
            }
        }

        public List<Byte> Value
		{
			get
			{
                return this._value;
			}
			set
			{
                this._value = value;
			}
		}

        public Field(Byte[] bytes, bool _pen_type = false)
        {
            this._bytes = bytes;
            this._pentype = _pen_type;
            this.Parse();
        }

        private void Parse()
        {
            this._value = new List<byte>();
            byte[] reverse = this._bytes.Reverse().ToArray();

            if (this._bytes.Length == 4 || this._bytes.Length == 8)
            {
                if (_pentype)
                {
                    this._pen = BitConverter.ToUInt32(reverse, 0);
                    this._length = BitConverter.ToUInt16(reverse, 4);
                    Byte[] _info_element = new Byte[2];
                    Array.Copy(reverse, 6, _info_element, 0, 2);
                    BitArray _bit_array = new BitArray(_info_element);
                    _bit_array.Set(15, false);
                    var array = new int[1];
                    _bit_array.CopyTo(array, 0);
                    this._type = (ushort)array[0];
                }
                else
                {
                    this._type = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 0);
                    this._length = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 2);
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
