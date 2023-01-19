using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Avalonia.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MnemoschemeEditor.jsons;

public class IgnorePropertiesResolver : DefaultContractResolver
{
    private readonly HashSet<string> ignoreProps;
    public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
    {
        this.ignoreProps = new HashSet<string>(propNamesToIgnore);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (this.ignoreProps.Contains(property.PropertyName))
        {
            property.ShouldSerialize = _ => false;
        }

        /*if (property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
        {
            property.ShouldSerialize = _ => false;
        }*/
        if (property.PropertyName == "Children")
        {
            property.Writable = true;
        }
        if (!property.Writable)
        {
            property.ShouldSerialize = _ => false;
        }
        return property;
    }
}

public class DoubleConverter : JsonConverter<double>
{
    public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
    {
        if (double.IsNaN(value))
        {
            writer.WriteValue("NaN");
        }
        else if (double.IsNegativeInfinity(value))
        {
            writer.WriteValue("NInf");
        }
        else if (double.IsPositiveInfinity(value))
        {
            writer.WriteValue("PInf");
        }
        else
        {
            writer.WriteValue(value);
        }
    }

    public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String && (string)reader.Value == "NaN")
        {
            return double.NaN;
        }
        else if (reader.TokenType == JsonToken.String && (string)reader.Value == "NInf")
        {
            return double.NegativeInfinity;
        }
        else if (reader.TokenType == JsonToken.String && (string)reader.Value == "PInf")
        {
            return double.PositiveInfinity;
        }

        return (double)reader.Value;
    }
}

public class ControlsConverter : JsonConverter<Controls>{
    public override void WriteJson(JsonWriter writer, Controls? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }

    public override Controls? ReadJson(JsonReader reader, Type objectType, Controls? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        
            
            JObject obj = JObject.Load(reader);
            JArray arr = JArray.Parse(obj["$values"].ToString());
            Controls controls = new Controls();
            foreach (var jToken in arr)
            {
                using (JsonTextReader textReader = new JsonTextReader(new StringReader(jToken.ToString())))
                {
                    var control = serializer.Deserialize(textReader, Type.GetType(jToken["$type"].ToString()));
                    controls.Add((IControl)control);
                }

            }

            return controls;
    }
} 
