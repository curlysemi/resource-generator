// <copyright file="Class.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ResourceGenerator.Class
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// The data model class.
    /// </summary>
    public class Class : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private XDocument xDocument;

        /// <summary>
        /// The target namespace.
        /// </summary>
        private string targetNamespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="xDocument">A description of a unique constraint.</param>
        /// <param name="targetNamespace">The target namespace.</param>
        /// <param name="targetClassName">The target class name.</param>
        public Class(XDocument xDocument, string targetNamespace, string targetClassName)
        {
            // Initialize the object.
            this.xDocument = xDocument;
            this.targetNamespace = targetNamespace;
            this.Name = targetClassName;

            //    /// <summary>
            //    /// A strongly-typed resource class, for looking up localized strings, etc.
            //    /// </summary>
            //    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
            //    public class Resource1
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.Name)
                .WithAttributeLists(this.AttributeLists)
                .WithModifiers(this.Modifiers)
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the data contract attribute syntax.
        /// </summary>
        private SyntaxList<AttributeListSyntax> AttributeLists
        {
            get
            {
                // This collects all the attributes.
                List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

                //    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
                attributes.Add(
                   SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("SuppressMessage"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("Microsoft.Design"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("CA1053:StaticHolderTypesShouldNotHaveConstructors")))
                                        }))))));

                //    [DebuggerNonUserCode()]
                attributes.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("DebuggerNonUserCode"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList()))));

                //    [CompilerGenerated()]
                attributes.Add(
                   SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("CompilerGenerated"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList()))));

                // The collection of attributes.
                return SyntaxFactory.List<AttributeListSyntax>(attributes);
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                //    /// <summary>
                //    /// A strongly-typed resource class, for looking up localized strings, etc.
                //    /// </summary>
                return SyntaxFactory.TriviaList(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                SyntaxFactory.XmlText()
                                .WithTextTokens(
                                    SyntaxFactory.TokenList(
                                        new[]
                                        {
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                " <summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                " A strongly-typed resource class, for looking up localized strings, etc.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                " </summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList())
                                        }))))));
            }
        }

        /// <summary>
        /// Gets the members syntax.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreatePrivateInstanceFields(members);
                members = this.CreateConstructors(members);
                members = this.CreatePublicInstanceProperties(members);
                return members;
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    });
            }
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // These are the constructors.
            members = members.Add(new Constructor(this.Name).Syntax);

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateInstanceFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the internal instance properties.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            fields.Add(new ResourceManagerField());

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the internal instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new CultureInfoProperty());
            properties.Add(new ResourceManagerProperty(this.targetNamespace, this.Name));

            // Add a property for every string resource.
            foreach (XElement xElement in this.xDocument.Root.Elements(XName.Get("data")))
            {
                // Only add strings that have actual values
                var originalStringElement = xElement.Descendants(XName.Get("value")).FirstOrDefault();
                if (originalStringElement != null)
                {
                    string originalString = null;
                    using (XmlReader reader = originalStringElement.CreateReader())
                    {
                        if (reader.Read())
                            originalString = reader.ReadInnerXml();
                    }

                    // Only add strings that have actual values
                    if (!string.IsNullOrWhiteSpace(originalString))
                    {
                        // Determine if we are dealing with a template string or a regular phrase
                        if (originalString.Contains("{{") && originalString.Contains("}}"))
                        {
                            // We're dealing with a template string

                            // Get all the parameters
                            var @params = new List<string>();

                            int numHandlebars = 0;
                            string currentParamToken = string.Empty;
                            foreach (var c in originalString)
                            {
                                bool inToken = numHandlebars == 2;
                                if (inToken)
                                {
                                    // We are still in the template parameter token
                                    currentParamToken += c;
                                }
                                else if (numHandlebars == 0 && !string.IsNullOrWhiteSpace(currentParamToken))
                                {
                                    // We have just exited the template paramter token
                                    var cleaned = currentParamToken.Substring(0, currentParamToken.Length - 1).Trim();
                                    @params.Add(cleaned);
                                    currentParamToken = string.Empty;
                                }

                                if (c == '{')
                                {
                                    // We might be entering a template parameter token
                                    numHandlebars++;
                                }
                                else if (c == '}')
                                {
                                    // We might be exiting a template parameter token
                                    numHandlebars--;
                                }
                                else if (!inToken)
                                {
                                    numHandlebars = 0;
                                    
                                    // This is just in case, because this is being written without any testing :) 
                                    currentParamToken = string.Empty;
                                }
                            }

                            string propertyName = xElement.Attribute(XName.Get("name")).Value;
                            string usablePropertyName = propertyName;
                            int specialStarIndex = propertyName.IndexOf('*');
                            string templateID = null;
                            if (specialStarIndex > -1)
                            {
                                usablePropertyName = propertyName.Substring(0, specialStarIndex);
                                templateID = propertyName.Substring(specialStarIndex).Replace("*", string.Empty);
                            }
                            if (!string.IsNullOrWhiteSpace(usablePropertyName))
                            {
                                properties.Add(new StringMember(this.Name, usablePropertyName, @params, originalString));
                                if (!string.IsNullOrWhiteSpace(templateID))
                                {
                                    properties.Add(new TemplateStringMember(this.Name, $"{templateID}__{usablePropertyName}", @params, originalString, templateID));
                                }
                            }
                        }
                        else
                        {
                            // Regular phrase:
                            properties.Add(new StringProperty(this.Name, xElement.Attribute(XName.Get("name")).Value, originalString));
                        }
                    }
                }
            }

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in properties.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }
    }
}