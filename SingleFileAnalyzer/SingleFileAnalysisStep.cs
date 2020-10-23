using System;
using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;

namespace SingleFileAnalyzer
{
    public class SingleFileAnalysisStep : BaseStep
    {
        public SingleFileAnalysisStep()
        {
        }

        protected override void ProcessAssembly(AssemblyDefinition assembly)
        {
            foreach (TypeDefinition type in assembly.MainModule.Types)
            {
                ProcessType(type);
            }
        }

        void ProcessType(TypeDefinition type)
        {
            foreach (TypeDefinition nestedType in type.NestedTypes)
            {
                ProcessType(nestedType);
            }

            foreach (MethodDefinition method in type.Methods)
            {
                ProcessMethod(method);
            }
        }

        void ProcessMethod (MethodDefinition method)
        {
        }
    }
}
