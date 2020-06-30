using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using System.Text;
using Svg;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.MSBuild;
using System.Reflection;
using Microsoft.CodeAnalysis.Options;

namespace FluentIconGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define the path to the Fluent UI System icon pack
            string dir = @"C:\Users\jjask\Pictures\Icons\SVG\fluentui-system-icons\assets";
            string outputProj = @"C:\Users\jjask\source\repos\FluentSystemIcons\Microsoft.Design.Fluent";
            string outputFile = Path.Combine(outputProj, "FluentSymbolIcon.g.cs");
            Regex svgReg = new Regex(@"ic_fluent_(\w+)_24_(regular|filled).svg");

            #region Code generation setup
            CompilationUnitSyntax cu = SF.CompilationUnit()
                .AddUsings(SF.UsingDirective(SF.IdentifierName("System")))
                .AddUsings(SF.UsingDirective(SF.IdentifierName("System.Collections.Generic")))
                .AddUsings(SF.UsingDirective(SF.IdentifierName("Windows.UI.Xaml.Controls")))
                .AddUsings(SF.UsingDirective(SF.IdentifierName("Windows.UI.Xaml.Media.Imaging")))
            ;

            NamespaceDeclarationSyntax ns = SF.NamespaceDeclaration(SF.IdentifierName("Microsoft.Design.Fluent"));

            ClassDeclarationSyntax iconsClass = SF.ClassDeclaration("FluentSymbolIcon")
                .AddModifiers(SF.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SF.Token(SyntaxKind.PartialKeyword))
            ;

            EnumDeclarationSyntax fluentSymbolEnum = SF.EnumDeclaration("FluentSymbol")
                .AddModifiers(SF.Token(SyntaxKind.PublicKeyword));

            PropertyDeclarationSyntax allFluentIconsProp =
                SF.PropertyDeclaration(
                    SF.ParseTypeName("Dictionary<FluentSymbol, SvgImageSource>"),
                    "AllFluentIcons"
                )
                .AddModifiers(SF.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SF.Token(SyntaxKind.StaticKeyword))
                .WithAccessorList(SF.AccessorList(SF.List(
                    new[] {
                        SF.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)),
                    }
                )))
                .WithInitializer(SF.EqualsValueClause(SF.ObjectCreationExpression(
                    SF.ParseTypeName("Dictionary<FluentSymbol, SvgImageSource>"),
                    null,
                    SF.InitializerExpression(
                        SyntaxKind.CollectionInitializerExpression,
                        SF.Token(SyntaxKind.OpenBraceToken),
                        new SeparatedSyntaxList<ExpressionSyntax>() { },
                        SF.Token(SyntaxKind.CloseBraceToken)
                    )
                )))
                .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken))
            ;
            var dictInitCollection = SF.SingletonSeparatedList<ExpressionSyntax>(null);
            #endregion

            foreach (string path in Directory.GetFiles(dir, @"*", SearchOption.AllDirectories))
            {
                var match = svgReg.Match(path);
                if (!match.Success)
                    continue;

                // Extrapolate the symbol name from the file path
                string file = path.Substring(dir.Length + 1); // Also remove the slash
                Console.WriteLine(file);
                bool isFilled = match.Groups[2].Value == "filled";
                string name = file.Split('\\')[0].Replace(" ", "") + (isFilled ? "Filled" : "");

                #region SVG reading (dead code)
                // Load the path data into a PathIcon
                //var svg = SvgDocument.Open(path);
                //var iconRootElem = svg.Children.FindSvgElementOf<SvgGroup>();
                //var iconElem = iconRootElem.Children[0];
                //SvgPath pathElem = iconElem.Children.FindSvgElementOf<SvgPath>();
                //string xamlPathElem = "new PathIcon {\r\n\tData = (Windows.UI.Xaml.Media.Geometry)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), \""
                //    + pathElem.PathData.ToString() + "\"),\r\n\tHorizontalAlignment = HorizontalAlignment.Center,\r\n\tVerticalAlignment = VerticalAlignment.Center\r\n};";
                //File.WriteAllText(outputFile, xamlPathElem);
                #endregion

                // Copy the SVG file to the Assets folder in the library project
                File.Copy(
                    path, Path.Combine(outputProj, "Assets", "Icons", name + ".svg"),
                    true
                );

                // Generate the C# source code
                // TODO: Switch to .NET source generators
                fluentSymbolEnum = fluentSymbolEnum.AddMembers(SF.EnumMemberDeclaration(name));
                dictInitCollection = dictInitCollection.Add(
                    SF.InitializerExpression(
                        SyntaxKind.ComplexElementInitializerExpression,
                        SF.SeparatedList<ExpressionSyntax>(new SyntaxNodeOrToken[] {
                            SF.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SF.IdentifierName("FluentSymbol"),
                                SF.IdentifierName(name)
                            ),
                            SF.Token(
                                SF.TriviaList(),
                                SyntaxKind.CommaToken,
                                SF.TriviaList(
                                    SF.Space
                                )
                            ),
                            SF.ObjectCreationExpression(
                                SF.IdentifierName("SvgImageSource")
                            )
                            .WithNewKeyword(SF.Token(SF.TriviaList(), SyntaxKind.NewKeyword, SF.TriviaList(
                                SF.Space
                            )))
                            .WithArgumentList(SF.ArgumentList(SF.SingletonSeparatedList(SF.Argument(
                                    SF.ObjectCreationExpression(
                                        SF.IdentifierName("Uri")
                                    )
                                    .WithNewKeyword(SF.Token(SF.TriviaList(), SyntaxKind.NewKeyword, SF.TriviaList(
                                        SF.Space
                                    )))
                                    .WithArgumentList(SF.ArgumentList(SF.SingletonSeparatedList(SF.Argument(
                                        SF.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SF.Literal($"ms-appx:///Microsoft.Design.Fluent/Assets/Icons/{name}.svg"
                                        )
                                    )))))
                                )))
                                .WithCloseParenToken(SF.Token(SF.TriviaList(), SyntaxKind.CloseParenToken, SF.TriviaList(
                                    SF.Space
                                )))
                            )
                        })
                    )
                    .WithLeadingTrivia(
                        SF.ElasticCarriageReturnLineFeed, SF.Whitespace("            ")
                    )
                );
            }

            // Update the document
            allFluentIconsProp = allFluentIconsProp.WithInitializer(SF.EqualsValueClause(SF.ObjectCreationExpression(
                SF.ParseTypeName("Dictionary<FluentSymbol, SvgImageSource>"),
                null,
                SF.InitializerExpression(
                    SyntaxKind.CollectionInitializerExpression,
                    dictInitCollection
                )
                .WithTrailingTrivia(SF.CarriageReturnLineFeed, SF.Whitespace("        "))
            )));
            iconsClass = iconsClass.AddMembers(allFluentIconsProp);
            iconsClass = iconsClass.AddMembers(fluentSymbolEnum);
            ns = ns.AddMembers(iconsClass);
            cu = cu.AddMembers(ns);

            // Write the generated code to the output file
            using (var workspace = MSBuildWorkspace.Create())
            {
                SyntaxNode formattedNode = Formatter.Format(cu, workspace);
                using (StreamWriter writer = File.CreateText(outputFile))
                {
                    formattedNode.WriteTo(writer);
                }

                //var task = workspace.OpenSolutionAsync(@"C:\Users\jjask\source\repos\FluentSystemIcons\FluentSystemIcons.sln");
                //task.Wait();
                //var sln = task.Result;
                //Project proj = sln.AddProject("Microsoft.Design.Fluent", "Microsoft.Design.Fluent", cu.Language);
                //var doc = proj.AddDocument("Icons.cs", formattedNode);
                //proj = doc.Project;
                //sln = proj.Solution;
                //workspace.TryApplyChanges(sln);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\r\nGeneration complete!");
            Console.ResetColor();
            Console.WriteLine("File at:");
            Console.WriteLine(outputFile);
            Console.WriteLine("\r\nPress any key to exit...");

            Console.Read();
        }
    }
}
