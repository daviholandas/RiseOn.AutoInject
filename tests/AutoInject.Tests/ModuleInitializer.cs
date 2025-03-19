using System.Runtime.CompilerServices;

namespace AutoInject.Tests;

public sealed class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}