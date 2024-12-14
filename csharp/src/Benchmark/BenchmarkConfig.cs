using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;

namespace Benchmark;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        SummaryStyle = SummaryStyle.Default
            .WithRatioStyle(RatioStyle.Percentage);

        var baseJob = Job.Default;



        AddJob(baseJob.WithArguments(new[] { new MsBuildArgument("/p:USE_NUGET=true") })
            .WithId("3.29.0")
            .WithBaseline(true)
        );
        AddJob(baseJob.WithArguments(new[] { new MsBuildArgument("/p:USE_NUGET=false") })
            .WithId("improved-fixed-size-repeated")
        );
    }
}