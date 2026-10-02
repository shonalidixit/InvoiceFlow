using Microsoft.Azure.Functions.Worker; using Microsoft.DurableTask; using Microsoft.DurableTask.Client; using Microsoft.Azure.Functions.Worker.Http; using System.Net;
namespace InvoiceFlow.Functions;
public static class InvoiceOrchestration {
 [Function("StartInvoiceWorkflow")] public static async Task<HttpResponseData> Start([HttpTrigger(AuthorizationLevel.Function,"post")] HttpRequestData req,[DurableClient] DurableTaskClient client){var id=await client.ScheduleNewOrchestrationInstanceAsync("ProcessInvoice",Guid.NewGuid().ToString());var r=req.CreateResponse(HttpStatusCode.Accepted);await r.WriteStringAsync(id);return r;}
 [Function("ProcessInvoice")] public static async Task<string> Run([OrchestrationTrigger] TaskOrchestrationContext ctx){var id=ctx.GetInput<string>()!;await ctx.CallActivityAsync("ValidateInvoice",id);await ctx.CallActivityAsync("PersistAuditEvent",id);return "Approved";}
 [Function("ValidateInvoice")] public static string Validate([ActivityTrigger] string id)=>$"Validated {id}";
 [Function("PersistAuditEvent")] public static string Audit([ActivityTrigger] string id)=>$"Audited {id}";
}
