// <copyright file="StringProperty.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
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
    public class StringProperty : SyntaxElement
    {
        /// <summary>
        /// The target class name;
        /// </summary>
        private string targetClassName;

        private string originalString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringProperty"/> class.
        /// </summary>
        /// <param name="targetClassName">The name of the target class.</param>
        /// <param name="key">The key used to reference this string.</param>
        public StringProperty(string targetClassName, string key, string originalString)
        {
            // Initialize the object.
            this.targetClassName = targetClassName;
            this.Name = key;
            this.originalString = originalString;

            //        /// <summary>
            //        /// Gets the resource string indexed by the MyResourceString key.
            //        /// </summary>
            //        public string MyResourceString
            //        {
            //          <AccessorList>
            //        {
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithAccessorList(this.AccessorList)
                .WithModifiers(this.Modifiers)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the list of accessors.
        /// </summary>
        private AccessorListSyntax AccessorList
        {
            get
            {
                // This collects the accessors.
                List<AccessorDeclarationSyntax> accessors = new List<AccessorDeclarationSyntax>();

                //            get
                //            {
                //                <GetAccessorBlock>
                //            }
                accessors.Add(
                    SyntaxFactory.AccessorDeclaration(
                        SyntaxKind.GetAccessorDeclaration,
                        SyntaxFactory.Block(SyntaxFactory.List(this.GetAccessorBlock))));

                // This is the complete list of accessors.
                return SyntaxFactory.AccessorList(SyntaxFactory.List(accessors));
            }
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
        /// Gets the 'Get' accessor.
        /// </summary>
        private List<StatementSyntax> GetAccessorBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                return Resource1.resourceManager.GetString("Welcome", Resource1.CultureInfo);
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
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
                                    })))));

                // This is the complete statement block.
                return statements;
            }
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