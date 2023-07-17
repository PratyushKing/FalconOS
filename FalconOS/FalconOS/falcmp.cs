using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class falcmp
    {
        public static string compile(string line)
        {
            string binary = "";
            if (!line.StartsWith("//"))
            {
                if (line.StartsWith("$print "))
                {
                    binary += "00 1 " + line.Replace("$print ", "").Replace("\\\\", "\\") + "\n";
                    binary += "00 2 0\n02 000\n01 1\n01 2\n";
                    return binary;
                }
                else if (line.StartsWith("$ask "))
                {
                    var seperator = line.Replace("$ask ", "").Split(' ');
                    var output = "0";
                    if (seperator.Length >= 1)
                    {
                        if (seperator[0] == "arg1")
                        {
                            output = "1";
                        }
                        else if (seperator[0] == "arg2")
                        {
                            output = "2";
                        }
                        else if (seperator[0] == "arg3")
                        {
                            output = "3";
                        }
                        else if (seperator[0] == "arg4")
                        {
                            output = "4";
                        }
                        else if (seperator[0] == "arg5")
                        {
                            output = "5";
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error! Invalid Arg!");
                        }
                        if (output == "0")
                        {
                            return "err";
                        }
                        binary += "00 1 " + output + "\n02 001\n01 1\n";
                    }
                }
            }
            return binary;
        }
    }
}
