namespace InvoiceFlow.Api.Models;
public class Invoice {
 public Guid Id { get; set; } = Guid.NewGuid();
 public string Vendor { get; set; } = "";
 public string InvoiceNumber { get; set; } = "";
 public decimal Amount { get; set; }
 public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
 public string Status { get; set; } = "Received";
 public string? BlobUri { get; set; }
}
public record CreateInvoiceRequest(string Vendor,string InvoiceNumber,decimal Amount);
public record ExtractionResult(string Vendor,string InvoiceNumber,decimal Amount,double Confidence);
public record AuditEvent(Guid InvoiceId,string EventType,DateTime Timestamp,string Detail);
