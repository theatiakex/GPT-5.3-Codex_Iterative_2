using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MaxCpsRule : IQcRule
{
    private readonly double _threshold;

    public MaxCpsRule(double threshold)
    {
        _threshold = threshold;
    }

    public string Name => "MaxCps";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        if (cue.Duration <= TimeSpan.Zero)
        {
            return new[] { new QcIssue(Name, "Cue duration must be greater than zero.") };
        }

        double cps = CalculateCps(cue);
        if (cps <= _threshold)
        {
            return Array.Empty<QcIssue>();
        }

        string message = $"Cue speed is {cps:F2} cps; limit is {_threshold:F2}.";
        return new[] { new QcIssue(Name, message) };
    }

    private static double CalculateCps(Cue cue)
    {
        int charCount = cue.Lines.Sum(line => line.Length);
        return charCount / cue.Duration.TotalSeconds;
    }
}
