using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    [EnableCors("Application")]
    public class QRController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        public string Generate()
        {
            return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASIAAAEiAQMAAABncE31AAAABlBMVEX///8AAABVwtN+AAABAklEQVRoge3YSxKEIAxF0VS5AJbk1lmSC7CKhhgE++eEMOi+byAqZ5biK0IIIeR/kiy7yLrlzxjqn4jyVsdH2MvbkvSt70B5qlKSGKxChy8FQ01UrUKo2SqD6lGTlDat727+Qo1T54psQ+Rm3UaNU33Clz7UeGXV0LqUnWja8tKwlgdqkhLtuw4WlLdqe6CD6lksu2uFUA6qUpF2FNAKodyVTkWaoy5rer8bQo1XnbdjmNRzMMpZdctA2uoQKfsilL+K2gRbC2w39DIzoVxUd+tjn7ldnm+aUa6qjI5aJrkG5a1Sf+WJmqC0qRWyx+f5CzVQWfbzDuIsE8pZEUII+fU8AAzUwzIE4xMRAAAAAElFTkSuQmCC";
        }
    }
}
