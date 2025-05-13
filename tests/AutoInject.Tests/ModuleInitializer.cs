using System.Runtime.CompilerServices;

namespace AutoInject.Tests;


public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
        => VerifySourceGenerators.Initialize();
}