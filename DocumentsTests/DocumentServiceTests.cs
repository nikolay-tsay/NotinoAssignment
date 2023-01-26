using DocumentCommon.Exceptions;
using DocumentServices;
using DocumentServices.Contracts;
using DocumentServices.Interfaces;
using DocumentStorage.Interfaces;
using DocumentStorage.Models;
using NSubstitute;
using Xunit;

namespace DocumentsTests;

public sealed class DocumentServiceTests
{
    private readonly IStorage _storage;
    private readonly IDocumentService _service;

    public DocumentServiceTests()
    {
        _storage = Substitute.For<IStorage>();
        _service = new DocumentService(_storage);
    }

    [Fact]
    public async Task AddDocument_ValidInput_ReturnsTrue()
    {
        var model = new DocumentModel
        {
            Id = "123",
            Tags = new List<string> { "tag1" },
            Data = "data"
        };

        _storage.SaveDocument(Arg.Any<DocumentStorageModel>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        var result = await _service.AddDocument(model, CancellationToken.None);

        Assert.True(result);
        await _storage.Received().SaveDocument(Arg.Any<DocumentStorageModel>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddDocument_InvalidInput_ThrowsDocumentException()
    {
        var model = new DocumentModel();
        var exception = await Assert.ThrowsAsync<DocumentException>(() => _service.AddDocument(model, CancellationToken.None));

        Assert.Equal("Id field is required", exception.Message);
    }

    [Fact]
    public async Task AddDocument_ErrorDuringSave_ThrowsDocumentException()
    {
        var model = new DocumentModel
        {
            Id = "123",
            Tags = new List<string> { "tag1" },
            Data = "data"
        };

        _storage.SaveDocument(Arg.Any<DocumentStorageModel>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        var exception = await Assert.ThrowsAsync<DocumentException>(() => _service.AddDocument(model, CancellationToken.None));

        Assert.Equal("Error during SaveDocument operation", exception.Message);
    }

    [Fact]
    public async Task EditDocument_ValidInput_ReturnsTrue()
    {
        var model = new DocumentModel
        {
            Tags = new List<string> { "tag2" },
            Data = "new data"
        };

        _storage.GetById("123", Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(new DocumentStorageModel
            {
                Id = "123",
                Tags = new List<string> { "tag1" },
                Data = "data"
            }));

        _storage.SaveDocument(Arg.Any<DocumentStorageModel>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        var result = await _service.EditDocument("123", model, CancellationToken.None);

        Assert.True(result);
        
        await _storage
            .Received()
            .SaveDocument(Arg.Is<DocumentStorageModel>(x 
                => x.Tags != null && x.Tags[0] == "tag2"), 
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EditDocument_DocumentNotFound_ThrowsDocumentNotFoundException()
    {
        _storage.GetById("123", Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult((DocumentStorageModel)null!));
        
        var exception = await Assert.ThrowsAsync<DocumentNotFoundException>(() => _service.EditDocument("123", new DocumentModel(), CancellationToken.None));

        Assert.Equal("Document with identifier 123 was not found", exception.Message);
    }

    [Fact]
    public async Task GetDocument_ValidInput_ReturnsDocumentModel()
    {
        _storage.GetById("123", Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(new DocumentStorageModel
            {
                Id = "123",
                Tags = new List<string> { "tag1" },
                Data = "data"
            }));

        var result = await _service.GetDocument("123", CancellationToken.None);

        Assert.Equal("123", result.Id);
        Assert.Equal(new List<string> { "tag1" }, result.Tags);
        Assert.Equal("data", result.Data);
    }

    [Fact]
    public async Task GetDocument_DocumentNotFound_ThrowsDocumentNotFoundException()
    {
        _storage.GetById("123", Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult((DocumentStorageModel)null!));

        var exception = await Assert.ThrowsAsync<DocumentNotFoundException>(() => _service.GetDocument("123", CancellationToken.None));

        Assert.Equal("Document with identifier 123 was not found", exception.Message);
    }
}
