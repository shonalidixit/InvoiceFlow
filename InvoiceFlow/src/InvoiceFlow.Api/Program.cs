using InvoiceFlow.Api.Data;
using InvoiceFlow.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<InvoiceDbContext>(o => o.UseInMemoryDatabase("InvoiceFlow"));
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddSingleton<IAiInvoiceExtractor, MockAiInvoiceExtractor>();
builder.Services.AddSingleton<IBlobStorageService, LocalBlobStorageService>();
builder.Services.AddSingleton<IAuditEventStore, InMemoryAuditEventStore>();
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection(); app.MapControllers(); app.Run();
public partial class Program { }
