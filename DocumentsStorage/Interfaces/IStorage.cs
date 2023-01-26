using DocumentStorage.Models;

namespace DocumentStorage.Interfaces;

public interface IStorage
{
    Task<bool> SaveDocument(DocumentStorageModel document, CancellationToken cancellationToken);
    Task<DocumentStorageModel?> GetById(string id, CancellationToken cancellationToken);
}