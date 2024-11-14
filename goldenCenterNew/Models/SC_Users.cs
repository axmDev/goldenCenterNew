using System.ComponentModel.DataAnnotations;

namespace goldenCenterNew.Models
{
    public class SC_Users
    {
        [Key]
        public int PKUserID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NTAccount { get; set; }
        public string Email { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Available { get; set; }
    }
}
