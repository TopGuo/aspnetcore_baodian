using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TB.AspNetCore.Infrastructrue.Extensions
{
    public class MvcControllerActivator : IControllerActivator
    {
        private readonly ITypeActivatorCache _typeActivatorCache;

        public MvcControllerActivator(ITypeActivatorCache typeActivatorCache)
        {
            _typeActivatorCache = typeActivatorCache ?? throw new ArgumentNullException(nameof(typeActivatorCache));
        }

        public virtual object Create(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException(nameof(controllerContext));
            }
            if (controllerContext.ActionDescriptor == null)
            {
                throw new Exception();
            }
            var controllerTypeInfo = controllerContext.ActionDescriptor.ControllerTypeInfo;
            if (controllerTypeInfo is null)
            {
                throw new Exception();
            }
            var requestServices = controllerContext.HttpContext.RequestServices;
            var obj = _typeActivatorCache.CreateInstance<object>(requestServices, controllerTypeInfo.AsType());
            foreach (var declaredProperty in controllerTypeInfo.DeclaredProperties)
            {
                declaredProperty.GetSetMethod(true).Invoke(obj, new object[1]
                {
                    ActivatorUtilities.GetServiceOrCreateInstance(requestServices, declaredProperty.PropertyType)
                });
            }
            return obj;
        }

        public virtual void Release(ControllerContext context, object controller)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            var disposable = controller as IDisposable;
            disposable?.Dispose();
        }
    }
}
