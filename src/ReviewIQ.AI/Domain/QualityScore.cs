namespace ReviewIQ.AI.Domain;

public class QualityScore
{
    public int Value { get; }

    public QualityScore(int value)
    {
        if (value < 0 || value > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Quality score must be between 0 and 100.");

        Value = value;
    }
}