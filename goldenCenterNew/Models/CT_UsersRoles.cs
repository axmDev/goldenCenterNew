using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class CT_UsersRoles
    {
        [Key]
        public int PKUserRoleID { get; set; }
        public int FKUserID { get; set; }
        public int FKRoleID { get; set; }
        public string Comments { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Available { get; set; }

        [ForeignKey("FKUserID")]
        public SC_Users User { get; set; }

        [ForeignKey("FKRoleID")]
        public CT_Roles Role { get; set; }
    }
}
