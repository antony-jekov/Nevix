namespace Jekov.Nevix.Common.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class UserRegisterViewModel
    {
        [Required]
        [StringLength(40)]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [Required]
        [StringLength(40)]
        [DataMember(Name = "password")]
        public string Password { get; set; }

        [Required]
        [StringLength(40)]
        [DataMember(Name = "confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}