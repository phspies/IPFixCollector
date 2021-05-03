using System;
using System.Collections.Generic;

namespace IPFixCollector.Modules.Netflow.v9
{
    public class TemplatesV9
    {
        public SynchronizedCollection<Template> Templates { get; set; }
        public Int32 Count { get { return this.Templates.Count; } }
        public TemplatesV9()
        {
            this.Templates = new SynchronizedCollection<Template>();
        }
    }
}
