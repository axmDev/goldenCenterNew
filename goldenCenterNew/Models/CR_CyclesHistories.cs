using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class CR_CyclesHistories
    {
        [Key]
        public int PKCycleHistoryID { get; set; }
        public int FKDeviceID { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public string SerialNumber { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Available { get; set; }

        [ForeignKey("FKDeviceID")]
        public CT_Devices Device { get; set; }
    }
}
