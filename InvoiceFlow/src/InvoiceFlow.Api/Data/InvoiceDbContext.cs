using InvoiceFlow.Api.Models; using Microsoft.EntityFrameworkCore;
namespace InvoiceFlow.Api.Data;
public class InvoiceDbContext(DbContextOptions<InvoiceDbContext> options):DbContext(options){ public DbSet<Invoice> Invoices => Set<Invoice>(); }
