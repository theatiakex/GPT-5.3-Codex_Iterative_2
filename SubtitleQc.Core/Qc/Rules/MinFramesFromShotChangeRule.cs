using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MinFramesFromShotChangeRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;
    private readonly int _thresholdFrames;

    public MinFramesFromShotChangeRule(int thresholdFrames)
        : this(new EmptyShotChangeProvider(), thresholdFrames)
    {
    }

    public MinFramesFromShotChangeRule(IShotChangeProvider shotChangeProvider, int thresholdFrames)
    {
        _shotChangeProvider = shotChangeProvider;
        _thresholdFrames = thresholdFrames;
    }

    public string Name => "MinFramesFromShotChange";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        int? cueStartFrame = ResolveCueStartFrame(cue, context.ExternalData);
        IReadOnlyList<int> cutFrames = ResolveCutFrames(context.ExternalData, _shotChangeProvider);
        if (cueStartFrame is null || cutFrames.Count == 0)
        {
            return Array.Empty<QcIssue>();
        }

        int nearestDistance = cutFrames.Min(frame => Math.Abs(cueStartFrame.Value - frame));
        if (nearestDistance >= _thresholdFrames)
        {
            return Array.Empty<QcIssue>();
        }

        string message = $"Cue starts {nearestDistance} frame(s) from nearest cut; minimum is {_thresholdFrames}.";
        return new[] { new QcIssue(Name, message) };
    }

    private static int? ResolveCueStartFrame(Cue cue, ExternalQcData? data)
    {
        if (cue.StartFrame.HasValue)
        {
            return cue.StartFrame.Value;
        }

        if (data?.FrameRate is null || data.FrameRate <= 0)
        {
            return null;
        }

        return (int)Math.Round(cue.Start.TotalSeconds * data.FrameRate.Value);
    }

    private static IReadOnlyList<int> ResolveCutFrames(ExternalQcData? data, IShotChangeProvider provider)
    {
        IReadOnlyList<int> providedFrames = provider.GetShotChangeFrames();
        if (providedFrames.Count > 0)
        {
            return providedFrames;
        }

        if (data is null)
        {
            return Array.Empty<int>();
        }

        if (data.ShotChangeFrames.Count > 0)
        {
            return data.ShotChangeFrames;
        }

        if (data.FrameRate is null || data.FrameRate <= 0)
        {
            return Array.Empty<int>();
        }

        return data.ShotChangeTimestamps
            .Select(ts => (int)Math.Round(ts.TotalSeconds * data.FrameRate.Value))
            .ToArray();
    }
}
