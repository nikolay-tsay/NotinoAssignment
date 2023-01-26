using System.Collections.Concurrent;
using DocumentStorage.Interfaces;
using DocumentStorage.Models;

namespace DocumentStorage.Storages;

public sealed class InMemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<string, DocumentStorageModel> _storage = new();

    public async Task<bool> SaveDocument(DocumentStorageModel document, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            _storage.AddOrUpdate(document.Id, document, (_, _) => document);
            return true;
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