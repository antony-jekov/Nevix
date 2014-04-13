using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jekov.Nevix.Common.Models
{
    public class ForgottenPasswordRequest
    {
        public ForgottenPasswordRequest()
        {
        }

        public ForgottenPasswordRequest(NevixUser requestUser, string secretKey)
        {
            this.User = requestUser;
            this.SecretKey = secretKey;
        }

        public int Id { get; set; }

        public string SecretKey { get; set; }

        public int UserId { get; set; }

        public virtual NevixUser User { get; set; }
    }
}
