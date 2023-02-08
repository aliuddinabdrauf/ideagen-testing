using System.Globalization;
using System.Runtime.CompilerServices;

namespace ideagen_testing
{
    /// <summary>
    /// 
    /// this is build without refering to any source in internet, except for the do while loop, where I forgot how the syntax 😅
    /// the time taken to build this, cannot be sure the exact amount of time, since I'm not building it at once.
    /// the estimate time taken maybe around 4 hours. including all things, like coding, problem solving, thinking, writing comment, testing, and so on.
    /// 
    /// Please note that numbers, operators and brackets are all separated by spaces.
    /// IMPORTANT: THIS APP EXPECT THE INPUT TO BE IN CORRECT FORMAT AS PER STATED IN THE SPECIFICATION. ANY WRONG FORMAT, LIKE DOUBLE OR NO SPACING,
    /// MAY TRINGGER ERROR, OR WILL RESULTING WRONG ANSWER.
    /// </summary>
    internal static class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Enter your mathematical operation");
                string toCalculate = Console.ReadLine().TrimStart().TrimEnd();
                Console.WriteLine( $"the result is: {Calculate(toCalculate)}");
                Console.WriteLine("Enter any key to continue using, or enter x to quit");
            }
            while (Console.ReadLine() != "x");
        }


        /// <summary>
        /// this method will take a string mathematical statement, and will calculate it
        /// the stratergy is by checking the first operation that need to be done, then the result of the operation,
        /// will replacing the operation statement in the string. for example:
        /// the statement is 1 + 2 * ( 2 + 3 ),
        /// so for the first round, it will solve the 2 + 3, then replace the result, so the string will be 1 + 2 * 5.
        /// then it will call back the method to solve the remaining operation. so nesting method call is using.
        /// </summary>
        /// <param name="sum">string that will be calculated</param>
        /// <returns></returns>

        static double Calculate(string statement)
        {
            //find brackets index
            int lastBracketIndex = statement.LastIndexOf('(');
            if (lastBracketIndex < 0)
            {
                //if no bracket found, then proceed with multiply and division, whoever first
                int firstIndexOfMorD = statement.IndexOfAny(new char[] { '/', '*' });
                if(firstIndexOfMorD < 0)
                {
                    //if not found, then proceed from first to last operation in the statement
                    int firstOperation = statement.IndexOfAnyWithLeadingSpaces(new char[] { '+', '-' });
                    if(firstOperation < 0)
                    {
                        //if no operation found, means, there are no operation need to be made, then just return as it is
                        return Convert.ToDouble(statement);
                    }
                    else
                    {
                        //found a add or substract operation
                        //get the start and end index of the operation
                        int startIndex = 0; //start index will always zero for this case, since it will take the earliest operation
                        var indexThirdSpaceSub = statement.Substring(firstOperation + 2).IndexOf(' ');
                        int indexOfThirdSpace = (indexThirdSpaceSub < 0) ? indexThirdSpaceSub : indexThirdSpaceSub + firstOperation + 2;
                        if (indexOfThirdSpace < 0)//will be greater than 0 if other operation detected by using spaces
                        {
                            //this if only one operation found
                            var calculated = OnlyOneOperation(statement);
                            return calculated;
                        }
                        else
                        {
                            var toCalculate = statement.Substring(startIndex, indexOfThirdSpace);
                            var calculated = OnlyOneOperation(toCalculate);
                            //after got result, replace the calculated operation in the string with the calculated result
                            var newStatement = calculated + statement.Remove(startIndex, indexOfThirdSpace);
                            //then run again the function for next operation
                            return Calculate(newStatement);
                        }
                    }
                }
                else
                {
                    //get the start index of number before the first operation
                    var gs = statement.Substring(0, firstIndexOfMorD - 1);
                    var LastSpaceIndex = gs.LastIndexOf(' ');
                    int numberStartIndex = LastSpaceIndex + 1;
                    //check if there's spaces after the operation
                    var indexThirdSpaceSub = statement.Substring(firstIndexOfMorD + 2).IndexOf(' ');
                    int indexOfThirdSpace = (indexThirdSpaceSub < 0) ? indexThirdSpaceSub : indexThirdSpaceSub + firstIndexOfMorD + 2;

                    if(indexOfThirdSpace < 0)
                    {
                        //this means this is the last operation in the statement or only operation in the statement
                        var toCalculate = statement.Substring(numberStartIndex);
                        var calculated = OnlyOneOperation(toCalculate);
                        //replace the the operation that have been calculated, with the calculated value
                        var newStatement = statement.Remove(numberStartIndex, statement.Length - numberStartIndex).Insert(numberStartIndex, calculated + "");
                        //run the function again
                        return Calculate(newStatement);
                    }
                    else
                    {
                        var toCalculate = statement.Substring(numberStartIndex, indexOfThirdSpace - numberStartIndex);
                        var calculated = OnlyOneOperation(toCalculate);
                        //replace the the operation that have been calculated, with the calculated value
                        var newStatement = statement.Remove(numberStartIndex,  indexOfThirdSpace - numberStartIndex).Insert(numberStartIndex, calculated + "");
                        return Calculate(newStatement);
                    }

                }
            }
            else
            {
                //get the index of the bracket closure
                int lastBracketClosureIndex = statement.Substring(lastBracketIndex).IndexOf(')') + lastBracketIndex;
                //get all the operation inside the bracket, and remove the leading and ending space
                var bracketOperation = statement.Substring(lastBracketIndex + 1, lastBracketClosureIndex - lastBracketIndex - 1);
                bracketOperation = bracketOperation.TrimEnd().TrimStart();
                //call the method back
                double bracketResult = Calculate(bracketOperation);
                //now replace the bracket with the result
                var newStatement = statement.Remove(lastBracketIndex, lastBracketClosureIndex - lastBracketIndex + 1).Insert(lastBracketIndex, bracketResult+"");
                return Calculate(newStatement);
            }
            //return result;
        }
        /// <summary>
        /// this method will calculate based on the string given, but only limited to one operation
        /// </summary>
        /// <param name="operationStr"></param>
        /// <returns></returns>
        static double OnlyOneOperation(string operationStr)
        {
            double result = 0;
            var valueAndOperation = operationStr.Split(' ');
            if (valueAndOperation.Length == 3)
            {
                var operation = valueAndOperation[1];
                var firstNumber = Convert.ToDouble(valueAndOperation[0]);
                var secondNumber = Convert.ToDouble(valueAndOperation[2]);
                if(operation == "+")
                {
                    result = firstNumber + secondNumber;
                }
                else if(operation == "-")
                {
                    result = firstNumber - secondNumber;
                }
                else if (operation == "*")
                {
                    result = firstNumber * secondNumber;
                }
                else
                {
                    result= firstNumber / secondNumber;
                }
            }
            return result;
        }
        /// <summary>
        /// this created, since + and - can be exist without spacing with the number, that can be means positive and negative integer
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toSearch"></param>
        /// <returns></returns>
        static int IndexOfAnyWithLeadingSpaces(this string text, char [] toSearch)
        {
            var charArr = text.ToCharArray();
            
            for (int i=0; i< charArr.Length; ++i)
            {
                for (int j=0; j < toSearch.Length; ++j)
                {
                    if (charArr[i] == toSearch[j])
                    {
                        if(i>0 && charArr[i-1]==' ')
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
    }
}