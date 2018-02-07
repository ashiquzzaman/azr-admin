using AzR.Utilities;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;

namespace AzR.Core.Web.MEF
{
    public class InterfaceModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType.IsInterface)
            {
                ModelBindingContext context = new ModelBindingContext(bindingContext);
                Lazy<object, object> export = AzRBootstrap.Container.GetExports(bindingContext.ModelType, null, bindingContext.ModelType.FullName).SingleOrDefault();
                object item = null;
                if (null != export)
                {
                    item = export.Value;
                }

                if (null != item)
                {
                    AzRBootstrap.Container.SatisfyImportsOnce(item);
                }

                Func<object> modelAccessor = () => item;
                if (item != null)
                    context.ModelMetadata = new ModelMetadata(new DataAnnotationsModelMetadataProvider(),
                        bindingContext.ModelMetadata.ContainerType, modelAccessor, item.GetType(), bindingContext.ModelName);

                return base.BindModel(controllerContext, context);
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}