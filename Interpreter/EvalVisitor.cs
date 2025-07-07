using System;
using System.Collections.Generic;

public class EvalVisitor : pascalBaseVisitor<object>
{
    private readonly Dictionary<string, object> memory = new();

    public override object VisitVariableDeclaration(pascalParser.VariableDeclarationContext context)
    {
        foreach (var id in context.identifierList().identifier())
        {
            var name = id.GetText();
            if (!memory.ContainsKey(name))
                memory[name] = 0;
        }
        return null;
    }

    public override object VisitAssignmentStatement(pascalParser.AssignmentStatementContext context)
    {
        var varName = context.variable().GetText();
        var value = Visit(context.expression());
        memory[varName] = value;
        return null;
    }
    public override object VisitIfStatement(pascalParser.IfStatementContext context)
    {
        var condition = Visit(context.expression());

        bool result = Convert.ToBoolean(condition);

        if (result)
        {
            Visit(context.statement(0)); // then-ветка
        }
        else if (context.statement().Length > 1)
        {
            Visit(context.statement(1)); // else-ветка (если есть)
        }

        return null;
    }
    public override object VisitExpression(pascalParser.ExpressionContext context)
    {
        if (context.relationaloperator() != null)
        {
            var left = (int)Visit(context.simpleExpression());
            var right = (int)Visit(context.expression());
            var op = context.relationaloperator().GetText();

            return op switch
            {
                "=" => left == right,
                "<" => left < right,
                ">" => left > right,
                "<=" => left <= right,
                ">=" => left >= right,
                "<>" => left != right,
                _ => throw new Exception($"Unknown operator: {op}")
            };
        }
        else
        {
            return Visit(context.simpleExpression());
        }
    }
    public override object VisitWhileStatement(pascalParser.WhileStatementContext context)
    {
        while (Convert.ToBoolean(Visit(context.expression())))
        {
            Visit(context.statement());
        }
        return null;
    }
    public override object VisitSimpleExpression(pascalParser.SimpleExpressionContext context)
    {
        var leftVal = Visit(context.term());

        if (leftVal is string) return leftVal;

        int left = Convert.ToInt32(leftVal);

        if (context.additiveoperator() != null && context.simpleExpression() != null)
        {
            var rightVal = Visit(context.simpleExpression());

            if (rightVal is string) return rightVal;

            int right = Convert.ToInt32(rightVal);
            var op = context.additiveoperator().GetText();

            return op switch
            {
                "+" => left + right,
                "-" => left - right,
                "or" => (left != 0 || right != 0) ? 1 : 0,
                _ => throw new Exception($"Unknown additive operator: {op}")
            };
        }

        return left;
    }

    public override object VisitTerm(pascalParser.TermContext context)
    {
        var leftVal = Visit(context.signedFactor());

        // Если строка — возвращаем как есть (ничего не умножаем)
        if (leftVal is string) return leftVal;

        int left = Convert.ToInt32(leftVal);

        if (context.multiplicativeoperator() != null && context.term() != null)
        {
            var rightVal = Visit(context.term());

            if (rightVal is string) return rightVal; // защита

            int right = Convert.ToInt32(rightVal);
            var op = context.multiplicativeoperator().GetText();

            return op switch
            {
                "*" => left * right,
                "/" => left / right,
                "div" => left / right,
                "mod" => left % right,
                _ => throw new Exception($"Unknown mul operator: {op}")
            };
        }

        return left;
    }


    public override object VisitSignedFactor(pascalParser.SignedFactorContext context)
    {
        var value = Visit(context.factor());

        // Если это строка — просто возвращаем
        if (value is string s)
            return s;

        // Преобразуем к числу
        int number = Convert.ToInt32(value);

        if (context.PLUS() != null)
            return +number;
        else if (context.MINUS() != null)
            return -number;

        return number;
    }


    public override object VisitFactor(pascalParser.FactorContext context)
    {
        var text = context.GetText();

        // если это число
        if (int.TryParse(text, out int number))
            return number;

        // если это строка в одинарных кавычках
        if (text.StartsWith("'") && text.EndsWith("'"))
            return text.Trim('\'');

        // если это скобочное выражение
        if (context.expression() != null)
            return Visit(context.expression());

        // если это переменная
        if (memory.TryGetValue(text, out var value))
            return value;

        return 0; // по умолчанию
    }



    public override object VisitProcedureStatement(pascalParser.ProcedureStatementContext context)
    {
        var text = context.GetText().ToLower();

        if (text.StartsWith("write"))
        {
            var expr = context.parameterList()?.actualParameter()?.FirstOrDefault()?.expression();
            if (expr != null)
            {
                var value = Visit(expr);
                Console.WriteLine(value);
            }
        }

        else if (text.StartsWith("read"))
        {
            var param = context.parameterList()?.actualParameter()?.FirstOrDefault();
            if (param != null)
            {
                var varName = param.GetText(); // имя переменной
                Console.Write($"Введите значение для {varName}: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                {
                    memory[varName] = value;
                }
                else
                {
                    Console.WriteLine("Ошибка: введите целое число.");
                    memory[varName] = 0; // или выдать исключение
                }
            }
        }
        return null;
    }
}
