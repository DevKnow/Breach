
public enum LANGUAGE
{
    Ko,
    En,
}

public enum CommandType
{
    None,
    Attack,
    Support,
}

public enum CompareType
{
    Error,          // can not parse with anything
    Equal,          // ==
    NotEqual,       // !=
    Greater,        // >
    GreaterOrEqual, // >=
    Less,           // <
    LessOrEqual,    // <=
}

public enum BuffType
{
    Positive,
    Negative,
    Neutral,
}

public enum DurationTrigger
{
    OnRoundEnd,
    OnRoundStart,
    OnHit,
    OnAttack,
}

public enum BuffKeyword
{
    Error,
    Thorn,
}

public enum HitResultType
{
    Miss,
    Hit,
    Critical,
    Fumble
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare
}