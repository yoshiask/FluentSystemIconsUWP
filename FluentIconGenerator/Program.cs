﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

namespace FluentIconGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define the path to the Fluent UI System icon pack
            
            string dir = (args.Length >= 1) ? args[0]
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                  "source", "repos", "fluentui-system-icons", "assets");
            string outputProj = (args.Length >= 2) ? args[1]
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                  "source", "repos", "FluentSystemIcons", "Fluent.Icons");
            string outputFile = Path.Combine(outputProj, "FluentSymbolIcon.g.cs");
            Regex svgReg = new Regex(@"ic_fluent_(\w+)_24_(regular|filled).svg");

            #region Code generation setup
            CompilationUnitSyntax cu = SF.CompilationUnit()
                .AddUsings(SF.UsingDirective(SF.IdentifierName("System.Collections.Generic")))
            ;

            var doc = @"// DO NOT edit this file! Changes will be overridden whenever the
    // FluentIconGenerator is run.
";
            SyntaxTrivia generatedMessageComment = SF.Comment(doc);

            NamespaceDeclarationSyntax ns = SF.NamespaceDeclaration(SF.IdentifierName("Fluent.Icons"));

            ClassDeclarationSyntax iconsClass = SF.ClassDeclaration("FluentSymbolIcon")
                .AddModifiers(SF.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SF.Token(SyntaxKind.PartialKeyword))
                .WithLeadingTrivia(SF.TriviaList(generatedMessageComment))
            ;

            EnumDeclarationSyntax fluentSymbolEnum = SF.EnumDeclaration("FluentSymbol")
                .AddModifiers(SF.Token(SyntaxKind.PublicKeyword));

            PropertyDeclarationSyntax allFluentIconsProp =
                SF.PropertyDeclaration(
                    SF.ParseTypeName("Dictionary<FluentSymbol, string>"),
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
                    SF.ParseTypeName("Dictionary<FluentSymbol, string>"),
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

                #region SVG reading
                // Load the path data into a string
                var svg = new XmlDocument();
                svg.Load(path);
                // create ns manager
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(svg.NameTable);
                xmlnsManager.AddNamespace("svg", "http://www.w3.org/2000/svg");

                XmlNodeList list = svg.LastChild.SelectNodes("//svg:path", xmlnsManager);
                string xamlPathData = "";
                foreach (XmlNode pathElem in list)
                {
                    xamlPathData += pathElem.Attributes["d"].Value + " ";
                }
                #endregion

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
                            SF.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SF.Literal(xamlPathData)
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
                SF.ParseTypeName("Dictionary<FluentSymbol, string>"),
                null,
                SF.InitializerExpression(
                    SyntaxKind.CollectionInitializerExpression,
                    dictInitCollection
                )
                .WithTrailingTrivia(SF.CarriageReturnLineFeed, SF.Whitespace("        "))
            )));
            iconsClass = iconsClass.AddMembers(allFluentIconsProp);
            ns = ns.AddMembers(fluentSymbolEnum);
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
