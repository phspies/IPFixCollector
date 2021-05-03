using System;
using System.Collections.Generic;

namespace IPFixCollector.Modules.Netflow.v10
{
    public class TemplatesV10
    {
        public SynchronizedCollection<Template> Templates { get; set; }
        public int Count { get { return Templates.Count; } }
        public TemplatesV10()
        {
            Templates = new SynchronizedCollection<Template>();
        }
    }
}
