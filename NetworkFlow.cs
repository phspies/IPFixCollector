using System;
using System.ComponentModel.DataAnnotations;

namespace IPFixCollector.DataModel
{
    public class NetworkFlow
    {
        [Key, StringLength(50)]
        public string Id { get; set; }
        [StringLength(50)]
        public string Source_address { get; set; }
        [StringLength(50)]
        public string Target_address { get; set; }
        public int Source_port { get; set; }
        public int Target_port { get; set; }
        public int Protocol { get; set; }
        public DateTime Timestamp { get; set; }
        public long Start_timestamp { get; set; }
        public long Stop_timestamp { get; set; }
        public int Packets { get; set; }
        public int Kbyte { get; set; }

    }
}
