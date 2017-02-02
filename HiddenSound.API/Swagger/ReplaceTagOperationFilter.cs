using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HiddenSound.API.Swagger
{
    public class ReplaceTagOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {

            var attribute = context.ApiDescription
                .ControllerAttributes()
                .OfType<AreaAttribute>().FirstOrDefault();

            if (attribute != null)
            {
                operation.Tags[0] = attribute.RouteValue;
            }

            operation.Consumes.Clear();
            operation.Consumes.Add("application/json");

            operation.Produces.Clear();
            operation.Produces.Add("application/json");

            foreach (var response in operation.Responses)
            {
                if (response.Key == "401")
                {
                    response.Value.Schema = null;
                }
            }
        }
    }
}
