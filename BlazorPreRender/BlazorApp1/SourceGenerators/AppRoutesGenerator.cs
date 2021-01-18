using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SourceGenerators
{
    // see https://github.com/dotnet/roslyn-sdk/blob/master/samples/CSharp/SourceGenerators/SourceGeneratorSamples/AutoNotifyGenerator.cs
    // https://github.com/dotnet/roslyn-sdk/blob/master/samples/CSharp/SourceGenerators/SourceGeneratorSamples/MustacheGenerator.cs
    [Generator]
    public class AppRoutesGenerator : ISourceGenerator
    {
        private const string RouteAttributeName = "Microsoft.AspNetCore.Components.RouteAttribute";
        
        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                Debugger.Launch();
                var allRoutePaths = GetRouteTemplates(context.Compilation);

                var dictSource = SourceText.From(Templates.AppRoutes(allRoutePaths), Encoding.UTF8);
                context.AddSource("AppRoutes", dictSource);
            }
            catch (Exception)
            {
                Debugger.Launch();
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //Debugger.Launch();
        }

        private static ImmutableArray<string> GetRouteTemplates(Compilation compilation)
        {
            // Get all classes
            IEnumerable<SyntaxNode> allNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            IEnumerable<ClassDeclarationSyntax> allClasses = allNodes
                .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                .OfType<ClassDeclarationSyntax>();
            
            return allClasses
                .Select(component => GetRoutePath(compilation, component))
                .Where(route => route is not null)
                .Cast<string>()// stops the nullable lies
                .ToImmutableArray();
        }
        
        private static string? GetRoutePath(Compilation compilation, ClassDeclarationSyntax component)
        {
            var routeAttribute = component.AttributeLists
                .SelectMany(x => x.Attributes)
                .FirstOrDefault(attr => attr.Name.ToString() == RouteAttributeName);
                
            if (routeAttribute?.ArgumentList?.Arguments.Count != 1)
            {
                // no route path
                return null;
            }
                
            var semanticModel = compilation.GetSemanticModel(component.SyntaxTree);

            var routeArg = routeAttribute.ArgumentList.Arguments[0];
            var routeExpr = routeArg.Expression;
            return semanticModel.GetConstantValue(routeExpr).ToString();
        }
    }
}
