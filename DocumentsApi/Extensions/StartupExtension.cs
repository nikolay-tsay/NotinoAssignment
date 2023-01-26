using DocumentCommon.Enums;
using DocumentCommon.Options;
using DocumentServices;
using DocumentServices.Interfaces;
using DocumentStorage.Interfaces;
using DocumentStorage.Storages;
using NotinoAssignment.Middleware;
using NotinoAssignment.OutputFormatters;

namespace NotinoAssignment.Extensions;

public static class StartupExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
            options.OutputFormatters.Add(new DocumentOutputFormatter());
        });

        var storageOptions = new StorageOptions();
        builder.Configuration.Bind("StorageOption", storageOptions);
        builder.Services.AddSingleton(storageOptions);
        builder.AddStorage(storageOptions);
        
        builder.Services.AddScoped<IDocumentService, DocumentService>();

        builder.Services.AddSwaggerGen();
    }

    public static void SetupPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.MapControllers();
    }

    private static void AddStorage(this WebApplicationBuilder builder, StorageOptions options)
    {
        switch (options.Type)
        {
            case StorageTypes.InMemory:
                builder.Services.AddSingleton<IStorage, InMemoryStorage>();
                break;
            case StorageTypes.Hdd:
                builder.Services.AddSingleton<IStorage, HddStorage>();
                break;
            default:
                throw new Exception("No storage option specified");
        }
    }
}