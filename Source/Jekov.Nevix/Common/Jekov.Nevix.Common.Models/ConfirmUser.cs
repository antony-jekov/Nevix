using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jekov.Nevix.Common.Models
{
    public class ConfirmUser
    {
        public ConfirmUser ()
        {
        }

        public ConfirmUser (NevixUser user, string secretKey)
        {
            this.User = user;
            this.SecretKey = secretKey;
        }
        public int Id { get; set; }

        public string SecretKey { get; set; }

        public int UserId { get; set; }

        public virtual NevixUser User { get; set; }
    }
}
