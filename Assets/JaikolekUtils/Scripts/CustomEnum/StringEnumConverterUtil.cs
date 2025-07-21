using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JaikolekUtils.CustomEnum
{
    public static class StringEnumConverterUtil
    {
        public static bool TryConvertToStringEnum(object obj, out StringEnum result)
        {
            result = default;

            if (obj is StringEnum se)
            {
                result = se;
                return true;
            }

            if (obj is string str)
            {
                result = new StringEnum(str);
                return true;
            }

            if (obj is JObject jObj)
            {
                var name = jObj["name"]?.ToString() ?? "None";
                var value = jObj["value"]?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    result = new StringEnum(name, value);
                    return true;
                }
            }

            if (obj is Dictionary<string, object> dict)
            {
                dict.TryGetValue("name", out var nameObj);
                dict.TryGetValue("value", out var valueObj);

                var name = nameObj?.ToString() ?? "None";
                var value = valueObj?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    result = new StringEnum(name, value);
                    return true;
                }
            }

            return false;
        }
    }
}
