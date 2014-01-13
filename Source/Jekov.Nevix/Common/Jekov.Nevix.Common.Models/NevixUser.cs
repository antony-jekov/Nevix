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
        
        [Required]
        [StringLength(ModelConstants.PasswordLength)]
        public string Password { get; set; }

        [StringLength(40)]
        public string SessionKey { get; set; }

        [StringLength(ModelConstants.ChannelNameLength)]
        public string ChannelName { get; set; }

        public DateTime? LastFilesUpdate { get; set; }

        public virtual ICollection<MediaFolder> Folders { get; set; }
    }
}