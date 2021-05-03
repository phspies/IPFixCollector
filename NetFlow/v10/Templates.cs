using System;
using System.Collections.Generic;

namespace IPFixCollector.Modules.Netflow.v10
{
    public class TemplatesV10
    {
        public SynchronizedCollection<Template> Templates { get; set; }
        public Int32 Count { get { return this.Templates.Count; } }
        public TemplatesV10()
        {
            this.Templates = new SynchronizedCollection<Template>();
        }
    }
}
