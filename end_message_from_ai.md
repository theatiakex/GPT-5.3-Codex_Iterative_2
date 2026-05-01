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