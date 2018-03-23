using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LaesoeLineApi.Swagger
{
    public class HonorRequiredAttributeFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            var actionParameters = context.ApiDescription.ActionDescriptor.Parameters.ToDictionary(x => x.Name, x => x);

            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                if (parameter.In == "query" && actionParameters[parameter.Name] is ControllerParameterDescriptor descriptor)
                {
                    if (descriptor.ParameterInfo.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute)))
                    {
                        parameter.Required = true;
                    }
                }
            }
        }
    }
}
