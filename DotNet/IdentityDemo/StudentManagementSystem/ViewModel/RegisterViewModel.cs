using System;

namespace StudentManagementSystem.ViewModel
{
    public class RegisterViewModel
    {
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }
}
