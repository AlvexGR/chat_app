using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Entities.Models
{
    [Table("Users")]
    public class User : BaseModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
