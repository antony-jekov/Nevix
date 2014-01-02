namespace Jekov.Nevix.Common.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class UserLoginViewModel
    {
        [Required]
        [DataMember(Name = "email")]
        [StringLength(40)]
        public string Email { get; set; }

        [Required]
        [DataMember(Name = "password")]
        [StringLength(40)]
        public string Password { get; set; }
    }
}