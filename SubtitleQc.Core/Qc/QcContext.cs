using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Qc;

public sealed class QcContext
{
    public QcContext(IReadOnlyList<Cue> cues)
    {
        Cues = cues;
    }

    public IReadOnlyList<Cue> Cues { get; }
}
