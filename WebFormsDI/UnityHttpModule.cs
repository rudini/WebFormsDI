// Type: Unity.WebForms.UnityHttpModule
// Assembly: Unity.WebForms, Version=1.2.4781.41479, Culture=neutral, PublicKeyToken=null
// MVID: 52D3D3D9-B5DC-41E0-B51C-48F35D9A7BCE
// Assembly location: c:\Users\rrudin\Documents\Visual Studio 2013\Projects\WebFormsDI\packages\Unity.WebForms.1.2.0.0\lib\net40\Unity.WebForms.dll

using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Web;
using System.Web.UI;

namespace Unity.WebForms
{
    using System.Linq;

    public class UnityHttpModule : IHttpModule
    {
        private IUnityContainer _parentContainer;
        private IUnityContainer _childContainer;

        private IUnityContainer ParentContainer
        {
            get
            {
                return this._parentContainer ?? (this._parentContainer = HttpExtensions.GetContainer(HttpContext.Current.Application));
            }
        }

        private IUnityContainer ChildContainer
        {
            get
            {
                return this._childContainer;
            }
            set
            {
                this._childContainer = value;
                HttpExtensions.SetChildContainer(HttpContext.Current, value);
            }
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.ContextOnBeginRequest);
            context.PreRequestHandlerExecute += new EventHandler(this.OnPreRequestHandlerExecute);
            context.EndRequest += new EventHandler(this.ContextOnEndRequest);
        }

        public void Dispose()
        {
        }

        private void ContextOnBeginRequest(object sender, EventArgs e)
        {
            this.ChildContainer = this.ParentContainer.CreateChildContainer();            
        }

        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (HttpContext.Current.Handler == null)
                return;
            IHttpHandler handler = HttpContext.Current.Handler;



            // find out the type of handler 
            var viewtype = handler.GetType().BaseType;
            // get the presentertype by a naming convention
            //var presenterType = Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.Name == viewtype.Name + "Presenter").Single();
            if (viewtype == null)
            {
                throw new Exception("Presenter not found");
            }

            var presenterType = viewtype.Assembly.GetTypes().Single(_ => _.Name == viewtype.Name + "Presenter");

            var presenter = this.ChildContainer.Resolve(
                presenterType,
                new ResolverOverride[] { new ParameterOverride("view", handler) });

            UnityContainerExtensions.BuildUp(
                this.ChildContainer,
                handler.GetType(),
                (object)handler,
                new ResolverOverride[] { new PropertyOverride("Presenter", presenter) });


            Page page = handler as Page;
            if (page == null)
                return;
            page.InitComplete += new EventHandler(this.OnPageInitComplete);
        }

        private void OnPageInitComplete(object sender, EventArgs e)
        {
            foreach (Control control in UnityHttpModule.GetControlTree((Control)sender))
            {
                if (!(control.GetType().FullName ?? string.Empty).StartsWith("System") || !(control.GetType().BaseType != (Type)null ? control.GetType().BaseType.FullName : string.Empty).StartsWith("System"))
                    UnityContainerExtensions.BuildUp(this.ChildContainer, control.GetType(), (object)control, new ResolverOverride[0]);
            }
        }

        private void ContextOnEndRequest(object sender, EventArgs e)
        {
            if (this.ChildContainer == null)
                return;
            this.ChildContainer.Dispose();
        }

        private static IEnumerable GetControlTree(Control root)
        {
            if (root.HasControls())
            {
                foreach (Control root1 in root.Controls)
                {
                    yield return (object)root1;
                    if (root1.HasControls())
                    {
                        foreach (Control control in UnityHttpModule.GetControlTree(root1))
                            yield return (object)control;
                    }
                }
            }
        }
    }
}
