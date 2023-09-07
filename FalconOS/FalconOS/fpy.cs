using Cosmos.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class fpy
    {
        //The Source Code of FPython
        
        public void genErr(ERRORS.err error, int line, int errCode)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ERRORS.ToStr(error));
        }

        public string exec(string script)
        {
            genErr(ERRORS.err.AttributeError, 1, 10);
            return "No Errors!";
        }

        public struct ERRORS
        {
            public enum err
            {
                AttributeError,
                EOFError,
                FloatingPointError,
                GeneratorExit,
                ImportError,
                IndexError,
                KeyError,
                KeyboardInterrupt,
                MemoryError,
                NameError,
                NotImplementedError,
                OSError,
                OverflowError,
                ReferenceError,
                RuntimeError,
                StopIteration,
                SyntaxError,
                IndentationError,
                TabError,
                SystemError,
                SystemExit,
                TypeError,
                UnboundLocalError,
                UnicodeError,
                ValueError,
                ZeroDivisionError
            }

            public static string ToStr(err from)
            {
                string[] errstr = { "AttributeError", "EOFError", "FloatingPointError", "GeneratorExit", "ImportError", "IndexError",
                 "KeyError", "KeyboardInterrupt", "MemoryError", "NameError", "NotImplementedError", "OSError", "OverflowError", "ReferenceError",
                "RuntimeError", "StopIteration", "SyntaxError", "IndentationError", "TabError", "SystemError", "TypeError", "UnboundLocalError",
                 "UnicodeError", "ValueError", "ZeroDivisionError"};
                if (((int)from) > 0 && ((int)from) < 25)
                {
                    return errstr[((int)from)];
                }
                throw new Exception("Invalid Error Conversion");
            }
        }
    }
}
