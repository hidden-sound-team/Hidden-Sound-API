using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;

namespace HiddenSound.API.Swagger
{
    public class HideOperationFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var removal = new List<string>();

            foreach(var path in swaggerDoc.Paths)
            {
                var methods = new[] { path.Value.Get, path.Value.Post, path.Value.Delete, path.Value.Put };

                foreach(var method in methods)
                {
                    if(method == null)
                    {
                        continue;
                    }

                    if(!method.Tags.Any(t => t.Contains("OAuth") || t.Contains("API"))){
                        removal.Add(path.Key);
                    }
                }
            }

            foreach(var removeKey in removal)
            {
                if (swaggerDoc.Paths.ContainsKey(removeKey))
                {
                    swaggerDoc.Paths.Remove(removeKey);
                }
            }
        }
    }
}
