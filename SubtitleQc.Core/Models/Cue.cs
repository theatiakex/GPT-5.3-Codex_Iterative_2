namespace SubtitleQc.Core.Models;

public sealed record Cue
{
    public Cue(
        string id,
        TimeSpan start,
        TimeSpan end,
        IReadOnlyList<string> lines,
        int? startFrame = null,
        int? endFrame = null)
    {
        Id = id;
        Start = start;
        End = end;
        Lines = lines;
        StartFrame = startFrame;
        EndFrame = endFrame;
    }

    public string Id { get; }

    public TimeSpan Start { get; }

    public TimeSpan End { get; }

    public IReadOnlyList<string> Lines { get; }

    public int? StartFrame { get; }

    public int? EndFrame { get; }

    public TimeSpan Duration => End - Start;
}
