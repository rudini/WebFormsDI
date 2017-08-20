// Type: Unity.WebForms.PreApplicationStart
// Assembly: Unity.WebForms, Version=1.2.4781.41479, Culture=neutral, PublicKeyToken=null
// MVID: 52D3D3D9-B5DC-41E0-B51C-48F35D9A7BCE
// Assembly location: c:\Users\rrudin\Documents\Visual Studio 2013\Projects\WebFormsDI\packages\Unity.WebForms.1.2.0.0\lib\net40\Unity.WebForms.dll

using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Unity.WebForms
{
    public class PreApplicationStart
    {
        private static bool _isStarting;

        public static void PreStart()
        {
            if (PreApplicationStart._isStarting)
                return;
            PreApplicationStart._isStarting = true;
            DynamicModuleUtility.RegisterModule(typeof(UnityHttpModule));
        }
    }
}
