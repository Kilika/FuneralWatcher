using FuneralWatcher.Configuration;
using FuneralWatcher.Logging.ConsoleLogger;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic;
using FuneralWatcher.Logic.Contract;
using FuneralWatcher.Logic.ImageProcessor;
using FuneralWatcher.Logic.TessImageInterpreter;
using FuneralWatcher.Logic.WindowsScreenCastImageProvider;
using FuneralWatcher.Workflows;
using Ninject;

IKernel kernel = new StandardKernel();
kernel.Bind<IImageInterpreter>().To<TessImageInterpreter>();
kernel.Bind<IImageProcessor>().To<ImageProcessor>();
kernel.Bind<IScanner>().To<DevScanner>();
kernel.Bind<IResultProcessor>().To<FileResultProcessor>();
kernel.Bind<IImageProvider>().To<WindowsScreenCastImageProvider>();

kernel.Bind<ILogger>().To<ConsoleLogger>();
kernel.Bind<IConfiguration>().To<Configuration>().InSingletonScope();
kernel.Bind<IConfigurationRepository>().To<ConfigurationRepository>().InSingletonScope();

var workflow = kernel.Get<IScanner>();
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
