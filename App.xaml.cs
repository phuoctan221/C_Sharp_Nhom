using System.Net;
using System.Windows;
using System.Windows.Media;

namespace DoAnNhom
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;

            RenderOptions.ProcessRenderMode =
                System.Windows.Interop.RenderMode.SoftwareOnly;

            base.OnStartup(e);
        }
    }
}