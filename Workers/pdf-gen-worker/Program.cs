using pdf_gen_worker;
using pdf_gen_worker.Queues;
using pdf_gen_worker.Storages;
using pdf_gen_worker.PDF_Generators;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IMessageQueueConsumer, AmazonSqsService>();
builder.Services.AddSingleton<IFileStorage, CloudflareR2>();
builder.Services.AddSingleton<IPdfGenerator, PdfGenerator>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
