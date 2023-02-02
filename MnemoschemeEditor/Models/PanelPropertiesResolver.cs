using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using AvAp2.Models.BaseClasses;
using DynamicData;
using MnemoschemeEditor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MnemoschemeEditor.jsons;

public class PanelPropertiesResolver : DefaultContractResolver
{

    private IPointsAccessor _points;
    private readonly HashSet<string> ignoreProps;
    public PanelPropertiesResolver(IEnumerable<string> propNamesToIgnore, IPointsAccessor pointsAccessor)
    {
        this.ignoreProps = new HashSet<string>(propNamesToIgnore);
        _points = pointsAccessor;
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (this.ignoreProps.Contains(property.PropertyName) || ignoreProps.Any(x=>property.PropertyName.Contains(x)))
        {
            property.ShouldSerialize = _ => false;
        }
        if (property.PropertyName == "Children")
        {
            property.Writable = true;
            property.ValueProvider = new CollectionProvider(member as PropertyInfo);
        }
        
        if (!property.Writable)
        {
            property.ShouldSerialize = _ => false;
        }
        return property;
    }

    protected override JsonContract CreateContract(Type objectType)
    {
        
        var contract =  base.CreateContract(objectType);
        if (objectType == typeof(Panel))
        {
            contract.OnSerializedCallbacks.Add((o, context) =>
            {
                var newPoints = _points.GetPoints();
                newPoints.Add(new Point(Canvas.GetLeft((Panel)o), Canvas.GetTop((Panel)o)));
                _points.SavePoints(newPoints);
            });
            contract.OnDeserializedCallbacks.Add((o, context) =>
            {
                var newPoints = _points.GetPoints();
                var first = newPoints.First();
                Canvas.SetTop((AvaloniaObject)o, first.Y);
                Canvas.SetLeft((AvaloniaObject)o, first.X);
                newPoints.Remove(first);
                _points.SavePoints(newPoints);
            });
        }

        return contract;
    }
}

public class CollectionProvider:IValueProvider
{
    
    private PropertyInfo _targetProperty;
    public CollectionProvider(PropertyInfo targetProperty)
    {
        _targetProperty = targetProperty;
    }
    public void SetValue(object target, object? value)
    {
        var targetProp = _targetProperty.GetValue(target);
        if (targetProp is Controls listTarget && value is Controls listValue)
        {
            listTarget.Clear();
            listTarget.AddRange(listValue);
        }
    }
    public object? GetValue(object target)
    {
        object value = _targetProperty.GetValue(target);
        return value;
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

public class PanelsConverter : JsonConverter<Panel>
{
    public override void WriteJson(JsonWriter writer, Panel? value, JsonSerializer serializer)
    {
        JObject obj = JObject.FromObject(value);
        obj.Add("Top", Canvas.GetTop(value));
        obj.Add("Left", Canvas.GetLeft(value));
        foreach (var props in obj)
        {
            serializer.Serialize(writer, props.Value, typeof(Panel));
        }
    }

    public override Panel? ReadJson(JsonReader reader, Type objectType, Panel? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        double top = Convert.ToDouble(obj["Top"].ToString());
        double left = Convert.ToDouble(obj["Left"].ToString());
        obj.Remove("Top");
        obj.Remove("Left");
        return serializer.Deserialize<Panel>(new JsonTextReader(new StringReader(obj.ToString())));
    }
}
