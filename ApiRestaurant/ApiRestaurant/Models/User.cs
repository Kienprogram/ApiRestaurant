using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_restaurant.Models
{
    [Table("tb_user")]
    public class User
    {
        [Key]
        [Column("userid")]
        public int UserId { get; set; }
        
        [Column("username")]
        public string? UserName { get; set; }
        
        [Column("password")]
        public string? Password { get; set; }
        
        [Column("employeeid")]
        public int EmployeeId { get; set; }
    }

    public class Login
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
