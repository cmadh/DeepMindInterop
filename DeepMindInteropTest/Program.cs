using System.Diagnostics;
using DeepMindInterop.Extensions;

namespace DeepMindInteropTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                using (var nodeosSwig = new NodeosSwig())
                {

                    var logger = new DeepMindInteropLogger();

                    var aargs = new string[]
                    {
                        "",
                        //"--delete-all-blocks",
                        "--replay-blockchain",
                        //"--terminate-at-block", "739449",
                        "--data-dir", "/home/cmadh/swig_testing/data",
                        "--config-dir", "/home/cmadh/swig_testing/config",
                        "--genesis-json", "/home/cmadh/swig_testing/genesis.json"
                    };

                    using (var stringVector = new StringVector(aargs))
                    {
                        nodeosSwig.Start(aargs.Length, stringVector, logger);
                    }
                }

                Debug.WriteLine("Nodeos Stopped");

            }).Start();
        }
    }
}