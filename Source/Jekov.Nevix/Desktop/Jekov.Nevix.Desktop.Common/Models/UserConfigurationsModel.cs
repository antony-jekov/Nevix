namespace Jekov.Nevix.Desktop.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserConfigurationsModel
    {
        public string SessionKey { get; set; }

        public string BsPlayerLocation { get; set; }

        public DateTime LastMediaUpdate { get; set; }
    }
}
