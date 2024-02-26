using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

public class ExpressionEvaluator
{
    public static double Evaluate(string expression)
    {
        var numbers = new Stack<double>();
        var operators = new Stack<char>();

        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];

            if (char.IsDigit(c))
            {
                double num = c - '0';
                while (i + 1 < expression.Length && char.IsDigit(expression[i + 1]))
                {
                    num = num * 10 + (expression[i + 1] - '0');
                    i++;
                }
                numbers.Push(num);
            }
            else if (c == '(')
            {
                operators.Push(c);
            }
            else if (c == ')')
            {
                while (operators.Count > 0 && operators.Peek() != '(')
                {
                    EvaluateOperator(numbers, operators.Pop());
                }
                operators.Pop();
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                while (operators.Count > 0 && Precedence(c) <= Precedence(operators.Peek()))
                {
                    EvaluateOperator(numbers, operators.Pop());
                }
                operators.Push(c);
            }
        }

        while (operators.Count > 0)
        {
            EvaluateOperator(numbers, operators.Pop());
        }

        return numbers.Pop();
    }

    private static void EvaluateOperator(Stack<double> stack, char op)
    {
        double b = stack.Pop();
        double a = stack.Pop();
        if (op == '+')
            stack.Push(a + b);
        else if (op == '-')
            stack.Push(a - b);
        else if (op == '*')
            stack.Push(a * b);
        else if (op == '/')
            stack.Push(a / b);
    }

    private static int Precedence(char op)
    {
        if (op == '+' || op == '-')
            return 1;
        else if(op == '*' || op == '/')
            return 2;
        return 0;
    }

    public static void Main(string[] args)
    {
        try
        {
            string filename = args[0].ToString();
            string[] lines = File.ReadAllLines(filename);

            for (int i = 1; i < Int32.Parse(lines[0]) + 1; i++) {
                try
                {
                    string expression = lines[i];
                    double result = Evaluate(expression);
                    Console.WriteLine($"{expression} = {result}");
                }
                catch
                {
                    Console.WriteLine("ERROR");
                }
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine(e.ToString());
            Console.WriteLine("ERROR");
        }
    }
}



