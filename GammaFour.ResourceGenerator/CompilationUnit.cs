// <copyright file="CompilationUnit.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ResourceGenerator
{
    using System.Xml.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A complete unit for the compiler.
    /// </summary>
    public class CompilationUnit
    {
        /// <summary>
        /// The name of the target class.
        /// </summary>
        private string targetClassName;

        /// <summary>
        /// The namespace of the target module.
        /// </summary>
        private string targetNamespace;

        /// <summary>
        /// The data model schema.
        /// </summary>
        private XDocument xDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationUnit"/> class.
        /// </summary>
        /// <param name="xDocument">The data model schema.</param>
        /// <param name="targetClassName">The target class name.</param>
        /// <param name="targetNamespace">The target namespace.</param>
        public CompilationUnit(XDocument xDocument, string targetClassName, string targetNamespace)
        {
            // Initialize the object.
            this.xDocument = xDocument;
            this.targetClassName = targetClassName;
            this.targetNamespace = targetNamespace;

            // This is the syntax for the compilation unit.
            this.Syntax = SyntaxFactory.CompilationUnit().WithMembers(this.Members);
        }

        /// <summary>
        /// Gets or sets the syntax.
        /// </summary>
        public CSharpSyntaxNode Syntax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // The compilation unit consists of a single namespace.
                Namespace @namespace = new Namespace(this.xDocument, this.targetNamespace, this.targetClassName);
                return SyntaxFactory.SingletonList<MemberDeclarationSyntax>(@namespace.Syntax);
            }
        }
    }
}