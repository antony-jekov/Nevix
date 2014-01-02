namespace Jekov.Nevix.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class MediaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ModelConstants.NameLength)]
        public string Name { get; set; }

        public int ParentFolderId { get; set; }

        public virtual MediaFolder ParentFolder { get; set; }
    }
}