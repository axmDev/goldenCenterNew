using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class CT_Roles
    {
        [Key]
        public int PKRoleID { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Available { get; set; }
    }
}
