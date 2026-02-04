using Dapper;
using System.Data;

public class DateOnlyHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        return value switch
        {
            DateTime dt => DateOnly.FromDateTime(dt),
            null => throw new ArgumentNullException(nameof(value)),
            _ => throw new InvalidOperationException($"Cannot convert {value.GetType()} to DateOnly")
        };
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(TimeOnly.MinValue); // 00:00:00
    }
}

public class TimeOnlyHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override TimeOnly Parse(object value)
    {
        return value switch
        {
            DateTime dt => TimeOnly.FromDateTime(dt),
            TimeSpan ts => TimeOnly.FromTimeSpan(ts),
            null => throw new ArgumentNullException(nameof(value)),
            _ => throw new InvalidOperationException($"Cannot convert {value.GetType()} to TimeOnly")
        };
    }

    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.Value = value.ToTimeSpan(); // or value.ToDateTime(DateOnly.MinValue)
    }
}