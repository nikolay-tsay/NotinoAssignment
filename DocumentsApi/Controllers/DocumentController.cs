using DocumentServices.Contracts;
using DocumentServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace NotinoAssignment.Controllers;

[ApiController]
public sealed class DocumentController : ControllerBase
{
    private readonly IDocumentService _service;
    
    public DocumentController(IDocumentService service)
    {
        _service = service;
    }

    /// <summary>
    /// Add new document to storage
    /// </summary>
    /// <param name="model">Document to add</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>True/False if operation was successful</returns>
    [HttpPost("documents")]
    [Produces("application/json")]
    public async Task<IActionResult> AddDocument([FromBody] DocumentModel model, CancellationToken cancellationToken)
    {
        var result = await _service.AddDocument(model, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Edit document info
    /// </summary>
    /// <param name="id">Documents unique identifier</param>
    /// <param name="model">Update model</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>True/False if operation was successful</returns>
    [HttpPut("documents/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> EditDocument(string id, [FromBody] DocumentModel model, CancellationToken cancellationToken)
    {
        var result = await _service.EditDocument(id, model, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieve document from storage
    /// </summary>
    /// <param name="id">Documents unique identifier</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns></returns>
    [HttpGet("documents/{id}")]
    public async Task<IActionResult> GetDocument(string id, CancellationToken cancellationToken)
    {
        var result = await _service.GetDocument(id, cancellationToken);

        return Ok(result);
    }
}