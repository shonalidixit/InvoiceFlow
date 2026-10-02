using InvoiceFlow.Api.Data; using InvoiceFlow.Api.Models; using Microsoft.EntityFrameworkCore;
namespace InvoiceFlow.Api.Services;
public interface IInvoiceService { Task<Invoice> CreateAsync(CreateInvoiceRequest r); Task<IReadOnlyList<Invoice>> GetAllAsync(); Task<Invoice?> GetAsync(Guid id); }
public class InvoiceService(InvoiceDbContext db, IAuditEventStore audit):IInvoiceService {
 public async Task<Invoice> CreateAsync(CreateInvoiceRequest r){
  if(string.IsNullOrWhiteSpace(r.Vendor) || r.Amount<=0) throw new ArgumentException("Vendor and positive amount are required.");
  var x=new Invoice{Vendor=r.Vendor,InvoiceNumber=r.InvoiceNumber,Amount=r.Amount}; db.Invoices.Add(x); await db.SaveChangesAsync();
  await audit.WriteAsync(new(x.Id,"InvoiceCreated",DateTime.UtcNow,$"Invoice {x.InvoiceNumber} created")); return x;
 }
 public async Task<IReadOnlyList<Invoice>> GetAllAsync()=>await db.Invoices.AsNoTracking().OrderByDescending(x=>x.InvoiceDate).ToListAsync();
 public Task<Invoice?> GetAsync(Guid id)=>db.Invoices.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
}
public interface IAiInvoiceExtractor { Task<ExtractionResult> ExtractAsync(Stream document,CancellationToken ct=default); }
public class MockAiInvoiceExtractor:IAiInvoiceExtractor { public Task<ExtractionResult> ExtractAsync(Stream d,CancellationToken ct=default)=>Task.FromResult(new ExtractionResult("Demo Vendor","INV-AI-001",1250.00m,.97)); }
public interface IBlobStorageService { Task<string> UploadAsync(string name,Stream content,CancellationToken ct=default); }
public class LocalBlobStorageService:IBlobStorageService { public async Task<string> UploadAsync(string n,Stream c,CancellationToken ct=default){ var dir=Path.Combine(Path.GetTempPath(),"invoiceflow");Directory.CreateDirectory(dir);var p=Path.Combine(dir,$"{Guid.NewGuid()}-{Path.GetFileName(n)}");await using var f=File.Create(p);await c.CopyToAsync(f,ct);return new Uri(p).AbsoluteUri; } }
public interface IAuditEventStore { Task WriteAsync(AuditEvent e); IReadOnlyCollection<AuditEvent> Events {get;} }
public class InMemoryAuditEventStore:IAuditEventStore { private readonly List<AuditEvent> _e=[]; public IReadOnlyCollection<AuditEvent> Events=>_e.AsReadOnly(); public Task WriteAsync(AuditEvent e){_e.Add(e);return Task.CompletedTask;} }
