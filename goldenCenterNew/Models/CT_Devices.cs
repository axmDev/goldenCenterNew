using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class CT_Devices
    {
        [Key]
        public int PKDeviceID { get; set; }
        public int FKTypeID { get; set; }
        public int Cycles { get; set; }
        public string SerialNumber { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Available { get; set; }
        public int Failures { get; set; }

        [ForeignKey("FKTypeID")]
        public CT_DeviceTypes DeviceType { get; set; }
    }
}
