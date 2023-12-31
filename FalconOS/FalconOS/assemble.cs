﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
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


        public static string compile(string code, string output)
        {
            var lines = code.Split('\n');
            var exec = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("push "))
                {
                    var pushargs = line.Replace("push ", "").Split(' ');
                    exec += "00 " + pushargs[0].Replace("arg", "") + line.Replace("push " + pushargs[0], "") + "\n";
                }
                else if (line.StartsWith("pop "))
                {
                    var pushargs = line.Replace("pop ", "").Split(' ');
                    exec += "01 " + line.Replace("pop arg", "") + "\n";
                }
                else if (line.StartsWith("int "))
                {
                    switch (line.Replace("int ", ""))
                    {
                        case "0":
                            exec += "02 000\n";
                            break;
                        case "1":
                            exec += "02 001\n";
                            break;
                        case "2":
                            exec += "02 002\n";
                            break;
                        case "3":
                            exec += "02 003\n";
                            break;
                        case "4":
                            exec += "02 004\n";
                            break;
                        case "5":
                            exec += "02 005\n";
                            break;
                        case "6":
                            exec += "02 006\n";
                            break;
                        case "7":
                            exec += "02 007\n";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid interrupt mentioned.");
                            return "ERROR!";
                    }
                } else if (line.StartsWith("add "))
                {
                    var to = "1";
                    switch (line.Split(' ')[1])
                    {
                        case "arg1":
                            to = "1";
                            break;
                        case "arg2":
                            to = "2";
                            break;
                        case "arg3":
                            to = "3";
                            break;
                        case "arg4":
                            to = "4";
                            break;
                        case "arg5":
                            to = "5";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid add arg mentioned.");
                            return "ERROR!";
                    }
                    switch (line.Split(' ')[2])
                    {
                        case "arg1":
                            exec += "03 " + to + " 1\n";
                            break;
                        case "arg2":
                            exec += "03 " + to + " 2\n";
                            break;
                        case "arg3":
                            exec += "03 " + to + " 3\n";
                            break;
                        case "arg4":
                            exec += "03 " + to + " 4\n";
                            break;
                        case "arg5":
                            exec += "03 " + to + " 5\n";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid add to mentioned.");
                            return "ERROR!";
                    }
                } else if (line.StartsWith("sub "))
                {
                    var to = "1";
                    switch (line.Split(' ')[1])
                    {
                        case "arg1":
                            to = "1";
                            break;
                        case "arg2":
                            to = "2";
                            break;
                        case "arg3":
                            to = "3";
                            break;
                        case "arg4":
                            to = "4";
                            break;
                        case "arg5":
                            to = "5";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid subtract arg mentioned.");
                            return "ERROR!";
                    }
                    switch (line.Split(' ')[2])
                    {
                        case "arg1":
                            exec += "04 " + to + " 1\n";
                            break;
                        case "arg2":
                            exec += "04 " + to + " 2\n";
                            break;
                        case "arg3":
                            exec += "04 " + to + " 3\n";
                            break;
                        case "arg4":
                            exec += "04 " + to + " 4\n";
                            break;
                        case "arg5":
                            exec += "04 " + to + " 5\n";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid subtract to mentioned.");
                            return "ERROR!";
                    }
                }
                else if (line.StartsWith("mul "))
                {
                    var to = "1";
                    switch (line.Split(' ')[1])
                    {
                        case "arg1":
                            to = "1";
                            break;
                        case "arg2":
                            to = "2";
                            break;
                        case "arg3":
                            to = "3";
                            break;
                        case "arg4":
                            to = "4";
                            break;
                        case "arg5":
                            to = "5";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid multiplication arg mentioned.");
                            return "ERROR!";
                    }
                    switch (line.Split(' ')[2])
                    {
                        case "arg1":
                            exec += "05 " + to + " 1\n";
                            break;
                        case "arg2":
                            exec += "05 " + to + " 2\n";
                            break;
                        case "arg3":
                            exec += "05 " + to + " 3\n";
                            break;
                        case "arg4":
                            exec += "05 " + to + " 4\n";
                            break;
                        case "arg5":
                            exec += "05 " + to + " 5\n";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid multiplication to mentioned.");
                            return "ERROR!";
                    }
                }
                else if (line.StartsWith("div "))
                {
                    var to = "1";
                    switch (line.Split(' ')[1])
                    {
                        case "arg1":
                            to = "1";
                            break;
                        case "arg2":
                            to = "2";
                            break;
                        case "arg3":
                            to = "3";
                            break;
                        case "arg4":
                            to = "4";
                            break;
                        case "arg5":
                            to = "5";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid divide arg mentioned.");
                            return "ERROR!";
                    }
                    switch (line.Split(' ')[2])
                    {
                        case "arg1":
                            exec += "06 " + to + " 1\n";
                            break;
                        case "arg2":
                            exec += "06 " + to + " 2\n";
                            break;
                        case "arg3":
                            exec += "06 " + to + " 3\n";
                            break;
                        case "arg4":
                            exec += "06 " + to + " 4\n";
                            break;
                        case "arg5":
                            exec += "06 " + to + " 5\n";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR! ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid divide to mentioned.");
                            return "ERROR!";
                    }
                }
            }
            return exec;
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
        }
    }
}
