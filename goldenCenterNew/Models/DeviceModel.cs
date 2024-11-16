using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Required]
        [Display(Name = "Device Type")]
        public int FKTypeID { get; set; }

        public string DeviceTypeName { get; set; }

        public int Cycles { get; set; } = 0;
        public int WeeklyCycles { get; set; } = 0;
    }
}
