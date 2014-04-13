using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jekov.Nevix.Web.ServerApi.ViewModels
{
    public class ResetPassViewModel
    {
        public string SecretKey { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(40, ErrorMessage = "Must be between 5 and 40 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(40, ErrorMessage = "Must be between 5 and 40 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}