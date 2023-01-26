using DocumentCommon.Exceptions;
using DocumentServices.Contracts;
using DocumentServices.Interfaces;
using DocumentStorage.Interfaces;
using DocumentStorage.Models;

namespace DocumentServices;

public sealed class DocumentService : IDocumentService
{
    private readonly IStorage _storage;

    public DocumentService(IStorage storage)
    {
        _storage = storage;
    }
    
    public async Task<bool> AddDocument(DocumentModel model, CancellationToken cancellationToken)
    {
        ValidateDocument(model);
        var storageModel = new DocumentStorageModel
        {
            Id = model.Id!,
            Tags = model.Tags!,
            Data = model.Data!
        };

        var success = await _storage.SaveDocument(storageModel, cancellationToken);
        if (!success)
        {
            throw new DocumentException("Error during SaveDocument operation");
        }
        
        return success;
    }

    public async Task<bool> EditDocument(string id, DocumentModel model, CancellationToken cancellationToken)
    {
        var document = await _storage.GetById(id, cancellationToken);
        if (document is null)
        {
            throw new DocumentNotFoundException($"Document with identifier {id} was not found");
        }

        document.Tags = model.Tags ?? document.Tags;
        document.Data = model.Data ?? document.Data;

        var result = await _storage.SaveDocument(document, cancellationToken);
        return result;
    }

    public async Task<DocumentModel> GetDocument(string id, CancellationToken cancellationToken)
    {
        var storageModel = await _storage.GetById(id, cancellationToken);
        if (storageModel is null)
        {
            throw new DocumentNotFoundException($"Document with identifier {id} was not found");
        }
        
        var result = new DocumentModel
        {
            Id = storageModel.Id,
            Tags = storageModel.Tags,
            Data = storageModel.Data
        };

        return result;
    }

    private static void ValidateDocument(DocumentModel model)
    {
        if (string.IsNullOrEmpty(model.Id))
        {
            throw new DocumentException("Id field is required");
        }

        if (model.Tags?.Any() != true)
        {
            throw new DocumentException("Tags field is required");
        }

        if (model.Data is null)
        {
            throw new DocumentException("Data field is required");
        }
    }
}