using System.Drawing;
using FuneralWatcher.Configuration;
using FuneralWatcher.Logging.ConsoleLogger;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic;
using FuneralWatcher.Logic.Contracts;
using FuneralWatcher.Logic.EmguImageInterpreter;
using FuneralWatcher.Logic.StaticImageProvider;
using FuneralWatcher.Logic.TessImageInterpreter;
using FuneralWatcher.Logic.WindowsScreenCastImageProvider;
using FuneralWatcher.Workflows;
using Ninject;

IKernel kernel = new StandardKernel();
InitializeBindings(kernel);

var workflow = kernel.Get<IWorkflow>();
var resultProcess = kernel.Get<IResultProcessor>();
var logger = kernel.Get<ILogger>();


// Define Handler
workflow.PatternMatchingFlankChangeDetected += (sender, eventArgs) => {
    if (eventArgs.NewDetection)
    {
        Console.WriteLine("Dead detected");
        resultProcess.Process();
    }
    else
    {
        Console.WriteLine("Respawn detected");
    }
};

try
{
    workflow.Run();
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

void InitializeBindings(IKernel kernel1)
{
    kernel1.Bind<ILogger>().To<ConsoleLogger>().InSingletonScope();
    kernel1.Bind<IConfiguration>().To<Configuration>().InSingletonScope();
    kernel1.Bind<IConfigurationRepository>().To<ConfigurationRepository>().InSingletonScope();

    kernel1.Bind<IWorkflow>().To<DevWorkflow>();
    kernel1.Bind<IImageProvider>().To<StaticImageProvider>();
    kernel1.Bind<IImageRecognizer>().To<DeadScreenRecognition>();
    kernel1.Bind<IImageInterpreter>().To<TessImageInterpreter>();
    kernel1.Bind<IResultProcessor>().To<FileResultProcessor>();
}

