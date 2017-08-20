// Type: Unity.WebForms.HttpExtensions
// Assembly: Unity.WebForms, Version=1.2.4781.41479, Culture=neutral, PublicKeyToken=null
// MVID: 52D3D3D9-B5DC-41E0-B51C-48F35D9A7BCE
// Assembly location: c:\Users\rrudin\Documents\Visual Studio 2013\Projects\WebFormsDI\packages\Unity.WebForms.1.2.0.0\lib\net40\Unity.WebForms.dll

using Microsoft.Practices.Unity;
using System.Web;

namespace Unity.WebForms
{
    public static class HttpExtensions
    {
        private static object _thisLock = new object();
        private const string GlobalContainerKey = "EntLibContainer";
        private const string RequestContainerKey = "EntLibChildContainer";

        static HttpExtensions()
        {
        }

        public static IUnityContainer GetContainer(this HttpApplicationState appState)
        {
            IUnityContainer unityContainer = appState["EntLibContainer"] as IUnityContainer;
            try
            {
                if (unityContainer == null)
                {
                    appState.Lock();
                    unityContainer = (IUnityContainer)new UnityContainer();
                    appState["EntLibContainer"] = (object)unityContainer;
                }
            }
            finally
            {
                appState.UnLock();
            }
            return unityContainer;
        }

        public static void SetContainer(this HttpApplicationState appState, IUnityContainer container)
        {
            appState.Lock();
            try
            {
                appState["EntLibContainer"] = (object)container;
            }
            finally
            {
                appState.UnLock();
            }
        }

        public static IUnityContainer GetChildContainer(this HttpContext context)
        {
            IUnityContainer container = context.Items[(object)"EntLibChildContainer"] as IUnityContainer;
            if (container == null)
            {
                lock (HttpExtensions._thisLock)
                {
                    container = HttpExtensions.GetContainer(context.Application).CreateChildContainer();
                    HttpExtensions.SetChildContainer(context, container);
                }
            }
            return container;
        }

        public static void SetChildContainer(this HttpContext context, IUnityContainer container)
        {
            lock (HttpExtensions._thisLock)
                context.Items[(object)"EntLibChildContainer"] = (object)container;
        }
    }
}
