using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TypicalTools.Models
{
    public class AdminUser
    {
        public int UserID { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "Field is required")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Field is required")]
        public string Password { get; set; }
    }
}
