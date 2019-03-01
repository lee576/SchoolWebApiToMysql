using Newtonsoft.Json;
using SqlSugar;

namespace Infrastructure.Service.Base
{
    public class SqlSugarSerializeService : ISerializeService
    {
        public string SerializeObject(object value)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.SerializeObject(value, settings);
        }

        public T DeserializeObject<T>(string value)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
