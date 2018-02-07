using AzR.Utilities;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace AzR.Core.Web.MEF
{
    public class AzRControllerFactory : DefaultControllerFactory
    {
        private CompositionContainer _container;

        public AzRControllerFactory(CompositionContainer container)
        {
            _container = container;
        }
        //protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        //{
        //    var controllerType = base.GetControllerType(requestContext, controllerName);

        //    if (controllerType == null)
        //    {
        //        var controller = _container.GetExports<IController, IControllerMetaData>().SingleOrDefault(x => x.Metadata.ControllerName == controllerName).Value;

        //        if (controller != null)
        //        {
        //            return controller.GetType();
        //        }
        //    }
        //    return controllerType;
        //}

        protected override IController GetControllerInstance(RequestContext context, Type controllerType)
        {
            IController result = null;


            if (!AzRBootstrap.IsIntialized)
            {
                AzRBootstrap.Intialize();
                ControllerBuilder.Current.SetControllerFactory(new AzRControllerFactory(AzRBootstrap.Container));
            }
            if (controllerType == null)
            {
                return null;
            }
            //if (controllerType == null)
            //{
            //    return base.GetControllerInstance(context, controllerType);
            //    // return null;
            //}

            var export = _container.GetExports(typeof(IAzRController), null, controllerType.FullName).SingleOrDefault(x => x.Value.GetType() == controllerType);

            if (null != export)
            {
                result = export.Value as IController;
            }

            if (null != result)
            {
                _container.SatisfyImportsOnce(result);
            }

            return result;
        }
        public override void ReleaseController(IController controller)
        {
            ((IDisposable)controller).Dispose();
        }
    }

    //public class Custom1ControllerFactory : IControllerFactory
    //{
    //    private readonly DefaultControllerFactory _defaultControllerFactory;

    //    public Custom1ControllerFactory()
    //    {
    //        _defaultControllerFactory = new DefaultControllerFactory();
    //    }
    //    public IController CreateController(RequestContext requestContext, string controllerName)
    //    {
    //        var controller = Bootstrapper.GetInstance<IController>(controllerName);

    //        if (controller == null)
    //            throw new Exception("Controller not found!");

    //        return controller;
    //    }

    //    public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
    //    {
    //        return SessionStateBehavior.Default;
    //    }

    //    public void ReleaseController(IController controller)
    //    {
    //        var disposableController = controller as IDisposable;

    //        if (disposableController != null)
    //        {
    //            disposableController.Dispose();
    //        }
    //    }
    //}

    //public class CustomControllerFactory : DefaultControllerFactory
    //{
    //    protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
    //    {
    //        var controller = MefBootstrap.GetInstance<IController>(controllerType?.Name);

    //        //here if the controller object is not found (resulted as null) we can go ahead and get the default controller.
    //        //This means that in order to get the Controller we have to Export it first which will be shown later in this post
    //        return null == controller ? base.GetControllerInstance(requestContext, controllerType) : controller;
    //    }
    //    public override void ReleaseController(IController controller)
    //    {
    //        //this is were the controller gets disposed
    //        ((IDisposable)controller).Dispose();
    //    }

    //    //protected override Type GetControllerType(RequestContext requestContext, string controllerName)
    //    //{
    //    //    var type = base.GetControllerType(requestContext, controllerName);

    //    //    if (type != null /*&& IsIngored(type)*/)
    //    //    {
    //    //        return null;
    //    //    }

    //    //    return type;
    //    //}

    //    //public static bool IsIngored(Type type)
    //    //{
    //    //    return type.Assembly.GetCustomAttributes(typeof(IgnoreAssemblyAttribute), false).Any()
    //    //           || type.GetCustomAttributes(typeof(IgnoreControllerAttribute), false).Any();
    //    //}
    //}


}