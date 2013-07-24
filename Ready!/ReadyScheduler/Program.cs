using System.ServiceProcess;
using ReadyScheduler.Tasks;

namespace ReadyScheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ReadyScheduler() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
