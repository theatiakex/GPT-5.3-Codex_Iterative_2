using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc;

public sealed class EmptyShotChangeProvider : IShotChangeProvider
{
    public IReadOnlyList<TimeSpan> GetShotChangeTimestamps()
    {
        return Array.Empty<TimeSpan>();
    }

    public IReadOnlyList<int> GetShotChangeFrames()
    {
        return Array.Empty<int>();
    }
}
