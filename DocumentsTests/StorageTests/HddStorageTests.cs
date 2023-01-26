using DocumentCommon.Options;
using DocumentStorage.Models;
using DocumentStorage.Storages;
using Xunit;

namespace DocumentsTests.StorageTests;

public sealed class HddStorageTests : IDisposable
{
    private readonly DocumentStorageModel _testDocument;
    private readonly HddStorage _storage;
    private readonly string _testFilePath;

    public HddStorageTests()
    {
        var options = new StorageOptions { HddStoragePath = Path.GetTempPath() };
        _storage = new HddStorage(options);
        _testFilePath = Path.Combine(options.HddStoragePath, "test.json");
        _testDocument = new DocumentStorageModel
        {
            Id = "test",
            Tags = new List<string> { "tag1", "tag2" },
            Data = "test data"
        };
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
    
    [Fact]
    public async Task SaveDocument_ShouldSaveDocumentToFile()
    {
        var result = await _storage.SaveDocument(_testDocument, CancellationToken.None);

        Assert.True(result);
        Assert.True(File.Exists(_testFilePath));
    }

    [Fact]
    public async Task GetById_ShouldReturnDocument()
    {
        await _storage.SaveDocument(_testDocument, CancellationToken.None);
        
        var result = await _storage.GetById(_testDocument.Id, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(_testDocument.Id, result.Id);
        Assert.Equal(_testDocument.Data, result.Data);
        Assert.Equal(_testDocument.Tags, result.Tags);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull()
    {
        var result = await _storage.GetById("non-existent", CancellationToken.None);
        
        Assert.Null(result);
    }
}