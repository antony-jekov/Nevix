namespace Jekov.Nevix.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class NevixUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ModelConstants.EmailLength, MinimumLength=ModelConstants.EmailMinLength)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(ModelConstants.PasswordLength)]
        public string Password { get; set; }

        [StringLength(36)]
        public string SessionKey { get; set; }

        public DateTime? LastFilesUpdate { get; set; }

        public string Media { get; set; }

        public DateTime? LastSurveyTaken { get; set; }
    }
}