using InvoiceFlow.Api.Data; using InvoiceFlow.Api.Models; using InvoiceFlow.Api.Services; using Microsoft.EntityFrameworkCore;
namespace InvoiceFlow.Tests;
public class InvoiceServiceTests {
 [Fact] public async Task CreateInvoice_PersistsInvoice(){var o=new DbContextOptionsBuilder<InvoiceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;await using var db=new InvoiceDbContext(o);var s=new InvoiceService(db,new InMemoryAuditEventStore());var x=await s.CreateAsync(new("Kefron Demo","INV-1",100));Assert.Equal("Received",x.Status);Assert.Single(await s.GetAllAsync());}
 [Fact] public async Task CreateInvoice_RejectsNonPositiveAmount(){var o=new DbContextOptionsBuilder<InvoiceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;await using var db=new InvoiceDbContext(o);var s=new InvoiceService(db,new InMemoryAuditEventStore());await Assert.ThrowsAsync<ArgumentException>(()=>s.CreateAsync(new("Vendor","INV-2",0)));}
}
