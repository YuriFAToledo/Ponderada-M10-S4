using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseHttpMetrics(); // Adiciona a coleta de métricas HTTP
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
});

Meter meter = new Meter("MetricsDemo", "1.0");
Counter<long> requestCounter = meter.CreateCounter<long>("requests");
Histogram<float> responseTimeHistogram = meter.CreateHistogram<float>("response_time", "ms");

Random random = new Random();
for (int i = 0; i < 10; i++)
{
    requestCounter.Add(1);
    float responseTime = random.Next(50, 500); // Simula um tempo de resposta entre 50ms e 500ms
    responseTimeHistogram.Record(responseTime);
    Console.WriteLine($"Request {i + 1} processed in {responseTime}ms.");
}

app.Run();
