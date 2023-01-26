using DocumentCommon.Exceptions;
using DocumentServices.Contracts;
using Microsoft.AspNetCore.Mvc.Formatters;
using NotinoAssignment.Extensions;

namespace NotinoAssignment.OutputFormatters;

public sealed class DocumentOutputFormatter : OutputFormatter
{
    private readonly IDictionary<string, Func<DocumentOutputModel, Task<string>>> _serializers = 
        new Dictionary<string, Func<DocumentOutputModel, Task<string>>>
    {
        {"application/json", doc => doc.ToJson() },
        {"application/xml", doc => doc.ToXml() },
        {"application/x-msgpack", doc => doc.ToMessagePack() }
    };
    
    public DocumentOutputFormatter()
    {
        SupportedMediaTypes.Add("application/json");
        SupportedMediaTypes.Add("application/xml");
        SupportedMediaTypes.Add("application/x-msgpack");
    }
    
    protected override bool CanWriteType(Type? type) => typeof(DocumentModel).IsAssignableFrom(type);
    
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
        var acceptHeader = context.HttpContext.Request.Headers.Accept;
        if (context.Object is null || context.ObjectType != typeof(DocumentModel))
        {
            throw new DocumentException();
        }
        
        var document = (DocumentModel)context.Object;
        if (document is null)
        {
            throw new DocumentException();
        }

        var responseModel = new DocumentOutputModel
        {
            Id = document.Id!,
            Tags = document.Tags!,
            Data = document.Data!
        };
        
        string content;
        try
        {
            if (!string.IsNullOrEmpty(acceptHeader) && _serializers.TryGetValue(acceptHeader!, out var serializer))
            {
                content = await serializer(responseModel);
                context.HttpContext.Response.ContentType = acceptHeader;
            }
            else
            {
                content = await responseModel.ToJson();
                context.HttpContext.Response.ContentType = "application/json";
            }
        }
        catch (Exception ex)
        {
            throw new DocumentException($"An error occurred while serializing the document object to {acceptHeader}", ex);
        }

        await context.HttpContext.Response.WriteAsync(content);
    }
}