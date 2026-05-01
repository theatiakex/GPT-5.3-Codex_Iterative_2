using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc;

public sealed class RuleEngine
{
    private readonly IReadOnlyList<IQcRule> _rules;

    public RuleEngine(IEnumerable<IQcRule> rules)
    {
        _rules = rules?.ToArray() ?? Array.Empty<IQcRule>();
    }

    public QcReport Evaluate(IEnumerable<Cue> cues)
    {
        return Evaluate(cues, externalData: null);
    }

    public QcReport Evaluate(IEnumerable<Cue> cues, ExternalQcData? externalData)
    {
        IReadOnlyList<Cue> cueList = cues?.ToArray() ?? Array.Empty<Cue>();
        var context = new QcContext(cueList, externalData);
        var results = cueList.Select(cue => EvaluateCue(cue, context)).ToArray();
        return new QcReport(results);
    }

    private QcResult EvaluateCue(Cue cue, QcContext context)
    {
        var issues = _rules.SelectMany(rule => rule.Evaluate(cue, context)).ToArray();
        QcStatus status = issues.Length == 0 ? QcStatus.Passed : QcStatus.Failed;
        return new QcResult(cue.Id, status, issues);
    }
}
