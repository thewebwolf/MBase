using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.Serialization;
using System.Linq;

using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Reflection;

namespace MBase.MBase.ServiceHost.Controllers
{
    public static class ControllerBuilder
    {
        public static byte[] CreateControllerCode(IService service)
        {
            var @file = SF.CompilationUnit();
            var @namespace = SF.NamespaceDeclaration(SF.ParseName("MBase.ServiceHost.Controllers")).NormalizeWhitespace();

            @file = @file
                .AddUsings(SF.UsingDirective(SF.IdentifierName("System")));
            @file = @file
                .AddUsings(SF.UsingDirective(SF.IdentifierName("Microsoft.AspNetCore.Mvc")));
            @file = @file
                .AddUsings(SF.UsingDirective(SF.IdentifierName("System.Threading.Tasks")));

            @file = @file.AddUsings(SF.UsingDirective(SF.IdentifierName(service.GetType().Namespace)));
            @file = @file.AddUsings(SF.UsingDirective(SF.IdentifierName(service.GetType().Namespace + ".Models")));

            foreach (var item in service.Commands.Where(m => typeof(ICommand).IsAssignableFrom(m.GetType())))
            {
                @file = @file.AddUsings(SF.UsingDirective(SF.IdentifierName(item.GetType().Namespace)));
            }





            var classDeclaration = SF.ClassDeclaration(service.GetType().Name.ToString() + "Controller")
                .AddModifiers(SF.Token(SyntaxKind.PublicKeyword))
                .AddBaseListTypes(SF.SimpleBaseType(SF.ParseTypeName("ControllerBase")))
                .WithAttributeLists(
                        SF.List(
                            new AttributeListSyntax[]{
                                SF.AttributeList(
                                    SF.SingletonSeparatedList(
                                         SF.Attribute(SF.IdentifierName("Route"))
                                            .WithArgumentList(
                                                SF.AttributeArgumentList(
                                                     SF.SingletonSeparatedList(
                                                        SF.AttributeArgument(
                                                            SF.LiteralExpression(SyntaxKind.StringLiteralExpression,SF.Literal($"api/[controller]")))))))),
                            SF.AttributeList(
                                    SF.SingletonSeparatedList(
                                         SF.Attribute(SF.IdentifierName("ApiController"))))}));

            List<MemberDeclarationSyntax> controllerMembers = new List<MemberDeclarationSyntax>();

            controllerMembers.Add(
                SF.FieldDeclaration(
                    SF.VariableDeclaration(
                        SF.ParseTypeName(service.GetType().Name))
                        .AddVariables(SF.VariableDeclarator("_service")))
                .AddModifiers(SF.Token(SyntaxKind.PrivateKeyword)));

            controllerMembers.Add(
                SF.ConstructorDeclaration(service.GetType().Name.ToString() + "Controller")
                    .WithParameterList(
                         SF.ParameterList(
                             SF.SingletonSeparatedList(
                                 SF.Parameter(
                                     SF.Identifier("service"))
                                 .WithType(
                                    SF.IdentifierName("IService")))

                             ))
                    .AddModifiers(SF.Token(SyntaxKind.PublicKeyword))
                    .WithBody(SF.Block(SF.ParseStatement($"this._service = ({service.GetType().Name})service;"))));

            foreach (var item in service.Commands.Where(m => typeof(ICommand).IsAssignableFrom(m.GetType())))
            {
                var syntax = @$"

            var response = await CommandHelper.ExecuteMethod<{item.Name}>(new {item.Name}(),new Request<{item.Name}>(request, new MessageEnvelope()));
            if(response.Envelope.HasErrors)
                throw response.Envelope.Exceptions[0];
            return ({item.ResponseType.Name})response.Message;".Split(Environment.NewLine).Select(line => SF.ParseStatement(line));

                controllerMembers.Add(SF.MethodDeclaration(SF.ParseTypeName($"Task<{item.ResponseType.Name}>"), item.Name)
                     .WithAttributeLists(
                        SF.List(
                            new AttributeListSyntax[]{
                                SF.AttributeList(
                                    SF.SingletonSeparatedList(
                                         SF.Attribute(SF.IdentifierName("Route"))
                                            .WithArgumentList(
                                                SF.AttributeArgumentList(
                                                     SF.SingletonSeparatedList(
                                                        SF.AttributeArgument(
                                                            SF.LiteralExpression(SyntaxKind.StringLiteralExpression,SF.Literal($"{item.Name}")))))))),
                                SF.AttributeList(
                                    SF.SingletonSeparatedList(
                                         SF.Attribute(SF.IdentifierName("HttpPost"))))
                            }))
                     .WithParameterList(
                             SF.ParameterList(
                                 SF.SingletonSeparatedList(
                                     SF.Parameter(
                                         SF.Identifier("request"))
                                     .WithType(
                                        SF.IdentifierName(item.RequestType.Name)))))


                    .AddModifiers(SF.Token(SyntaxKind.PublicKeyword))
                    .AddModifiers(SF.Token(SyntaxKind.AsyncKeyword))
                    .WithBody(SF.Block(syntax)));
            }
            controllerMembers.Add(SF.MethodDeclaration(SF.ParseTypeName("ActionResult<string>"), "Get")
                     .WithAttributeLists(
                        SF.List(
                            new AttributeListSyntax[]{
                                SF.AttributeList(
                                    SF.SingletonSeparatedList(
                                         SF.Attribute(SF.IdentifierName("Route"))
                                            .WithArgumentList(
                                                SF.AttributeArgumentList(
                                                     SF.SingletonSeparatedList(
                                                        SF.AttributeArgument(
                                                            SF.LiteralExpression(SyntaxKind.StringLiteralExpression,SF.Literal($"Ping")))))))),
                                SF.AttributeList(
                                    SF.SingletonSeparatedList(
                                         SF.Attribute(SF.IdentifierName("HttpGet"))))
                            }))
                    .AddModifiers(SF.Token(SyntaxKind.PublicKeyword))
                    .WithBody(SF.Block(SF.ParseStatement(@$"return ""Pong!"";"))));
            classDeclaration = classDeclaration.AddMembers(controllerMembers.ToArray());

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Normalize and get code as string.
            var code = @file.AddMembers(@namespace)
                .NormalizeWhitespace()
                .ToFullString();


            // Output new code to the console.
            var compiler = new Compiler();

            return compiler.Compile(code);
        }
    }
    internal class Compiler
    {
        public byte[] Compile(string sourceCode)
        {

            using (var peStream = new MemoryStream())
            {
                var result = GenerateCode(sourceCode).Emit(peStream);

                if (!result.Success)
                {
                    Console.WriteLine("Compilation done with error.");

                    var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    return null;
                }

                Console.WriteLine("Compilation done without any error.");

                peStream.Seek(0, SeekOrigin.Begin);

                return peStream.ToArray();
            }
        }

        private static CSharpCompilation GenerateCode(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp8);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

            var references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(Console).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(IService).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.Controller).Assembly.Location));


            foreach (var asb in AppDomain.CurrentDomain.GetAssemblies())
            {
                references.Add(MetadataReference.CreateFromFile(asb.Location));
            }

            return CSharpCompilation.Create($"dynamic_{Guid.NewGuid()}.dll",
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }
    }
}
