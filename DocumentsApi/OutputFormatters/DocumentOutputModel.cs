namespace NotinoAssignment.OutputFormatters;

public class DocumentOutputModel
{
    public required  string Id { get; set; }
    public required List<string> Tags { get; set; }
    public required object Data { get; set; }
}