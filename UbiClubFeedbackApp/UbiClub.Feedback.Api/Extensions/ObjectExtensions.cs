using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UbiClub.Feedback.Api.Extensions
{
    public static class ObjectExtensions
    {
        public static string Serialize(this object src, bool useCamelCasing = false)
        {
            return SerializeToString(src, NullValueHandling.Ignore, useCamelCasing);
        }

        public static string Dump(this object src, bool useCamelCasing = false)
        {
            return SerializeToString(src, NullValueHandling.Include, useCamelCasing);
        }

        static string SerializeToString(object src, NullValueHandling nullValueHandling, bool useCamelCasing)
        {
            if (!useCamelCasing)
            {
                return JsonConvert.SerializeObject(src, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = nullValueHandling
                });
            }

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(src, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = nullValueHandling,
                ContractResolver = contractResolver,
            });
        }
    }
}