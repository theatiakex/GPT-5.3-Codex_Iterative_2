### Iteration 1

Implemented a working Iteration 1 prototype from agents_iterative.md + iteration_1.md with a clean core module and passing tests.

What I built
Added new core library project: SubtitleQc.Core/SubtitleQc.Core.csproj
Added unified internal subtitle model:
SubtitleQc.Core/Models/Cue.cs
SubtitleQc.Core/Models/SubtitleDocument.cs
Added QC engine contracts/results (JSON-serializable records/classes):
SubtitleQc.Core/Qc/QcStatus.cs
SubtitleQc.Core/Qc/QcIssue.cs
SubtitleQc.Core/Qc/QcResult.cs
SubtitleQc.Core/Qc/QcReport.cs
SubtitleQc.Core/Qc/QcContext.cs
SubtitleQc.Core/Qc/Abstractions/IQcRule.cs
SubtitleQc.Core/Qc/RuleEngine.cs
Implemented required Iteration 1 rules:
SubtitleQc.Core/Qc/Rules/MaxLinesRule.cs
SubtitleQc.Core/Qc/Rules/MaxCplRule.cs
SubtitleQc.Core/Qc/Rules/MaxCpsRule.cs
SubtitleQc.Core/Qc/Rules/MinDurationRule.cs
SubtitleQc.Core/Qc/Rules/OverlapCheckRule.cs
SubtitleQc.Core/Qc/Rules/EmptyCueCheckRule.cs
Implemented isolated parsers for required formats:
SubtitleQc.Core/Parsing/Abstractions/ISubtitleParser.cs
SubtitleQc.Core/Parsing/SrtParser.cs
SubtitleQc.Core/Parsing/WebVttParser.cs
SubtitleQc.Core/Parsing/TimestampParser.cs
SubtitleQc.Core/Parsing/TimingLineParser.cs
Wired tests to the core project by updating:
SubtitleQc.Tests/SubtitleQc.Tests.csproj
Validation
Ran dotnet test and fixed compile contract mismatch (Cue ctor named parameters).
Final result: 13/13 tests passed.
Logged all test attempts to test_metrics.jsonl as required:
attempt 1: restore failure
attempt 2: compile failure
attempt 3: success (100% pass rate)
Notes
I had to add explicit package versions in SubtitleQc.Tests/SubtitleQc.Tests.csproj because the existing file had versionless PackageReference entries and restore failed otherwise.
Running tests generated SubtitleQc.Tests/bin and SubtitleQc.Tests/obj build artifacts.


### Iteration 2

Extended the prototype for Iteration 2 by adding TTML parsing while leaving the QC engine and existing rules unchanged.

What I added
New parser: SubtitleQc.Core/Parsing/TtmlParser.cs
Implements ISubtitleParser
Parses TTML XML (<p> elements) into the existing internal Cue model
Reads begin and end attributes for timing
Supports multi-line cue text via <br/>
Produces SubtitleDocument so existing RuleEngine can validate TTML cues with no rule changes
What I preserved
No changes to:
RuleEngine
Existing rule classes (MaxLines, MaxCpl, MaxCps, MinDuration, OverlapCheck, EmptyCueCheck)
Iteration 1 behavior remains intact.
Verification
Ran tests: dotnet test SubtitleQc.Tests/SubtitleQc.Tests.csproj
Result: Passed 13/13
Logged Iteration 2 telemetry in test_metrics.jsonl:
Added {"project_iteration":2,"attempt":1,...,"passed":13,"failed":0,"pass_rate_percentage":100}
If you want, I can continue with a small parser-selection layer next (e.g., by file extension or MIME) so SRT/WebVTT/TTML can be chosen automatically from one entry point.

### Iteration 3

Extended the prototype for Iteration 3 by adding external shot-change integration to the internal model and QC layer.

What I added
Internal model upgrade for external metadata:

Updated SubtitleQc.Core/Models/Cue.cs
Added optional StartFrame and EndFrame
Preserved existing constructor usage (id, start, end, lines)
Added SubtitleQc.Core/Models/ExternalQcData.cs
Holds ShotChangeTimestamps, ShotChangeFrames, and optional FrameRate
QC context/rule engine expansion:

Updated SubtitleQc.Core/Qc/QcContext.cs to carry optional external data
Updated SubtitleQc.Core/Qc/RuleEngine.cs
Kept existing Evaluate(cues) behavior
Added overload Evaluate(cues, externalData) for external-data aware flows
Shot-change abstraction and default provider:

Added SubtitleQc.Core/Qc/Abstractions/IShotChangeProvider.cs
Added SubtitleQc.Core/Qc/EmptyShotChangeProvider.cs
New rules for Iteration 3:

Added SubtitleQc.Core/Qc/Rules/CrossShotBoundaryCheckRule.cs
Supports constructor with IShotChangeProvider (as expected by tests)
Also supports parameterless constructor for compatibility
Added SubtitleQc.Core/Qc/Rules/MinFramesFromShotChangeRule.cs
Supports constructor with IShotChangeProvider + thresholdFrames
Kept constructor with only thresholdFrames
Uses cue frame data and shot-change frames, with fallback behavior
Validation
Ran dotnet test SubtitleQc.Tests/SubtitleQc.Tests.csproj
Final result: 19 passed, 0 failed
Logged Iteration 3 attempts in test_metrics.jsonl:
attempt 1: build failed (missing IShotChangeProvider)
attempt 2: build/tests successful, 100% pass rate