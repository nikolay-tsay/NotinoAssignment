using System.Text;
using DocumentCommon.Options;
using DocumentStorage.Interfaces;
using DocumentStorage.Models;
using Newtonsoft.Json;

namespace DocumentStorage.Storages;

public sealed class HddStorage : IStorage
{
    private readonly StorageOptions _options;

    public HddStorage(StorageOptions options)
    {
        _options = options;
        Directory.CreateDirectory(_options.HddStoragePath!);
    }

    public async Task<bool> SaveDocument(DocumentStorageModel document, CancellationToken cancellationToken)
    {
        try
        {
            byte[] arr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(document));
            string filePath = $"{_options.HddStoragePath!}\\{document.Id}.json";

            await File.WriteAllBytesAsync(filePath, arr, cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<DocumentStorageModel?> GetById(string id, CancellationToken cancellationToken)
    {
        string filePath = $"{_options.HddStoragePath!}\\{id}.json";
        if (!File.Exists(filePath))
        {
            return null;
        }
        
        var arr = await File.ReadAllBytesAsync(filePath, cancellationToken);
        return JsonConvert.DeserializeObject<DocumentStorageModel>(Encoding.UTF8.GetString(arr));
    }
}