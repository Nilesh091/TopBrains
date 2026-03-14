using System;
using System.ComponentModel.DataAnnotations;

namespace LoginAuth.Models
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Please enter your username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Please enter your password")]
        public string Password { get; set; }
        public int IsActive { get; set; }

    }
}
