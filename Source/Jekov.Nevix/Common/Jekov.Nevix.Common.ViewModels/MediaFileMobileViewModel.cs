namespace Jekov.Nevix.Common.ViewModels
{
    using System.Runtime.Serialization;

    [DataContract]
    internal class MediaFileMobileViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}