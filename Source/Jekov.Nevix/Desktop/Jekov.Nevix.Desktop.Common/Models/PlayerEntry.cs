using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jekov.Nevix.Desktop.Common.Models
{
    public class PlayerEntry
    {
        public string Name { get; protected set; }

        public string Location { get; protected set; }

        public PlayerEntry (string name, string location)
        {
            this.Name = name;
            this.Location = location;
        }
    }
}
