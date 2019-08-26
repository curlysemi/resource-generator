using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBond.ResourceGenerator.Class
{
    internal static class DocumentationHelper
    {
        private static List<SyntaxToken> ToLine(string singleLine)
        {
            return new List<SyntaxToken>
            {
                SyntaxFactory.XmlTextLiteral(
                    SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                    singleLine,
                    string.Empty,
                    SyntaxFactory.TriviaList()),
                SyntaxFactory.XmlTextNewLine(
                    SyntaxFactory.TriviaList(),
                    Environment.NewLine,
                    string.Empty,
                    SyntaxFactory.TriviaList()),
            };
        }

        private static List<SyntaxToken> ToLines(string originalString)
        {
            var lines = new List<SyntaxToken>();

            // The strings might not contain the 'proper' newlines for the current environment (so not using `Environment.NewLine`), but this should correct both Windows and Unix
            var ls = originalString.Replace("\r", "").Split('\n');

            foreach (var l in ls.Select((val, i ) => new { val, i }))
            {
                var temp = l.val;
                if (l.i == 0)
                {
                    temp = $" Looks up a localized string similar to \"{temp}";
                }
                if (l.i == ls.Length - 1)
                {
                    temp += "\".";
                }
                lines.AddRange(ToLine(temp));
            }

            return lines;
        }

        public static SyntaxTriviaList GetDocumentationMessage(string originalString)
        {
            // The document comment trivia is collected in this list.
            List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

            var beforeLines = new[]
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
            };

            var lines = ToLines(originalString);

            var afterLines = new[]
            {
                SyntaxFactory.XmlTextLiteral(
                    SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                    " </summary>",
                    string.Empty,
                    SyntaxFactory.TriviaList()),
                SyntaxFactory.XmlTextNewLine(
                    SyntaxFactory.TriviaList(),
                    Environment.NewLine,
                    string.Empty,
                    SyntaxFactory.TriviaList()),
            };

            var all = new List<SyntaxToken>();

            all.AddRange(beforeLines);
            all.AddRange(lines);
            all.AddRange(afterLines);

            //        /// <summary>
            //        /// Gets the resource string indexed by the MyResourceString key.
            //        /// </summary>
            comments.Add(
                SyntaxFactory.Trivia(
                    SyntaxFactory.DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        SyntaxFactory.SingletonList<XmlNodeSyntax>(
                            SyntaxFactory.XmlText()
                            .WithTextTokens(
                                SyntaxFactory.TokenList(
                                    all.ToArray()
                                    ))))));

            // This is the complete document comment.
            return SyntaxFactory.TriviaList(comments);
        }
    }
}
