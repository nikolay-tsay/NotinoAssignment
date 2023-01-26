using DocumentServices.Contracts;

namespace DocumentServices.Interfaces;

public interface IDocumentService
{
    Task<bool> AddDocument(DocumentModel model, CancellationToken cancellationToken);
    Task<bool> EditDocument(string id, DocumentModel model, CancellationToken cancellationToken);
    Task<DocumentModel> GetDocument(string id, CancellationToken cancellationToken);
}