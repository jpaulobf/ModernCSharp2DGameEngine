namespace util;

public sealed class Util {
    
    /**
     * Code from Microsoft.com
     * How to use: ParallelOptions options = new() { MaxDegreeOfParallelism = Environment.ProcessorCount };
     * Util.ParallelWhile(options, () => this.isEngineRunning, body => { 
     */
    public static void ParallelWhile(ParallelOptions parallelOptions,
                                         Func<bool> condition,
                                         Action<ParallelLoopState> body)
    {
        ArgumentNullException.ThrowIfNull(parallelOptions);
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(body);

        int workersCount = parallelOptions.MaxDegreeOfParallelism switch
        {
            -1 => Int32.MaxValue, // -1 means unlimited parallelism.
            _ => parallelOptions.MaxDegreeOfParallelism
        };

        Parallel.For(0, workersCount, parallelOptions, (_, state) =>
        {
            while (!state.ShouldExitCurrentIteration)
            {
                if (!condition()) { state.Stop(); break; }
                body(state);
            }
        });
    }
}