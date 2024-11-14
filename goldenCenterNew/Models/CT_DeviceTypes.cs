using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class CT_DeviceTypes
    {
        [Key]
        public int PKDeviceTypeID { get; set; }
        public int Type { get; set; }
        public int CyclesLimit { get; set; }
        public int WeeklyCyclesLimit { get; set; }
        public float AlertThreshold { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Available { get; set; }
    }
}
