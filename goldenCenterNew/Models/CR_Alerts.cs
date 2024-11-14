using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class CR_Alerts
    {
        [Key]
        public int PKAlertID { get; set; }
        public int FKDeviceID { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Available { get; set; }

        [ForeignKey("FKDeviceID")]
        public CT_Devices Device { get; set; }
    }
}
