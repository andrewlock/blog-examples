using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerators
{
    [Generator]
    public class PageDetailsGenerator : ISourceGenerator
    {
        private const string RouteAttributeName = "Microsoft.AspNetCore.Components.RouteAttribute";
        private const string MenuItemAttributeName = "MenuItem";
        
        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                
                IEnumerable<RouteableComponent> menuComponents = GetMenuComponents(context.Compilation);

                context.AddSource("PageDetail", SourceText.From(Templates.PageDetail(), Encoding.UTF8));
                context.AddSource("MenuItemAttribute", SourceText.From(Templates.MenuItemAttribute(), Encoding.UTF8));
                var pageDetailsSource = SourceText.From(Templates.MenuPages(menuComponents), Encoding.UTF8);
                context.AddSource("PageDetails", pageDetailsSource);
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

        private static ImmutableArray<RouteableComponent> GetMenuComponents(Compilation compilation)
        {
            // Get all classes
            IEnumerable<SyntaxNode> allNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            IEnumerable<ClassDeclarationSyntax> allClasses = allNodes
                .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                .OfType<ClassDeclarationSyntax>();

            return allClasses
                .Select(component => TryGetMenuComponent(compilation, component))
                .Where(page => page is not null)
                .Cast<RouteableComponent>() // stops the nullable lies
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Title)
                .ToImmutableArray();
        }
        
        private static RouteableComponent? TryGetMenuComponent(Compilation compilation, ClassDeclarationSyntax component)
        {
            var attributes = component.AttributeLists
                .SelectMany(x => x.Attributes)
                .Where(attr => 
                    attr.Name.ToString() == RouteAttributeName
                    || attr.Name.ToString() == MenuItemAttributeName)
                .ToList();

            var routeAttribute = attributes.FirstOrDefault(attr => attr.Name.ToString() == RouteAttributeName);
            var menuItemAttribute = attributes.FirstOrDefault(attr => attr.Name.ToString() == MenuItemAttributeName);

            if (routeAttribute is null || menuItemAttribute is null)
            {
                return null;
            }
                
            if (
                routeAttribute.ArgumentList?.Arguments.Count != 1 ||
                menuItemAttribute.ArgumentList?.Arguments.Count < 2)
            {
                // no route path or description value
                return null;
            }
                
            var semanticModel = compilation.GetSemanticModel(component.SyntaxTree);

            var routeArg = routeAttribute.ArgumentList.Arguments[0];
            var routeExpr = routeArg.Expression;
            var routeTemplate = semanticModel.GetConstantValue(routeExpr).ToString();
            
            var iconArg = menuItemAttribute.ArgumentList.Arguments[0];
            var iconExpr = iconArg.Expression;
            var icon = semanticModel.GetConstantValue(iconExpr).ToString();
            
            var descriptionArg = menuItemAttribute.ArgumentList.Arguments[1];
            var descriptionExpr = descriptionArg.Expression;
            var title = semanticModel.GetConstantValue(descriptionExpr).ToString();

            var order = 0;
            if (menuItemAttribute.ArgumentList?.Arguments.Count == 3)
            {
                var orderArg = menuItemAttribute.ArgumentList.Arguments[2];
                var orderExpr = orderArg.Expression;
                var maybeOrder = semanticModel.GetConstantValue(orderExpr);
                if (maybeOrder.HasValue)
                {
                    order = (int) maybeOrder.Value;
                }
            }
            
            return new RouteableComponent(routeTemplate, title, icon, order);
        }
    }
}