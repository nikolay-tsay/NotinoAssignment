using DocumentStorage.Models;
using DocumentStorage.Storages;
using Xunit;

namespace DocumentsTests.StorageTests;

public class InMemoryStorageTests
{
    private readonly InMemoryStorage _storage;

    public InMemoryStorageTests()
    {
        _storage = new InMemoryStorage();
    }
    
    [Fact]
    public async Task SaveDocument_ShouldAddNewDocumentToStorage()
    {
        var document = new DocumentStorageModel
        {
            Id = "1",
            Tags = new List<string> { "tag1" },
            Data = "data"
        };

        var result = await _storage.SaveDocument(document, CancellationToken.None);
        Assert.True(result);
        
        var savedDocument = await _storage.GetById("1", CancellationToken.None);
        Assert.Equal(document, savedDocument);
    }

    [Fact]
    public async Task SaveDocument_ShouldUpdateExistingDocument()
    {
        var document = new DocumentStorageModel
        {
            Id = "1",
            Tags = new List<string> { "tag1" },
            Data = "data"
        };
        
        await _storage.SaveDocument(document, CancellationToken.None);
        var newDocument = new DocumentStorageModel
        {
            Id = "1",
            Tags = new List<string> { "tag1", "tag2" },
            Data = "new data"
        };
        
        var result = await _storage.SaveDocument(newDocument, CancellationToken.None);
        Assert.True(result);
        
        var savedDocument = await _storage.GetById("1", CancellationToken.None);
        Assert.Equal(newDocument, savedDocument);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull()
    {
        var result = await _storage.GetById("non-existent", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnDocument()
    {
        var document = new DocumentStorageModel
        {
            Id = "1",
            Tags = new List<string> { "tag1" },
            Data = "data"
        };
        
        await _storage.SaveDocument(document, CancellationToken.None);

        var result = await _storage.GetById("1", CancellationToken.None);
        Assert.Equal(document, result);
    }
}