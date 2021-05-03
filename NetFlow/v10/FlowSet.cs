using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace IPFixCollector.Modules.Netflow.v10
{
    public class FlowSet
    {
        private UInt16 _id;
        private UInt16 _length;
        private SynchronizedCollection<Template> _template;
        private List<Byte> _valuebyte;

        private Byte[] _bytes;

        public UInt16 ID
        {
            get
            {
                return this._id;
            }
        }
        public UInt16 Length
        {
            get
            {
                return this._length;
            }
        }
        public SynchronizedCollection<Template> Template
        {
            get
            {
                return this._template;
            }
        }

        public List<Byte> ValueByte
        {
            get
            {
                return this._valuebyte;
            }
        }

        public FlowSet(Byte[] bytes, TemplatesV10 templates, UInt32 _domain_id)
        {
            this._bytes = bytes;
            this.Parse(templates, _domain_id);
        }

        private void Parse(TemplatesV10 templates, UInt32 _domain_id)
        {
            byte[] reverse = this._bytes.Reverse().ToArray();
            this._template = new SynchronizedCollection<Template>();
            this._valuebyte = new List<byte>();
            this._id = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 0);
            this._length = BitConverter.ToUInt16(reverse, this._bytes.Length - sizeof(Int16) - 2);
            if ((this._id == 2))
            {
                int address = 6;
                while (address < this._bytes.Length)
                {
                    Template template = new Template(this._bytes, address, _domain_id);

                    this._template.Add(template);
                    Boolean flag = false;
                    SynchronizedCollection<Template> templs = templates.Templates;
                    for (int i = 0; i < templs.Count; i++)
                    {
                        if (template.ID == templs[i].ID)
                        {
                            flag = true;
                            templs[i] = template;
                        }
                    }

                    if (flag)
                    {
                        templates.Templates = templs;
                    }
                    else
                    {
                        templates.Templates.Add(template);
                    }

                    address += template.Length + 4;
                }
            }
            else if (this._id > 255)
            {
                Template templs = null;

                Template _template = templates.Templates.FirstOrDefault(x => x.ID == this._id && x.DomainID == _domain_id);
                if (_template != null)
                {
                    int j = 4, z;
                    templs = DeepClone(_template) as Template;
                    z = (this._length - 4) / templs.FieldLength;

                    for (int y = 0; y < z; y++)
                    {
                        foreach (Field fields in templs.Field)
                        {
                            for (int i = 0; i < fields.Length; i++, j++)
                            {
                                fields.Value.Add(this._bytes[j]);
                            }
                        }

                        this._template.Add(DeepClone(templs) as Template);

                        foreach (Field filds in templs.Field)
                        {
                            filds.Value.Clear();
                        }
                    }
                }
                else
                {
                    for (int i = 4; i < this._bytes.Length; i++)
                    {
                        this._valuebyte.Add(this._bytes[i]);
                    }
                }

                foreach (Template templ in templates.Templates)
                {
                    if (templ.ID == this._id)
                    {
                        templs = DeepClone(templ) as Template;
                    }
                }
            }
        }
        public static object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }
    }
}
