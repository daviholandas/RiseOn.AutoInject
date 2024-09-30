using System.Runtime.CompilerServices;

namespace RiseOn.AutoInject.Tests;

public class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifySourceGenerators.Initialize();
    }
}