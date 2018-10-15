using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Añade un mensaje para mostrarlo con alerts de Bootstrap.
        /// </summary>
        public static void AddMessage(this Controller controller, string tag, string message)
        {
            var tempDataKey = "Messages";
            var newMessage = new KeyValuePair<string, string>(tag, message);
            var data = controller.TempData.Get<List<KeyValuePair<string, string>>>(tempDataKey);

            if (data != null)
            {
                data.Add(newMessage);
                controller.TempData.Put(tempDataKey, data);
            }
            else
            {
                controller.TempData.Put(tempDataKey, new List<KeyValuePair<string, string>> { newMessage });
            }
        }
    }
}
