﻿using System;
using System.Linq;

namespace HiddenSound.API.Areas.Mobile.Models.Requests
{
    public class TransactionAuthorizeRequest
    {
        public string AuthorizationCode { get; set; }

        public string IMEI { get; set; }
    }
}