namespace DocumentStorage.Models;

public sealed class DocumentStorageModel
{
    public required string Id { get; init; }
    public List<string>? Tags { get; set; }
    public required object Data { get; set; }
}