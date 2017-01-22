using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Repositories
{
    public interface IValuesRepository
    {
        string[] GetValues();
    }
}
