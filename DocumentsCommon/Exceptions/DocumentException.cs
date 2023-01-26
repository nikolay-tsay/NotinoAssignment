using Microsoft.AspNetCore.Http;

namespace DocumentCommon.Exceptions;

public class DocumentException : Exception
{
    public DocumentException(string? message = null, Exception? innerException = null): base(message, innerException) { }

    public virtual int GetStatusCode()
    {
        return StatusCodes.Status500InternalServerError;
    }
}