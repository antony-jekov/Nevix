namespace Jekov.Nevix.Common.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class UserRegisterViewModel
    {
        [Required]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [Required]
        [DataMember(Name = "password")]
        public string Password { get; set; }

        [Required]
        [DataMember(Name = "confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}