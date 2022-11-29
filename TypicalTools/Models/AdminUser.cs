using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TypicalTools.Models
{
    public class AdminUser
    {
        public int UserID { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "Field is required")]
        [MaxLength(15)]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Field is required")]
        [MaxLength(15)]
        public string Password { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}

