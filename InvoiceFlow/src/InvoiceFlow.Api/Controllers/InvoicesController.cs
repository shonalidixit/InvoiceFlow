using InvoiceFlow.Api.Models; using InvoiceFlow.Api.Services; using Microsoft.AspNetCore.Mvc;
namespace InvoiceFlow.Api.Controllers;
[ApiController][Route("api/invoices")]
public class InvoicesController(IInvoiceService service,IAiInvoiceExtractor ai,IBlobStorageService blobs):ControllerBase {
 [HttpGet] public async Task<ActionResult<IReadOnlyList<Invoice>>> All()=>Ok(await service.GetAllAsync());
 [HttpGet("{id:guid}")] public async Task<ActionResult<Invoice>> One(Guid id){var x=await service.GetAsync(id);return x is null?NotFound():Ok(x);}
 [HttpPost] public async Task<ActionResult<Invoice>> Create(CreateInvoiceRequest request){try{var x=await service.CreateAsync(request);return CreatedAtAction(nameof(One),new{id=x.Id},x);}catch(ArgumentException e){return BadRequest(new{error=e.Message});}}
 [HttpPost("extract")] public async Task<ActionResult<ExtractionResult>> Extract(IFormFile file,CancellationToken ct){if(file.Length==0)return BadRequest();await using var s=file.OpenReadStream();var uri=await blobs.UploadAsync(file.FileName,s,ct);await using var s2=file.OpenReadStream();var result=await ai.ExtractAsync(s2,ct);Response.Headers.Append("X-Blob-Uri",uri);return Ok(result);}
}
