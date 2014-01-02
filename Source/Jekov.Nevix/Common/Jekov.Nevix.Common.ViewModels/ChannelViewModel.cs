namespace Jekov.Nevix.Common.ViewModels
{
    using System.Runtime.Serialization;

    public class ChannelViewModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}