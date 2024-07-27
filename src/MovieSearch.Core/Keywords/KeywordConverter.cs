using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieSearch.Core.Keywords;

/// <summary>
///     Expected parent json node is "keywords". The child node is variable
///     and should be set as a parameter to the JsonConverter attribute which
///     will use the KeywordConverter .ctor to create the converter with the
///     provided parameter.
/// </summary>
internal class KeywordConverter : JsonConverter
{
    private readonly string _key;

    public KeywordConverter(string key)
    {
        _key = key;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var obj = JToken.Load(reader);

        var arr = (JArray)obj[_key];

        var keywords = arr.ToObject<IReadOnlyList<Keyword>>();

        return keywords;
    }

    public override bool CanConvert(Type objectType)
    {
        return false;
    }
}
