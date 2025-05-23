using Newtonsoft.Json;

namespace Extensions.Converters
{
    public static class JsonConverter
    {
        public static string ConvertToJson(this object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return json;
        }

        public static T ConvertFromJson<T>(this string jsonString)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(jsonString);
                return obj;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        /// <summary>
        /// Конвертация Json-строки в объект
        /// </summary>
        /// <typeparam name="T">Тип целевого объекта конвертации</typeparam>
        /// <param name="jsonString">Строка данных</param>
        /// <returns></returns>
        public static object? ConvertFromJson(this string jsonString, Type type)
        {
            return JsonConvert.DeserializeObject(jsonString, type);
        }
    }
}
