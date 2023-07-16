using Cosmos.System.FileSystem.ISO9660;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    class ParserException : ApplicationException
    {
        public ParserException(string str) : base(str) { }

        public override string ToString()
        {
            return Message;
        }
    }

    internal class calc
    {
        enum types { NONE, DELIMITER, VARIABLE, NUMBER };
        enum errors { SYNTAX, UNBALPARENS, NOEXP, DIVBYZERO };

        string exp;
        int expIdx;
        string token;
        types tokType;

        public double Evaluate(string expstr)
        {
            double result;
            exp = expstr;
            expIdx = 0;

            try
            {
                GetToken();
                if (token == "")
                {
                    SyntaxErr(errors.NOEXP);
                    return 0.0;
                }

                EvalExp2(out result);

                if (token != "")
                {
                    SyntaxErr(errors.SYNTAX);
                }
                return result;
            }
            catch (ParserException exc)
            {
                Console.WriteLine(exc);
                return 0.0;
            }
        }

        void EvalExp2(out double result)
        {
            string op;
            double partialResult;

            EvalExp3(out result);
            while ((op = token) == "+" || op == "-")
            {
                GetToken();
                EvalExp3(out partialResult);
                switch (op)
                {
                    case "-":
                        result = result - partialResult; break;
                    case "+":
                        result = result + partialResult; break;
                }
            }
        }

        void EvalExp3(out double result)
        {
            string op;
            double partialResult = 0.0;

            EvalExp4(out result);
            while ((op = token) == "*" || op == "/" || op == "%")
            {
                GetToken();
                EvalExp4(out partialResult);
                switch (op)
                {
                    case "*":
                        result = result * partialResult; break;
                    case "/":
                        if (partialResult == 0.0) { SyntaxErr(errors.DIVBYZERO); }
                        result = result / partialResult; break;
                    case "%":
                        if (partialResult == 0.0)
                        {
                            SyntaxErr(errors.DIVBYZERO);
                        }
                        result = (int)result % (int)partialResult; break;
                }
            }
        }

        void EvalExp4(out double result)
        {
            double partialResult, ex;
            int t;

            EvalExp5(out result);
            if (token == "^")
            {
                GetToken();
                EvalExp4(out partialResult);
                ex = result;
                if (partialResult == 0.0)
                {
                    result = 1.0;
                    return;
                }
                for (t=(int)partialResult-1; t>0; t--) {
                    result = result * (double)ex;
                }
            }
        }

        void EvalExp5(out double result)
        {
            string op;
            op = "";
            if ((tokType == types.DELIMITER) && token == "+" || token == "-") { op = token; GetToken(); }
            EvalExp6(out result);
            if (op == "-") result = -result;
        }

        void EvalExp6(out double result)
        {
            if ((token == "("))
            {
                GetToken();
                EvalExp2(out result);
                if (token != ")")
                {
                    SyntaxErr(errors.UNBALPARENS);
                }
                GetToken();
            }
            else Atom(out result);
        }

        void Atom(out double result)
        {
            switch (tokType)
            {
                case types.NUMBER:
                    try
                    {
                        result = Double.Parse(token);
                    }
                    catch (Exception)
                    {
                        result = 0.0;
                        SyntaxErr(errors.SYNTAX);
                    }
                    GetToken();
                    return;
                default:
                    result = 0.0;
                    SyntaxErr(errors.SYNTAX);
                    break;
            }
        }

        void SyntaxErr(errors error)
        {
            string[] err =
            {
                "Syntax Error",
                "Unbalanced Parantheses",
                "No Expression Present",
                "Division by Zero"
            };

            Console.WriteLine("Invalid Expression: " + err[(int)error]);
        }

        void GetToken()
        {
            tokType = types.NONE;
            token = "";

            if (expIdx == exp.Length)
            {
                return;
            }

            while (expIdx < exp.Length && char.IsWhiteSpace(exp[expIdx])) ++expIdx;

            if (expIdx == exp.Length) return;
            if (IsDelim(exp[expIdx]))
            {
                token += exp[expIdx];
                expIdx++;
                tokType = types.DELIMITER;
            }
            else if (Char.IsLetter(exp[expIdx]))
            {
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length) break;
                }
                tokType = types.VARIABLE;
            }
            else if (Char.IsDigit(exp[expIdx]))
            {
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length) break;
                }
                tokType = types.NUMBER;
            }
        }

        bool IsDelim(char c)
        {
            if((" +-/*%^=()".IndexOf(c) != -1))
            {
                return true;
            }
            return false;
        }
    }
}
