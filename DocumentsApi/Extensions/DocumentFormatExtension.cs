using System.Text.Json;
using System.Text.Json.Serialization;
using DocumentCommon.Exceptions;
using MessagePack;
using Newtonsoft.Json;
using NotinoAssignment.OutputFormatters;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NotinoAssignment.Extensions;

public static class DocumentFormatExtension
{
    public static async Task<string> ToJson(this DocumentOutputModel model)
    {
        return await Task.Run(() 
            => JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }));
    }

    public static async Task<string> ToXml(this DocumentOutputModel model)
    {
        var json = await model.ToJson();
        
        var doc = await Task.Run(() 
            => JsonConvert.DeserializeXmlNode(json, "document"));
        
        if (doc is null)
        {
            throw new DocumentException("Error during XML serialization");
        }
        
        var xml = doc.OuterXml;
        return xml;
    }

    public static async Task<string> ToMessagePack(this DocumentOutputModel model)
    {
        var json = await model.ToJson();
        
        var bytes = await Task.Run(() 
            => MessagePackSerializer.ConvertFromJson(json));
        
        return await Task.Run(() 
            => Convert.ToBase64String(bytes));
    }
}