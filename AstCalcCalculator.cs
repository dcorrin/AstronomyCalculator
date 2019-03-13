using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AstronomyCalculator
{
    public partial class astCalcForm : Form
    {
        /// <summary>
        /// This part deals with the calculator.
        ///    calculation has the history of the computation without the current number attached to it.
        ///    currentNumber is the main part of the display. If not a result then values will add to it
        ///              pressing an operation button (unary or not) will convert this to a (partial) result
        ///    intermediate is the value of the current calculation including the current number. If currentNumber is 0  this is displayed
        ///    result is a flag to indicate that the current number is done for entry
        ///    restart is a flag to indicate we are starting a calculation
        ///    e.g. entering the keys 1,4,+,5,x2,+,1,+/-,0,*,2,= will perform the following
        ///         (if an operator was used, and as start is set 0 would be the current number)
        ///         1 -> currentNumber changes from 0 to 1 (no leading zeros), intermediate still 0 as calculation is empty
        ///         4 -> currentNumber changes from 1 to 14, intermediate still 0 as calculation is empty
        ///         + -> 14(currentNumber) and + added to calculation, currentNumber set to 0, 
        ///                 intermediate is 14 (14 + currentNumber->0)
        ///         5 -> currentNumber changes from 0 to 5, intermediate is 19 (14+5)
        ///         x2 -> sqr(5) added to calculation, currentNumber changed to 25 and result set (number input will do nothing?)
        ///                 intermediate is 39 (14+sqr(5))
        ///         + -> + added to calculation (not currentNumber as is result), currentNumber changed to 0  (displaed as 39), 
        ///                 intermeidate is 39 (14+sqr(5)+0), result is cleared
        ///         1 -> currentNumber changes from x to 1, intermediate is 40 (14+sqr(5)+1)
        ///         +/- -> currentNumber changes from 1 to -1, intermediate is 38 (14+sqr(5)+(-1))
        ///         0 -> currentNumber changes from -1 to -10, intermediate is 29, (14+sqr(5)+(-10))
        ///         * -> -10(current number) and * added to calculation, currentNumber set to 0, intermediate is 29 ((14+sqr(5)+(-10)x0)
        ///         2 -> current number changes from 0 to 2, intermediate is 19, (14+sqr(5)+(-10)*2)
        ///         = -> 2 added to calculation, intermediate still 19, currentNumber is set to 0 (displayed as 19) sent to history, 
        ///                 result is not set, but start is
        ///         The next input will start a new number unless it is an operator
        ///         operator (e.g. + or sqrt) will use the intermediate as restart is set.
        ///         when () and order of operations are set then only intermediate calculations need to change.
        /// </summary>
        private Dictionary<string, string> constList = null; // can't cast string?
        private Dictionary<int, string> valueHistory = null;
        private string calculation = "";
        private string currentNumber = "";
        private string intermediate = "";
        private int parenCnt = 0;
        private bool result;
        private bool restart;

        /// <summary>
        /// Called when we enter the tab - allocate the dictionaries if not done so already and read in the resource constants.txt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCalc_Enter(object sender, EventArgs e)
        {
            if (null == constList)
            {
                constList = new Dictionary<string, string>();
                loadConstants();
            }
            if (null == valueHistory)
            {
                valueHistory = new Dictionary<int, string>();
            }
            calculation = "Compute:";
            currentNumber = "0";
            result = false;
            restart = false;
            display();
        }

        private void loadConstants()
        {
            Assembly assembly = typeof(astCalcForm).Assembly;

            StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("AstronomyCalculator.Resources.Constants.txt"));
            string fileText = sr.ReadToEnd();
            string[] lines = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] nvp = line.Split(new char[] { ' ', '\t' }, 2);
                constList.Add(nvp[0], nvp[1]);
            }
            string[] cNames = new string[constList.Keys.Count];
            constList.Keys.CopyTo(cNames, 0);
            listConst.Items.AddRange((object[])cNames);
        }

        private string doMath(string term1, string op, string term2)
        {
            double t1 = double.Parse(term1);
            double t2 = double.Parse(term2);
            switch (op)
            {
                case "+":
                    return (t1 + t2).ToString();
                case "-":
                    return (t1 - t2).ToString();
                case "*":
                    return (t1 * t2).ToString();
                case "/":
                    return (t1 / t2).ToString();
            }
            return "0";
        }

        private string doMath(string op, string term)
        {
            double t1 = double.Parse(term);
            switch (op)
            {
                case "sqr":
                    return Math.Pow(t1, 2).ToString();
                case "sqrt":
                    return Math.Sqrt(t1).ToString();
                case "log":
                    return Math.Log10(t1).ToString();
                case "10x":
                    return Math.Pow(10, t1).ToString();
                case "ln":
                    return Math.Log(t1).ToString();
                case "ex":
                    return Math.Exp(t1).ToString();
                case "1/x":
                    return (1 / t1).ToString();
                case "sin":
                    return Math.Sin(t1).ToString();
                case "cos":
                    return Math.Cos(t1).ToString();
                case "tan":
                    return Math.Tan(t1).ToString();
                case "asin":
                    return Math.Asin(t1).ToString();
                case "acos":
                    return Math.Acos(t1).ToString();
                case "atan":
                    return Math.Atan(t1).ToString();
            }
            return "0";
        }

        private bool binaryOp(string op)
        {
            return (doMath("2", op, "1") != "0");
        }

        private static char getContext(string token)
        {
            switch (token)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    return '2';
                case "sqr":
                case "sqrt":
                case "log":
                case "10x":
                case "ln":
                case "ex":
                case "1/x":
                case "sin":
                case "cos":
                case "tan":
                case "asin":
                case "acos":
                case "atan":
                    return '1';
                case "(":
                    return '(';
                case ")":
                    return ')';
                case "Null":
                    return '0';
                default:
                    return 'N';
            }
        }

        private struct Token
        {
            public string value;
            public char context;

            public Token(string p1, char p2)
            {
                value = p1;
                context = p2;
            }

            public Token(string p1)
            {
                value = p1;
                context = getContext(p1);
            }

            public override string ToString() => "[" + value + "," + context + "]";
        }

        private static int parenCounter(string s)
        {
            int open = s.Split('(').Length - 1;
            int close = s.Split(')').Length - 1;
            return open - close;
        }

        private static string buildEquation(List<Token> tokens)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Token t in tokens)
            {
                sb.Append(t.value).Append(" ");
            }
            return sb.ToString();
        }

        private string evaluate(string calc)
        {
            // Use CurrentNumber if missing last value
            string[] elements = calc.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            List<Token> tokens = new List<Token>(elements.Length + 1);
            string last = "Null";
            foreach (string s in elements)
            {
                tokens.Add(new Token(s));
                last = s;
            }
            if (getContext(last) != 'N' && getContext(last) != ')')
            {
                tokens.Add(new Token(currentNumber));
            }
            int pcnt = parenCounter(calc);
            while (pcnt > 0)
            {
                tokens.Add(new Token(")"));
                pcnt--;
            }

            //Console.WriteLine("Pre: tokens(" + tokens.Count + "): {0}", string.Join(",", tokens));
            while (tokens.Count > 1)
            {
                bool match = false; // have to have 1 match per loop
                //Console.WriteLine("tokens = {0}" + string.Join(",", tokens));
                // First process parenthesis
                string currentLine = buildEquation(tokens);
                if (currentLine.Contains("("))
                {
                    Match regMatch = Regex.Match(currentLine, @"\(([^)]+)\)");
                    if (regMatch.Success)
                    {
                        string inner = regMatch.Groups[1].Value;
                        string innverValue = evaluate(inner);
                        // need token range
                        int firstP = 0;
                        int lastP = 0;
                        for (int i = 0; i < tokens.Count; i++)
                        {
                            if (tokens[i].context == ')')
                            {
                                lastP = i;
                                break;
                            }
                        }
                        for (int i = lastP; i >= 0; i--)
                        {
                            if (tokens[i].context == '(')
                            {
                                firstP = i;
                                break;
                            }
                        }
                        tokens[firstP] = new Token(innverValue);
                        for (int i = lastP; i > firstP; i--)
                        {
                            tokens.RemoveAt(i);
                        }
                        match = true;
                    }
                }
                // Next look for a unary operator right to left... sqr,sqrt,4 ->sqr,2
                for (int i = tokens.Count - 1; i >= 0; i--)
                {
                    if (tokens[i].context == '1')
                    {
                        string unary = doMath(tokens[i].value, tokens[i + 1].value);
                        tokens.RemoveAt(i + 1);
                        tokens[i] = new Token(unary);
                        match = true;
                    }
                }
                // Finally process any binary operators mult and division first
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i].context == '2' && (tokens[i].value == "*" || tokens[i].value == "/"))
                    {
                        string binary = doMath(tokens[i - 1].value, tokens[i].value, tokens[i + 1].value);
                        tokens.RemoveAt(i + 1);
                        tokens[i] = new Token(binary);
                        tokens.RemoveAt(i - 1);
                        match = true;
                    }
                }
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i].context == '2')
                    {
                        string binary = doMath(tokens[i - 1].value, tokens[i].value, tokens[i + 1].value);
                        tokens.RemoveAt(i + 1);
                        tokens[i] = new Token(binary);
                        tokens.RemoveAt(i - 1);
                        match = true;
                    }
                }
                if (!match)
                {
                    restart = true;
                    return "NaN";
                }
            }
            return tokens[0].value;
        }

        ///         = -> 2 added to calculation, intermediate still 19, currentNumber is set to 0 (displayed as 19) sent to history, 
        ///                 result is not set, but start is
        private void applyPostOperator(string op)
        {
            if (!result && !restart)
            { calculation += currentNumber; }
            if (restart)
            { calculation += intermediate; restart = false; }
            if (op == "=")
            {
                result = true;
                display();
                result = false;
                currentNumber = intermediate;
                restart = true;
                listCalcPrevious.Items.Add(currentNumber);
                if (listCalcPrevious.Items.Count > 10)
                {
                    listCalcPrevious.Items.RemoveAt(0);
                }
                calculation = "Compute:";
                currentNumber = "0";
                return;
            }
            calculation += " " + op + " ";
            currentNumber = "0";
            result = true;
            display();
            result = false;
        }

        private void applyPreOperator(string op)
        {
            calculation += op + " ";
            calculation += currentNumber;
            currentNumber = evaluate(op + " " + currentNumber);
            display();
            result = true;
        }

        private void display()
        {
            intermediate = evaluate(calculation.Substring(8));
            rtDisplay.Text = calculation + Environment.NewLine;
            int bigLetters = 0;
            if (result)
            {
                rtDisplay.AppendText(intermediate + Environment.NewLine);
                bigLetters = intermediate.Length;
            } else
            {
                rtDisplay.AppendText(currentNumber + Environment.NewLine);
                bigLetters = currentNumber.Length;
            }
            rtDisplay.AppendText("= " + intermediate);

            rtDisplay.SelectionStart = calculation.Length;
            rtDisplay.SelectionLength = bigLetters + 1;
            System.Drawing.Font currentFont = rtDisplay.SelectionFont;

            rtDisplay.SelectionFont = new Font(
               currentFont.FontFamily,
               currentFont.Size + 14,
               FontStyle.Bold
            );
            rtDisplay.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void addNumber(string num)
        {
            if (result) // Can't add numbers to result
            { return; }
            restart = false;
            if (currentNumber == "0")
            {
                currentNumber = num;
            } else
            {
                currentNumber += num;
            }
            display();
        }

        private void bAsin_Click(object sender, EventArgs e)
        {
            applyPreOperator("asin");
        }

        private void bSin_Click(object sender, EventArgs e)
        {
            applyPreOperator("sin");
        }

        private void bCos_Click(object sender, EventArgs e)
        {
            applyPreOperator("cos");
        }
        private void bAcos_Click(object sender, EventArgs e)
        {
            applyPreOperator("acos");
        }

        private void bAtan_Click(object sender, EventArgs e)
        {
            applyPreOperator("atan");
        }

        private void bTan_Click(object sender, EventArgs e)
        {
            applyPreOperator("tan");
        }

        private void bLParen_Click(object sender, EventArgs e)
        {
            parenCnt++;
            tParenCnt.Text = parenCnt.ToString();
            calculation += "( ";
            display();
        }

        private void bRParen_Click(object sender, EventArgs e)
        {
            if (parenCnt == 0)
            {
                return; // can't add more close paren
            }
            parenCnt--;
            tParenCnt.Text = parenCnt.ToString();
            calculation += currentNumber;
            calculation += " ) ";
            display();
            result = true;
        }

        private void bConstant_Click(object sender, EventArgs e)
        {
            string constName = listConst.Text;
            if (constName.Length == 0)
            {
                return;
            }
            currentNumber = constList[constName];
            display();
        }

        private void bMC_Click(object sender, EventArgs e)
        {
            tMemory.Text = "0";
        }

        private void bMR_Click(object sender, EventArgs e)
        {
            currentNumber = tMemory.Text;
            display();
        }

        private void bMP_Click(object sender, EventArgs e)
        {
            if (restart)
            {
                tMemory.Text = doMath(tMemory.Text, "+", intermediate);
            } else
            {
                tMemory.Text = doMath(tMemory.Text, "+", currentNumber);
            }
        }

        private void bMM_Click(object sender, EventArgs e)
        {
            if (restart)
            {
                tMemory.Text = doMath(tMemory.Text, "-", intermediate);
            } else
            {
                tMemory.Text = doMath(tMemory.Text, "-", currentNumber);
            }
        }

        private void bMT_Click(object sender, EventArgs e)
        {
            if (restart)
            {
                tMemory.Text = doMath(tMemory.Text, "*", intermediate);
            } else
            {
                tMemory.Text = doMath(tMemory.Text, "*", currentNumber);
            }
        }

        private void bMS_Click(object sender, EventArgs e)
        {
            if (restart)
            {
                tMemory.Text = intermediate;
            } else
            {
                tMemory.Text = currentNumber;
            }
        }

        private void bHistory_Click(object sender, EventArgs e)
        {
            currentNumber = listCalcPrevious.Text;
            if (currentNumber.Length == 0)
            {
                currentNumber = "0";
            }
            display();
        }

        private void bCopyDist_Click(object sender, EventArgs e)
        {
            double inputValue = validateInputValue(tDistValue);
            double multiplier = getValueAsKm(listDistance.Text);
            double distance = inputValue * multiplier;
            currentNumber = (distance * 1000).ToString();
            display();
        }

        private void bCopyMass_Click(object sender, EventArgs e)
        {
            double inputValue = validateInputValue(tMassValue);
            double multiplier = getValueAsKg(listMass.Text);
            double mass = inputValue * multiplier;
            currentNumber = mass.ToString();
            display();
        }

        private void bCopyTime_Click(object sender, EventArgs e)
        {
            double inputValue = validateInputValue(tTimeValue);
            double multipier = getValueAsSecond(listTime.Text);
            double time = inputValue * multipier;
            currentNumber = time.ToString();
            display();
        }

        private void bCopyAngle_Click(object sender, EventArgs e)
        {
            double inputValue = validateInputValue(tAngleValue);
            double multiplier = getValueAsMas(listAngle.Text);
            double angle = inputValue * multiplier;
            if (radioDegrees.Checked)
            {
                currentNumber = (angle / getValueAsMas("Degree")).ToString();
            } else
            {
                currentNumber = (angle / getValueAsMas("Radian")).ToString();
            }
            display();
        }

        private void bClip_Click(object sender, EventArgs e)
        {
            if (restart)
            {
                Clipboard.SetText(intermediate);
            } else
            {
                Clipboard.SetText(currentNumber);
            }
        }

        private void bSquare_Click(object sender, EventArgs e)
        {
            applyPreOperator("sqr");
        }

        private void bSqrt_Click(object sender, EventArgs e)
        {
            applyPreOperator("sqrt");
        }

        private void b10x_Click(object sender, EventArgs e)
        {
            applyPreOperator("10x");
        }

        private void blog_Click(object sender, EventArgs e)
        {
            applyPreOperator("log");
        }

        private void bEx_Click(object sender, EventArgs e)
        {
            applyPreOperator("ex");
        }

        private void bln_Click(object sender, EventArgs e)
        {
            applyPreOperator("ln");
        }

        private void bInverse_Click(object sender, EventArgs e)
        {
            applyPreOperator("1/x");
        }

        private void bPlusMinus_Click(object sender, EventArgs e)
        {
            if (currentNumber == "0")
            {
                currentNumber = "-";
                result = false;
            } else if (currentNumber.EndsWith("e+"))
            {
                currentNumber = currentNumber.Substring(0, currentNumber.Length - 1) + "-";
            } else if (currentNumber.StartsWith("-"))
            {
                currentNumber = currentNumber.Substring(1);
            } else
            {
                currentNumber = "-" + currentNumber;
            }
            display();
        }

        private void bExpand_Click(object sender, EventArgs e)
        {
            double num = double.Parse(currentNumber);
            if (currentNumber.Contains("e"))
            {
                currentNumber = num.ToString("F10");
            } else
            {
                currentNumber = num.ToString("e");
            }
            display();
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            calculation = "Compute:";
            currentNumber = "0";
            result = false;
            parenCnt = 0;
            display();
        }

        private void bCE_Click(object sender, EventArgs e)
        {
            currentNumber = "0";
            display();
        }

        private void bBack_Click(object sender, EventArgs e)
        {
            if (currentNumber.Length == 0 || currentNumber == "0")
            {
                currentNumber = "0";
            } else
            {
                currentNumber = currentNumber.Substring(0, currentNumber.Length - 1);
            }
            display();
        }

        private void bSeven_Click(object sender, EventArgs e)
        {
            addNumber("7");
        }

        private void bEight_Click(object sender, EventArgs e)
        {
            addNumber("8");
        }

        private void bNine_Click(object sender, EventArgs e)
        {
            addNumber("9");

        }

        private void bFour_Click(object sender, EventArgs e)
        {
            addNumber("4");
        }

        private void bFive_Click(object sender, EventArgs e)
        {
            addNumber("5");
        }

        private void bSix_Click(object sender, EventArgs e)
        {
            addNumber("6");
        }

        private void bOne_Click(object sender, EventArgs e)
        {
            addNumber("1");
        }

        private void bTwo_Click(object sender, EventArgs e)
        {
            addNumber("2");
        }

        private void bThree_Click(object sender, EventArgs e)
        {
            addNumber("3");
        }

        private void bExp_Click(object sender, EventArgs e)
        {
            if (currentNumber != "0")
            {
                currentNumber += "e+";
            }
            display();
        }

        private void bZero_Click(object sender, EventArgs e)
        {
            if (currentNumber == "0")
            {
                currentNumber = "0";
            } else
            {
                currentNumber += "0";
            }
            display();

        }

        private void bDot_Click(object sender, EventArgs e)
        {
            if (currentNumber == "0")
            {
                result = false;
            }
            if (!currentNumber.Contains("."))
            {
                currentNumber += ".";
            }
            display();

        }

        private void bDivide_Click(object sender, EventArgs e)
        {
            applyPostOperator("/");
        }

        private void bTimes_Click(object sender, EventArgs e)
        {
            applyPostOperator("*");
        }

        private void bMinus_Click(object sender, EventArgs e)
        {
            applyPostOperator("-");
        }

        private void bPlus_Click(object sender, EventArgs e)
        {
            applyPostOperator("+");
        }

        private void bEnter_Click(object sender, EventArgs e)
        {
            applyPostOperator("=");
        }

    }
}