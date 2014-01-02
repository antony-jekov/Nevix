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
        [StringLength(ModelConstants.EmailLength)]
        public string Email { get; set; }

        [StringLength(ModelConstants.FirstNameLength)]
        public string FirstName { get; set; }

        [StringLength(ModelConstants.LastNameLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(ModelConstants.PasswordLength)]
        public string Password { get; set; }

        public string SessionKey { get; set; }

        [StringLength(ModelConstants.ChannelNameLength)]
        public string ChannelName { get; set; }

        public DateTime? LastFilesUpdate { get; set; }

        public virtual ICollection<MediaFolder> Folders { get; set; }
    }
}