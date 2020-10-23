using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Linker;
using Mono.Linker.Steps;
using System.Diagnostics;

namespace ILLinkAnalyzers
{
    class SingleFileAnalysisSubStep : ISubStep
    {
        private LinkContext Context;

        public SubStepTargets Targets => SubStepTargets.Type | SubStepTargets.Method;

        public void Initialize(LinkContext context)
        {
            Context = context;
        }

        public bool IsActiveFor(AssemblyDefinition assembly)
        {
            return true;
        }

        public void ProcessAssembly(AssemblyDefinition assembly)
        {
        }

        public void ProcessEvent(EventDefinition @event)
        {
        }

        public void ProcessField(FieldDefinition field)
        {
        }

        public void ProcessMethod(MethodDefinition method)
        {
            if (method.HasBody)
            {
                foreach (var instruction in method.Body.Instructions)
                {
                    switch (instruction.OpCode.Code)
                    {
                        case Code.Call:
                        case Code.Calli:
                        case Code.Callvirt:
                            string accessedMemberName = null;
                            int code = 0;
                            if (instruction.Operand is MethodReference methodRef)
                            {
                                if (methodRef.Name == "GetFile")
                                    Debug.WriteLine("");

                                switch (methodRef.DeclaringType.FullName)
                                {
                                    case "System.Reflection.Assembly":
                                        switch (methodRef.Name)
                                        {
                                            case "get_Location":
                                                accessedMemberName = "Assembly.Location";
                                                code = 3000;
                                                break;
                                            case "GetFile":
                                                accessedMemberName = "Assembly.GetFile";
                                                code = 3001;
                                                break;
                                            case "GetFiles":
                                                accessedMemberName = "Assembly.GetFiles";
                                                code = 3001;
                                                break;
                                        }
                                        break;
                                    case "System.Reflection.AssemblyName":
                                        switch (methodRef.Name)
                                        {
                                            case "get_CodeBase":
                                                accessedMemberName = "AssemblyName.CodeBase";
                                                code = 3000;
                                                break;
                                            case "get_EscapedCodeBase":
                                                accessedMemberName = "AssemblyName.EscapedCodeBase";
                                                code = 3000;
                                                break;
                                        }
                                        break;
                                }

                                if (code != 0)
                                {
                                    Context.LogMessage(
                                        MessageContainer.CreateWarningMessage(
                                            Context,
                                            $"Access to {accessedMemberName}",
                                            code,
                                            new MessageOrigin(method, instruction.Offset),
                                            WarnVersion.ILLink5));
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public void ProcessProperty(PropertyDefinition property)
        {
        }

        public void ProcessType(TypeDefinition type)
        {
        }
    }
}
