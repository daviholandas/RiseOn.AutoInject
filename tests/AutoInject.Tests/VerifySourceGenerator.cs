using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RiseOn.AutoInject;

namespace AutoInject.Tests;

public static class VerifySourceGenerator
{
    public static Task Verify(string source)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
        CSharpCompilation compilation = CSharpCompilation.Create(
            "AutoInject.Tests",
            [ syntaxTree ]);

        ServiceCollectionGenerator generator = new();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGenerators(compilation);

        return Verifier.Verify(driver)
            .UseDirectory("TestResults");
    }
}