using Mono.Linker.Steps;

namespace ILLinkAnalyzers
{
    public class AnalyzersStep : SubStepsDispatcher
    {
        public AnalyzersStep()
        {
            Add(new SingleFileAnalysisSubStep());
        }
    }
}
