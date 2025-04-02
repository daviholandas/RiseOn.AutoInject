using AutoInject.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RiseOn.AutoInject;

namespace AutoInject.Tests;

public sealed class TestHelper
{
     public static Task VerifyGeneratedCode(string source)
     {
          var syntaxTree = CSharpSyntaxTree.ParseText(source);
          var compilation = CSharpCompilation.Create("AutoInject.Tests",
               [ syntaxTree ],
               [ MetadataReference.CreateFromFile(typeof(object).Assembly.Location) ]);

          ServiceCollectionGenerator generator = new ();
          GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
          driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

          return Verifier.Verify(driver).UseDirectory("Snapshot_Results");
     }
}