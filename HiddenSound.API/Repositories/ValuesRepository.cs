using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Repositories
{
    public class ValuesRepository : IValuesRepository
    {
        public string[] GetValues()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
