using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Qc;

public sealed class QcContext
{
    public QcContext(IReadOnlyList<Cue> cues, ExternalQcData? externalData = null)
    {
        Cues = cues;
        ExternalData = externalData;
    }

    public IReadOnlyList<Cue> Cues { get; }

    public ExternalQcData? ExternalData { get; }
}
