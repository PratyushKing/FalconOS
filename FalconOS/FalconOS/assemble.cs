using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.IO;
>>>>>>> 15fe22673d59a0d51d432693317421e44efc1937
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
<<<<<<< HEAD
    internal class assemble
    {
        public static void run(string line)
        {

=======
    internal static class assemble
    {
        public static string arg1 = "0";
        public static string arg2 = "0";
        public static string arg3 = "0";
        public static string arg4 = "0";
        public static string arg5 = "0";

        public enum interrupt
        {
            WriteCommandLine,
            ReadCommandLine,
            WriteVar,
            FileWrite,
            FileRead,
            DirectoryCreate,
            DirectoryDelete,
            RunShell,
            Unrecognized
        }

        public static void handleCode(string code)
        {
            var lines = code.Split('\n');
            foreach (var line in lines)
            {
                if (line.StartsWith("push "))
                {
                    var pushargs = line.Replace("push ", "").Split(' ');
                    push(pushargs[0], pushargs[1]);
                }
                else if (line.StartsWith("pop "))
                {
                    pop(line.Replace("pop ", ""));
                }
                else if (line.StartsWith("int "))
                {
                    switch (line.Replace("int ", ""))
                    {
                        case "0":
                            doAction(interrupt.WriteCommandLine);
                            break;
                        case "1":
                            doAction(interrupt.ReadCommandLine);
                            break;
                        case "2":
                            doAction(interrupt.WriteVar);
                            break;
                        case "3":
                            doAction(interrupt.FileWrite);
                            break;
                        case "4":
                            doAction(interrupt.FileRead);
                            break;
                        case "5":
                            doAction(interrupt.DirectoryCreate);
                            break;
                        case "6":
                            doAction(interrupt.DirectoryDelete);
                            break;
                        case "7":
                            doAction(interrupt.RunShell);
                            break;
                    }
                }
                /*else if (line.StartsWith("list"))
                {
                    Console.WriteLine(arg1 + Environment.NewLine + arg2 + Environment.NewLine + arg3 + Environment.NewLine + arg4 + Environment.NewLine + arg5);
                }*/
            }
        }

        public static void reset()
        {
            arg1 = "0"; arg2 = "0"; arg3 = "0"; arg4 = "0"; arg5 = "0";
            return;
        }

        public static string push(string to, string what)
        {
            to = to.ToLower();
            switch (to)
            {
                case "arg1":
                    arg1 = what; break;
                case "arg2":
                    arg2 = what; break;
                case "arg3":
                    arg3 = what; break;
                case "arg4":
                    arg4 = what; break;
                case "arg5":
                    arg5 = what; break;
                default:
                    return "error";
            }
            return "0";
        }

        public static void pop(string arg)
        {
            switch (arg)
            {
                case "arg1": arg1 = ""; break;
                case "arg2": arg1 = ""; break;
                case "arg3": arg1 = ""; break;
                case "arg4": arg1 = ""; break;
                case "arg5": arg1 = ""; break;
                default: return;
            }
        }

        public static interrupt doAction(interrupt input)
        {
            if (input == interrupt.WriteCommandLine)
            {
                Console.Write(arg1);
                if (arg2 == "1") { Console.Write("\n"); }
            }
            else if (input == interrupt.ReadCommandLine)
            {
                var fetchOutput = Console.ReadLine();
                arg1 = fetchOutput;
            }
            else if (input == interrupt.WriteVar)
            {
                Console.WriteLine(fetch(arg1));
            }
            else if (input == interrupt.FileWrite)
            {
                File.WriteAllText(data.currentDir + arg1, arg2);
            }
            else if (input == interrupt.FileRead)
            {
                arg2 = File.ReadAllText(arg1);
            }
            else if (input == interrupt.DirectoryCreate)
            {
                Directory.CreateDirectory(data.currentDir + arg1);
            }
            else if (input == interrupt.DirectoryDelete)
            {
                Directory.Delete(data.currentDir + arg1);
            }
            else if (input == interrupt.RunShell)
            {
                Shell shell = new Shell("FalconOS ASSEMBLY", data.ver);
                shell.exec(fetch(arg1));
            }
            return interrupt.Unrecognized;
        }

        public static void writeTo(string arg, string what)
        {
            switch (arg)
            {
                case "arg1": arg1 = what; break;
                case "arg2": arg2 = what; break;
                case "arg3": arg3 = what; break;
                case "arg4": arg4 = what; break;
                case "arg5": arg5 = what; break;
                default: return;
            }
            return;
        }

        public static string fetch(string arg)
        {
            switch (arg)
            {
                case "arg1": return arg1;
                case "arg2": return arg2;
                case "arg3": return arg3;
                case "arg4": return arg4;
                case "arg5": return arg5;
                default: return "NullArg";
            }
>>>>>>> 15fe22673d59a0d51d432693317421e44efc1937
        }
    }
}
