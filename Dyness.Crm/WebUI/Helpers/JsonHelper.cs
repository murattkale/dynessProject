using Newtonsoft.Json;

namespace WebUI.Helpers
{
    public static class JsonHelper
    {
        public static string ObjectToJsonString(object data)
        {
            var jsonObject = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return jsonObject;
        }
    }
}