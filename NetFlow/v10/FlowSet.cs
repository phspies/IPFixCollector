using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace IPFixCollector.Modules.Netflow.v10
{
    public class FlowSet
    {
        private byte[] _bytes;
        public ushort ID { get; private set; }
        public ushort Length { get; private set; }
        public SynchronizedCollection<Template> Template { get; private set; }
        public List<byte> ValueByte { get; private set; }

        public FlowSet(byte[] bytes, TemplatesV10 templates, uint _domain_id)
        {
            _bytes = bytes;
            Parse(templates, _domain_id);
        }

        private void Parse(TemplatesV10 templates, uint _domain_id)
        {
            byte[] reverse = _bytes.Reverse().ToArray();
            Template = new SynchronizedCollection<Template>();
            ValueByte = new List<byte>();
            ID = BitConverter.ToUInt16(reverse, _bytes.Length - sizeof(short) - 0);
            Length = BitConverter.ToUInt16(reverse, _bytes.Length - sizeof(short) - 2);
            if (ID == 2)
            {
                int address = 6;
                while (address < _bytes.Length)
                {
                    Template template = new Template(_bytes, address, _domain_id);
                    Template.Add(template);
                    bool flag = false;
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
            else if (ID > 255)
            {
                Template templs = null;

                Template _template = templates.Templates.FirstOrDefault(x => x.ID == ID && x.DomainID == _domain_id);
                if (_template != null)
                {
                    int j = 4, z;
                    templs = DeepClone(_template) as Template;
                    z = (this.Length - 4) / templs.FieldLength;

                    for (int y = 0; y < z; y++)
                    {
                        foreach (Field fields in templs.Field)
                        {
                            for (int i = 0; i < fields.Length; i++, j++)
                            {
                                fields.Value.Add(this._bytes[j]);
                            }
                        }

                        this.Template.Add(DeepClone(templs) as Template);

                        foreach (Field filds in templs.Field)
                        {
                            filds.Value.Clear();
                        }
                    }
                }
                else
                {
                    for (int i = 4; i < _bytes.Length; i++)
                    {
                        ValueByte.Add(_bytes[i]);
                    }
                }

                foreach (Template templ in templates.Templates)
                {
                    if (templ.ID == ID)
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
