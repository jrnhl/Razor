// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution.Legacy
{
    public class RazorParserTest
    {
        [Fact]
        public void CanParseStuff()
        {
            var parser = new RazorParser();
            var sourceDocument = TestRazorSourceDocument.CreateResource("TestFiles/Source/BasicMarkup.cshtml");
            var sourceContent = new char[sourceDocument.Length];
            sourceDocument.CopyTo(0, sourceContent, 0, sourceDocument.Length);
            var output = parser.Parse(sourceContent);

            Assert.NotNull(output);
        }

        [Fact]
        public void ParseMethodCallsParseDocumentOnMarkupParserAndReturnsResults()
        {
            var factory = new SpanFactory();

            // Arrange
            var parser = new RazorParser();

            // Act/Assert
            ParserTestBase.EvaluateResults(parser.Parse(new StringReader("foo @bar baz")),
                new MarkupBlock(
                    factory.Markup("foo "),
                    new ExpressionBlock(
                        factory.CodeTransition(),
                        factory.Code("bar")
                               .AsImplicitExpression(CSharpCodeParser.DefaultKeywords)
                               .Accepts(AcceptedCharacters.NonWhiteSpace)),
                    factory.Markup(" baz")));
        }

        [Fact]
        public void ParseMethodUsesProvidedParserListenerIfSpecified()
        {
            var factory = new SpanFactory();

            // Arrange
            var parser = new RazorParser();

            // Act
            var results = parser.Parse(new StringReader("foo @bar baz"));

            // Assert
            ParserTestBase.EvaluateResults(results,
                new MarkupBlock(
                    factory.Markup("foo "),
                    new ExpressionBlock(
                        factory.CodeTransition(),
                        factory.Code("bar")
                               .AsImplicitExpression(CSharpCodeParser.DefaultKeywords)
                               .Accepts(AcceptedCharacters.NonWhiteSpace)),
                    factory.Markup(" baz")));
        }
    }
}
