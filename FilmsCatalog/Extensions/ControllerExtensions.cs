using Microsoft.AspNetCore.Mvc;

namespace FilmsCatalog.Extensions
{
    public static class ControllerExtensions
    {
        public static string Action<T>(this Controller controller, string actionName)
        where T : Controller
        {
            var name = typeof(T).Name;
            string controllerName = name.EndsWith("Controller")
                ? name.Substring(0, name.LastIndexOf("Controller")) : name;
            return controller.Url.Action(actionName, controllerName);
        }
    }
}
