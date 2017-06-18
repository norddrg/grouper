using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace DRG.WebAPI
{
    public class MultipleOperationsWithSameVerbFilter : IOperationFilter
    {
        public void Apply(
            Operation operation,
            SchemaRegistry schemaRegistry,
            ApiDescription apiDescription)
        {
            if (operation.parameters != null)
            {
                operation.operationId += "By";
                foreach (var parm in operation.parameters)
                {
                    operation.operationId += string.Format("{0}", parm.name);
                }
            }
        }
    }
}