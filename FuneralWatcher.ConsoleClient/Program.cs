using Ninject;
using FuneralWatcher.Logic;
using FuneralWatcher.Logic.Contract;
using FuneralWatcher.Settings;
using FuneralWatcher.Workflows;

IKernel kernel = new StandardKernel();
kernel.Bind<IImageInterpreter>().To<ImageInterpreter>();
kernel.Bind<IImageProcessor>().To<ImageProcessor>();
kernel.Bind<IScanner>().To<DevScanner>();
kernel.Bind<IResultProcessor>().To<FileResultProcessor>();
kernel.Bind<IImageProvider>().To<WindowsScreenCastImageProvider>();

kernel.Bind<ILogger>().To<ConsoleLogger>();
kernel.Bind<IConfiguration>().To<Configuration>().InSingletonScope();
kernel.Bind<IConfigurationRepository>().To<ConfigurationRepository>().InSingletonScope();

var workflow = kernel.Get<IScanner>();
var resultProcess = kernel.Get<IResultProcessor>();
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

workflow.Run();