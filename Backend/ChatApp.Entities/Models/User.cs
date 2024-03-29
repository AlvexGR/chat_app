﻿using ChatApp.Entities.Common;
using ChatApp.Entities.Enums;

namespace ChatApp.Entities.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public bool IsConfirmed { get; set; }

        public string ConfirmationToken { get; set; }

        public string GooglePassword { get; set; }
    }
}
