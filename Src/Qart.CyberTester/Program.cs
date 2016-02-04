﻿using CommandLine;
using Common.Logging;
using Qart.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Qart.CyberTester
{
    class Options
    {
        [Option('d', "dir", Required = false, HelpText = "Path to the directory(s) with testcase(s).")]
        public string Dir { get; set; }

        [Option('r', "rebaseline", Required = false, HelpText = "Overwrites expected results")]
        public bool Rebaseline { get; set; }

        [Option('h', "help", Required = false, HelpText = "Usage")]
        public bool Usage { get; set; }
    }

    class Program
    {
        static ILog Logger = LogManager.GetLogger("");

        static int Main(string[] args)
        {
            int result = -1;
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                if (string.IsNullOrEmpty(options.Dir))
                {
                    options.Dir = Directory.GetCurrentDirectory();
                }
                var tracer = new TestCaseTracer();
                Execute(options, tracer);
                result = tracer.Results.Count(_ => _.Exception != null);
                Logger.InfoFormat("Failed testcases: {0}", result);

                XElement root = new XElement("TestResults", tracer.Results.Select(_ => new XElement("Test", new XAttribute("id", _.TestCase.Id), new XAttribute("status", _.Exception == null ? "succeeded" : "failed"))));
                root.Save("TestSessionResults.xml");

            }
            return result;
        }

        static void Execute(Options options, ITestCaseTracer tracer)
        {
            Logger.DebugFormat("Rebaseline [{0}], TestCases [{1}]", options.Rebaseline, options.Dir);

            var container = Bootstrapper.CreateContainer();

            var testSystem = new TestSystem(new Qart.Testing.FileBased.DataStore(options.Dir));
            Logger.Debug("Looking for test cases.");
            var testCases = testSystem.GetTestCases();

            using(var testSession = container.Resolve<ITestSession>())
            {
                foreach (var testCase in testCases)
                {
                    Logger.DebugFormat("Starting processing test case [{0}]", testCase.Id);
                    tracer.OnBegin(testCase);
                    try
                    {
                        var feature = testCase.GetContent(".test");
                        var processor = container.Resolve<ITestCaseProcessor>(feature);
                        processor.Process(testCase);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("An exception was raised while processing [{0}]", testCase.Id), ex);
                        tracer.OnFailure(testCase, ex);
                    }
                    tracer.OnFinish(testCase);
                    Logger.DebugFormat("Finished processing test case [{0}]", testCase.Id);
                }
            }
        }
    }
}
