// See https://aka.ms/new-console-template for more information
using gWorks.Hiring.Data;
using gWorks.Hiring.Services;
using gWorks.Hiring.TestConsoleApplication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// configure services
builder.Services.AddSchoolDbContext();
builder.Services.AddSingleton<Application>();
builder.Services.AddScoped<ISchoolDataSeedService, SchoolDataSeedService>();
builder.Services.AddScoped<IStudentService, StudentService>();

await builder.RunApplicationWithEntryPoint<Application>(async a => await a.Run());