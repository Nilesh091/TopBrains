using System;
using System.ComponentModel.DataAnnotations;

namespace Enterprise_Two_Factor_Authentication.ViewModels
{
    public class VerifyAuthenticatorViewModel
    {
        [Required]
        public string Code { get; set; }

        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
