ServiceCollection services = new();
services.___<FailureCounter>();   // one instance for the whole provider
services.___<AnalysisRun>();      // one instance per scope
services.___<FailureMatcher>();   // a new instance on every request
