using ChatApp.Entities.Common;

namespace ChatApp.Entities.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }
    }
}
