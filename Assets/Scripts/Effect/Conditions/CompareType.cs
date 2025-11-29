public static class CompareHelper
{
    private static readonly string[] _operators = { ">=", "<=", "==", "!=", ">", "<" };

    public static bool Compare(int a, int b, CompareType type)
    {
        return type switch
        {
            CompareType.Equal => a == b,
            CompareType.NotEqual => a != b,
            CompareType.Greater => a > b,
            CompareType.GreaterOrEqual => a >= b,
            CompareType.Less => a < b,
            CompareType.LessOrEqual => a <= b,
            _ => false
        };
    }

    public static int FindOperatorIndex(string input)
    {
        int minIndex = -1;

        for (int i = 0; i < _operators.Length; i++)
        {
            int index = input.IndexOf(_operators[i]);
            if (index >= 0 && (minIndex == -1 || index < minIndex))
            {
                minIndex = index;
            }
        }

        return minIndex;
    }

    public static bool TryParseCondition(string input, out CompareType compareType, out int value)
    {
        compareType = CompareType.Error;
        value = 0;

        for (int i = 0; i < _operators.Length; i++)
        {
            if (!input.StartsWith(_operators[i]))
                continue;

            compareType = ParseOperator(_operators[i]);
            value = int.Parse(input.Substring(_operators[i].Length));
            return true;
        }

        return false;
    }

    private static CompareType ParseOperator(string op)
    {
        return op switch
        {
            "==" => CompareType.Equal,
            "!=" => CompareType.NotEqual,
            ">" => CompareType.Greater,
            ">=" => CompareType.GreaterOrEqual,
            "<" => CompareType.Less,
            "<=" => CompareType.LessOrEqual,
            _ => CompareType.Error
        };
    }
}
