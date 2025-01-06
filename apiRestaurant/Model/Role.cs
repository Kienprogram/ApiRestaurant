using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace apiRestaurant.Model
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Column("roleid")]
        public int RoleId { get; set; }
        [Column("rolename")]
        public string RoleName { get; set; } = string.Empty;
    }
}
