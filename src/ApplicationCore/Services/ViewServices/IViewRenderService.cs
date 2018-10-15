using System.Threading.Tasks;

namespace ApplicationCore.Services.ViewServices
{
    public interface IViewRenderService
    {
         Task<string> RenderToStringAsync(string viewName, object model = null);
    }
}
