using DocumentStorage.Interfaces;
using DocumentStorage.Models;

namespace DocumentStorage.Storages;

public class InMemoryStorage : IStorage
{
    private readonly Dictionary<string, DocumentStorageModel> _storage = new();

    public async Task<bool> SaveDocument(DocumentStorageModel document, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            try
            {
                _storage[document.Id] = document;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }, cancellationToken);
    }

    public async Task<DocumentStorageModel?> GetById(string id, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            _storage.TryGetValue(id, out var result);
            return result;
        }, cancellationToken);
    }
}