using FuneralWatcher.Configuration;
using FuneralWatcher.Logging.ConsoleLogger;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic;
using FuneralWatcher.Logic.BasicImageEditor;
using FuneralWatcher.Logic.Contracts;
using FuneralWatcher.Logic.EmguImageInterpreter;
using FuneralWatcher.Logic.TessImageInterpreter;
using FuneralWatcher.Logic.WindowsScreenCastImageProvider;
using FuneralWatcher.Workflows;
using Ninject;

IKernel kernel = new StandardKernel();
kernel.Bind<IImageInterpreter>().To<TessImageInterpreter>();
kernel.Bind<IImageRecognizer>().To<EmguImageRecognizer>();
kernel.Bind<IWorkflow>().To<ImageEmguWorkflow>();
kernel.Bind<IResultProcessor>().To<FileResultProcessor>();
kernel.Bind<IImageEditor>().To<BasicImageEditor>();
kernel.Bind<IImageProvider>().To<WindowsScreenCastImageProvider>();

kernel.Bind<ILogger>().To<ConsoleLogger>().InSingletonScope();
kernel.Bind<IConfiguration>().To<Configuration>().InSingletonScope();
kernel.Bind<IConfigurationRepository>().To<ConfigurationRepository>().InSingletonScope();

var workflow = kernel.Get<IWorkflow>();
var resultProcess = kernel.Get<IResultProcessor>();
var logger = kernel.Get<ILogger>();
workflow.PatternMatchingFlankDetected += (sender, eventArgs) =>
{
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
