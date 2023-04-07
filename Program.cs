﻿//THIS INTERPRETER IS IN A VERY EARLY STATE!
// E.U.0001: Still in string mode even at end of line (Try to add a "\"")
// E.U.0002: Still in method mode even at end of line (Try to add a "]")
// E.U.0003: Still in NumCalculation mode even at end of line (Try to add a ")")
// E.U.0004: Invalid Command.CommandType at method Statement part 2
// E.U.0005: Invalid Command.CommandType at method Statement part 3
// E.U.0006: Invalid CodeContainer direction at method Statement part 3
// E.U.0007: Still in method mode even at end of file/parent method (Try to add a "}")
// E.U.0008: Can't create a variable with the type void
// E.U 0009: Can't create an array with the variable type "Void". Will that even be possible? Idk!

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TASI
{
    class TASI_Main
    {





        public const string interpreterVer = "1.0";
        public static Logger interpretInitLog = new();
        public static void Main(string[] args)
        {
            Global.currentLine = -1;
            Console.WriteLine("Doing tests...");
            Tests.NumCalcTests();
            //SyntaxAnalysis.AnalyseSyntax(StringProcess.ConvertLineToCommand("set helloWorld [Console.ReadLine];"));
            Console.ReadKey(false);
            Console.Clear();

            Console.WriteLine("Enter file location with code:");



            Global.InitInternalNamespaces();
            

            Stopwatch codeRuntime = new();
            codeRuntime.Start();
            Console.WriteLine("Comment-Removing and analysing tokens");
            //Remove comments 
            try
            {
                
                List<Command> commands = LoadFile.ByPath(Console.ReadLine() ?? throw new Exception("Code is null."));

                Console.WriteLine($"Finished token analysis; Interpreting. It took {codeRuntime.ElapsedMilliseconds}ms");

                
                var startValues = InterpretMain.InterpretHeaders(commands);
                Console.WriteLine($"{Global.Namespaces}{Global.allLoadedFiles}");
                var startCode = startValues.Item1 ?? throw new Exception("You can't start a library-type namespace directly.");


                foreach (NamespaceInfo namespaceInfo in Global.Namespaces) //Activate methodcalls after scanning headers to not cause any errors. BTW im sorry
                {
                    foreach(Method method in namespaceInfo.namespaceMethods)
                    {
                        foreach (List<Command> methodCodeOverload in method.methodCode)
                        {
                            foreach(Command overloadCode in methodCodeOverload)
                            {
                                if (overloadCode.commandType == Command.CommandTypes.MethodCall) overloadCode.methodCall.SearchCallMethod(namespaceInfo);
                            }
                        }
                    }
                    
                }
                foreach (Command command in startValues.Item1)
                {
                    if (command.commandType == Command.CommandTypes.MethodCall) command.methodCall.SearchCallMethod(startValues.Item2);
                }


                InterpretMain.InterpretNormalMode(startCode, new(new(), startValues.Item2));
                codeRuntime.Stop();
                Console.WriteLine($"Code finished; Runtime: {codeRuntime.ElapsedMilliseconds} ms");
                Console.ReadKey(false);

            }
            catch (NotImplementedException e)
            {

                Console.Clear();
                Console.WriteLine("There was an error interpreting your code:\n");
                Console.WriteLine(e.Message);
                if (Global.currentLine != -1)
                    Console.WriteLine($"\nThe error happened on line: {Global.currentLine + 1}");
                Console.ReadKey();


            }


            return;


        }
    }
}