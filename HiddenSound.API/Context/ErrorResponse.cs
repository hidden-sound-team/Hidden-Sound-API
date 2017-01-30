using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Context
{
    public class ErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();
    }
}
