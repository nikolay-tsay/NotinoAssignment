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
            using (StreamWriter file = File.CreateText($"{_options.HddStoragePath!}\\{document.Id}.json"))
            {
                var serializer = new JsonSerializer();
                await Task.Run(() => serializer.Serialize(file, document));
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<DocumentStorageModel?> GetById(string id, CancellationToken cancellationToken)
    {
        var content = await Task.Run(() => File.ReadAllText($"{_options.HddStoragePath!}\\{id}.json"));
        
        var document = JsonConvert.DeserializeObject<DocumentStorageModel>(content);
        return document;
    }
}