using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_restaurant.Models
{
    [Table("tb_employee")]
    public class Employee
    {
        [Key]
        [Column("employeeid")]
        public int EmployeeId { get; set; }

        [Column("employeename")]
        public string? EmployeeName { get; set; }

        [Column("employeesurname")]
        public string? EmployeeSurname { get; set; }

        [Column("phonenumber")]
        public string? PhoneNumber { get; set; }

        [Column("datestartwork")]
        public DateTime DateStartWork { get; set; }

        [Column("level")]
        public string? Level { get; set; }

        [Column("rule")]
        public string? Rule { get; set; } // Nullable if not required
    }
}
