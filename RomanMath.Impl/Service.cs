using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace RomanMath.Impl
{
	public static class Service
	{
        
        #region PRIVATE_CONSTANTS
        //contains allowed letters
        private static readonly string[] AVAILABLE_NUMERALS =
        {
            "M","D","C","L","X","V","I"
        };

        //contains allowed щзукфещк
        private static readonly string[] AVAILABLE_OPERATORS =
{
            "+","-","*"
        };

        //contstains possible roman numerals
        private static readonly string[] NUMERAL_OPTIONS =
        {
            "M", "CM", "D","CD", "C", "XC", "L",
            "XL", "IIXX", "XIIX", "X", "IX", "V", "IV", "I"
        };

        //contains decimal values for roman numerals
        private static readonly IReadOnlyDictionary<string, int> VALUES =
            new ReadOnlyDictionary<string, int>(new Dictionary<string, int>
        {
            {"I",       1 },
            {"IV",      4 },
            {"V",       5 },
            {"IX",      9 },
            {"X",       10 },
            {"XIIX",    18 },
            {"IIXX",    18 },
            {"XL",      40 },
            {"L",       50 },
            {"XC",      90 },
            {"C",       100 },
            {"CD",      400 },
            {"D",       500 },
            {"CM",      900 },
            {"M",       1000 }
        });


        #endregion
        /// <summary>
        /// See TODO.txt file for task details.
        /// Do not change contracts: input and output arguments, method name and access modifiers
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int Evaluate(string expression)
		{
            #region VALIDATION
            //"expression" argument shouldn't be null or empty
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(expression, "expression cannot be null or empty");
            }

            string strToRead = expression.ToUpper();

            //remove all spaces
            Regex.Replace(strToRead, @"\s+", "");

            //create a list of available objects from arrays
            string operators = string.Join(",", AVAILABLE_OPERATORS);
            string numerals = string.Join(",", AVAILABLE_NUMERALS);

            //create a regex
            string rgx = $@"^[{numerals}]+([{operators}][{numerals}]+)*$";

            //if the expression does not match the rules, throw the corresponding error
            if (Regex.IsMatch(strToRead, rgx) is false)
            {
                throw new ArgumentException(expression,
                    $"'expression' should contain only whitespaces, operators '{operators}' and letters '{numerals}'");
            }

            
            #endregion
            //stores an expression with decimal numbers
            string decimalExpression = "";

            // continue to read until the string is empty
            while (!string.IsNullOrEmpty(strToRead))
			{
                //add the decimal representation of the Roman numeral to the string
                //if the GetRoman method returned null, read the operator
                decimalExpression += GetDecimalFromRoman(ref strToRead) ?? GetOperation(ref strToRead);
			}

            //get the result of the expression using DataTable().Compute()
            string decimalResult = new DataTable().Compute(decimalExpression, null).ToString();

            //round the result and convert it to int
            int result = Convert.ToInt32(Math.Round(Convert.ToDouble(decimalResult)));

			return result;
		}

        #region HELPERS
        /// <summary>
        /// if the string starts with a valid operator, returns the operator and removes it from the original string
        /// </summary>
        /// <param name="source"></param>
        private static string GetOperation(ref string source)
        {
            //start with the first element of the allowed operators
            var availableOperatorPointer = 0;

            //until we iterate over all the operators
            while (availableOperatorPointer < AVAILABLE_OPERATORS.Length)
            {
                //select the current operator
                var operation = AVAILABLE_OPERATORS[availableOperatorPointer];

                //if the line does not start with the current operator
                if (!source.StartsWith(operation))
                {
                    //increase counter and repeat the cycle
                    availableOperatorPointer++;
                    continue;
                }

                //otherwise, remove the operator from the source string and return it
                source = source.Substring(operation.Length);
                return operation;
            }
            return null;
        }

        /// <summary>
        /// if the string starts with a valid number, returns the decimal representation of that number and removes it from the original string
        /// </summary>
        /// <param name="source"></param>
        private static string GetDecimalFromRoman(ref string source)
        {

            var resultNumber = 0;

            //start with the first element of the allowed numerals
            var numeralOptionPointer = 0;

            //until we iterate over all the numerals
            while (numeralOptionPointer < NUMERAL_OPTIONS.Length)
            {
                //select the current numeral
                var numeral = NUMERAL_OPTIONS[numeralOptionPointer];

                //if the line does not start with the current numeral
                if (!source.StartsWith(numeral))
                {
                    //increase counter and repeat the cycle
                    numeralOptionPointer++;
                    continue;
                }

                //otherwise
                //reset the counter
                numeralOptionPointer = 0;

                // get the decimal representation of the numeral
                var value = VALUES[numeral];

                //add it to the resulting value
                resultNumber += value;

                //remove the numeral from the source string
                source = source.Substring(numeral.Length);
            }

            //if the resulting value is not zero
            if (resultNumber.ToString() != "0")
            {
                //return it
                return resultNumber.ToString();
            }
            return null;
        }
        #endregion
    }
}
