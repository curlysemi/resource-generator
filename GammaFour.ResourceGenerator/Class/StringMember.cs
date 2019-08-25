namespace GammaFour.ResourceGenerator
{
    using System;
    using System.Collections.Generic;
    using DarkBond.ResourceGenerator.Class;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a property to start the background thread that reconciles the data.
    /// </summary>
    public class StringMember : SyntaxElement
    {
        /// <summary>
        /// The target class name;
        /// </summary>
        private string targetClassName;

        private string originalString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMember"/> class.
        /// </summary>
        /// <param name="targetClassName">The name of the target class.</param>
        /// <param name="key">The key used to reference this string.</param>
        public StringMember(string targetClassName, string key, IEnumerable<string> parameters, string originalString)
        {
            // Initialize the object.
            this.targetClassName = targetClassName;
            this.Name = key;
            this.originalString = originalString;

            //        /// <summary>
            //        /// Gets the resource string indexed by the MyResourceString key.
            //        /// </summary>
            //        public static string MyResourceString(object parameter1, object parameter2)
            //        {
            //          <AccessorList>
            //        {

            List<ParameterSyntax> parameterList = new List<ParameterSyntax>();

            foreach (var @param in parameters)
            {
                parameterList.Add(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(@param)).WithType(SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.ObjectKeyword))
                    )
                );
            }

            this.Syntax = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithBody(this.Body(parameters))
                .WithModifiers(this.Modifiers)
                .AddParameterListParameters(parameterList.ToArray())
                .WithLeadingTrivia(this.DocumentationComment);

            //this.Syntax = SyntaxFactory.PropertyDeclaration(
            //        SyntaxFactory.PredefinedType(
            //            SyntaxFactory.Token(SyntaxKind.StringKeyword)),
            //        SyntaxFactory.Identifier(this.Name))
            //    .WithAccessorList(this.Body)
            //    .WithModifiers(this.Modifiers)
            //    .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body(IEnumerable<string> parameters)
        {
            return SyntaxFactory.Block(SyntaxFactory.List(this.BuildGetResourceBlock(parameters)));
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                return DocumentationHelper.GetDocumentationMessage(this.originalString);
            }
        }

        /// <summary>
        /// Gets the 'GetString' block with appropriate template replacements
        /// </summary>
        private List<StatementSyntax> BuildGetResourceBlock(IEnumerable<string> parameters)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();


            var statement = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.targetClassName),
                                SyntaxFactory.IdentifierName("ResourceManager")),
                            SyntaxFactory.IdentifierName("GetString")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(this.Name))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(this.targetClassName),
                                            SyntaxFactory.IdentifierName("CultureInfo")))
                                })));

            foreach (var param in parameters)
            {
               statement = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, statement, SyntaxFactory.IdentifierName("Replace")),
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal("{{" + param + "}}"))),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.Argument(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(param),
                                            SyntaxFactory.IdentifierName("ToString"))))
                            }))
                    );
            }

            var returnStatement = SyntaxFactory.ReturnStatement(statement);


            statements.Add(returnStatement);

            // This is the complete statement block.
            return statements;
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                // internal
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword)
                    });
            }
        }
    }
}