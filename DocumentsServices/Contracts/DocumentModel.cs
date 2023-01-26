namespace DocumentServices.Contracts;

public sealed class DocumentModel
{
    public string? Id { get; init; }
    public List<string>? Tags { get; init; }
    public object? Data { get; set; }
}