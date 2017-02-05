using System;
using System.Linq;

namespace HiddenSound.API.Areas.API.Services
{
    public interface IQRService
    {
        string Create(string contents);
    }
}
