using HW_2_POS.Application.Ports;
using HW_2_POS.Application.Services;
using HW_2_POS.Domain.Repositories;
using HW_2_POS.Domain.Services;
using HW_2_POS.Infrastructure.Export;
using HW_2_POS.Infrastructure.Repositories;
using HW_2_POS.Infrastructure.Storage;
using HW_2_POS.Presentation;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Infrastructure (adapters)
services.AddSingleton<IDataStorage>(_ => new JsonStorage("pos-data.json"));
services.AddSingleton<IStudentRepository, JsonStudentRepository>();
services.AddSingleton<IRubricRepository, JsonRubricRepository>();
services.AddSingleton<IGradesRepository, JsonGradesRepository>();
services.AddSingleton<IResultExporter, CSVResultExporter>();
services.AddSingleton<IResultExporter, JsonResultExporter>();

services.AddSingleton<IFinalScoreCalculator>(
    s => new RoundingCalculator(new WeightedAverageCalculator(), digits: 2));

// Application services (use cases)
services.AddSingleton<IStudentService, StudentService>();
services.AddSingleton<IRubricService, RubricService>();
services.AddSingleton<IGradingService, GradingService>();
services.AddSingleton<IReportService, ReportService>();

// Presentation
services.AddSingleton<IConsoleUI, ConsoleUI>();
var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetRequiredService<IConsoleUI>().Run();
