using System;
using System.Web;

namespace WebFormsDI
{
    using Microsoft.Practices.Unity;

    using Unity.WebForms;

    using WebFormsDI.Presenter;

    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Create a Unity container and load the Enterprise Library extension.
            IUnityContainer myContainer = Application.GetContainer();
            //myContainer.AddExtension(new EnterpriseLibraryCoreExtension());

            // Perform any container initialization you require. For example, 
            // register any custom types you require in your application
            // for example, register services and other types to use throughout
            // the code, including any to be managed as singleton instances.
            myContainer.RegisterType<_Default>();
            myContainer.RegisterType<_DefaultPresenter>();
        }
    }
}