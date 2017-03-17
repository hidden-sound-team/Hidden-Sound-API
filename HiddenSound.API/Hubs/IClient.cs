using System;
using System.Linq;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Hubs
{
    public interface IClient
    {
        void UpdateAuthorization(AuthorizationStatus status);
    }
}