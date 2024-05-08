using System;
using System.Runtime.ConstrainedExecution;

public class DollarConverter
{
    private static readonly string DOLLARS = "dollars";
    private static readonly string CENTS = "cents";
    private static readonly string[] SINGLES = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    private static readonly string[] TEENS = { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
    private static readonly string[] TENS = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

    private static string ConvertThreeDigits(int number)
    {
        string result = "";

        int hundreds = number / 100;
        if (hundreds > 0)
        {
            result += SINGLES[hundreds] + " hundred ";
        }

        number %= 100;
        if (number >= 20)
        {
            int tens = number / 10;
            result += TENS[tens - 2];
            number %= 10;
            if (number > 0) // case like 45("fourty-five" with a dash '-')
            {
                result += "-";
            }
            else //case like 80("eighty" with no dash '-')
            {
                result += " ";
            }
            
        }

        if (number >= 10 && number < 20)
        {
            result += TEENS[number - 10] + " ";
        }
        else if (number > 0)
        {
            result += SINGLES[number] + " ";
        }

        return result;
    }
    
    public static string ConvertDecimalToWords(string amount)
    {
        int dollars = 0;
        int cents = 0;
        if(amount.Contains('.')) // cases with decimals
        {
            string[] ints = amount.Split('.');
            dollars = int.Parse(ints[0]);
            if (ints[1].Length == 1) // cases like 25.1, 0.2, 3456.7, etc. (one decimal)
            {
                ints[1] += "0";
            }
            cents = int.Parse(ints[1]);
        }
        else // cases without decimals
        {
            cents = 0;
            dollars = int.Parse(amount);
        }

        if (cents < 0 || cents > 99) 
        {
            throw new ArgumentException("Cent value is out of range");
        }
        else if (dollars < 0 || dollars > 999999999)
        {
            throw new ArgumentException("Dollar value is out of range");
        }
        
        if (dollars == 0 && cents == 0) // cases like 0, 00, 00000, etc.
        {
            return "zero dollars";
        }

        string result = "";

        if (dollars > 0)
        {
            int billions = dollars / 1000000000;
            int millions = (dollars % 1000000000) / 1000000;
            int thousands = (dollars % 1000000) / 1000;
            int remaining = dollars % 1000;

            if (billions > 0)
            {
                result += ConvertThreeDigits(billions) + "billion ";
            }

            if (millions > 0)
            {
                result += ConvertThreeDigits(millions) + "million ";
            }

            if (thousands > 0)
            {
                result += ConvertThreeDigits(thousands) + "thousand ";
            }

            if (remaining > 0)
            {
                result += ConvertThreeDigits(remaining);
            }

            if(dollars == 1) // cases like 1, 01, 000001, etc
            {
                result += "dollar ";
            }
            else
            {
                result += DOLLARS + " ";
            }  
        }

        if (cents > 0 && dollars == 0)
        {
            result += ConvertThreeDigits(cents) + CENTS;
        }
        else if (cents > 0) {
            result += "and " + ConvertThreeDigits(cents) + CENTS;
        }
        result.Trim();
        if (cents == 1) // remove the ending 's' in "cents"
        {
            result = result.Substring(0, result.Length - 1);
        }

        return result;
    }
}