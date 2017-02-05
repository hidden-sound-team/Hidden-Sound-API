using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Shared.Models
{
    public enum TransactionStatus
    {
        Pending = 0,

        Authorized = 1,

        Declined = 2,

        Expired = 3
    }
}
