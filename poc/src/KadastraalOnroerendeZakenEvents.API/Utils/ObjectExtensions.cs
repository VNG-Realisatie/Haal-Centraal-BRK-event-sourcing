using Newtonsoft.Json;

namespace KadastraalOnroerendeZakenEvents.API.Utils
{
    public static class ObjectExtensions
    {
        public static string SerializeToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
