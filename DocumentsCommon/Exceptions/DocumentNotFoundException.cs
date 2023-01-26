using Microsoft.AspNetCore.Http;

namespace DocumentCommon.Exceptions;

public sealed class DocumentNotFoundException : DocumentException
{
    public DocumentNotFoundException(string? message = null) : base(message) { }

    public override int GetStatusCode()
    {
        return StatusCodes.Status404NotFound;
    }
}