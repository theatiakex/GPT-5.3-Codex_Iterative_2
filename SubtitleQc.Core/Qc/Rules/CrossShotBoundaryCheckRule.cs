using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class CrossShotBoundaryCheckRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;

    public CrossShotBoundaryCheckRule()
        : this(new EmptyShotChangeProvider())
    {
    }

    public CrossShotBoundaryCheckRule(IShotChangeProvider shotChangeProvider)
    {
        _shotChangeProvider = shotChangeProvider;
    }

    public string Name => "CrossShotBoundaryCheck";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        IReadOnlyList<TimeSpan> cuts = ResolveCuts(context);
        bool crossesCut = cuts.Any(cut => cut > cue.Start && cut < cue.End);
        if (!crossesCut)
        {
            return Array.Empty<QcIssue>();
        }

        return new[] { new QcIssue(Name, "Cue spans over a shot change boundary.") };
    }

    private IReadOnlyList<TimeSpan> ResolveCuts(QcContext context)
    {
        IReadOnlyList<TimeSpan> providedCuts = _shotChangeProvider.GetShotChangeTimestamps();
        if (providedCuts.Count > 0)
        {
            return providedCuts;
        }

        return context.ExternalData?.ShotChangeTimestamps ?? Array.Empty<TimeSpan>();
    }
}
