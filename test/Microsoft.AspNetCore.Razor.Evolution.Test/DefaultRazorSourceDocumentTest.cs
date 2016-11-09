// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class DefaultRazorSourceDocumentTest
    {
        [Fact]
        public void Indexer()
        {
            // Arrange
            var expectedContent = "Hello, World!";
            var content = CreateContent(expectedContent);
            var indexerBuffer = new char[expectedContent.Length];
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: "file.cshtml");

            // Act
            for (var i = 0; i < document.Length; i++)
            {
                indexerBuffer[i] = document[i];
            }

            // Assert
            var output = new string(indexerBuffer);
            Assert.Equal(expectedContent, output);
        }

        [Fact]
        public void Length()
        {
            // Arrange
            var expectedContent = "Hello, World!";
            var content = CreateContent(expectedContent);
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: "file.cshtml");

            // Act & Assert
            Assert.Equal(expectedContent.Length, document.Length);
        }

        [Fact]
        public void Filename()
        {
            // Arrange
            var content = CreateContent();

            // Act
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: "file.cshtml");

            // Assert
            Assert.Equal("file.cshtml", document.Filename);
        }

        [Fact]
        public void Filename_Null()
        {
            // Arrange
            var content = CreateContent();

            // Act
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: null);

            // Assert
            Assert.Null(document.Filename);
        }

        [Fact]
        public void CopyTo_PartialCopyFromStart()
        {
            // Arrange
            var content = CreateContent("Hello, World!");
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: null);
            var expectedContent = "Hello";
            var charBuffer = new char[expectedContent.Length];

            // Act
            document.CopyTo(0, charBuffer, 0, expectedContent.Length);

            // Assert
            var copiedContent = new string(charBuffer);
            Assert.Equal(expectedContent, copiedContent);
        }

        [Fact]
        public void CopyTo_PartialCopyDestinationOffset()
        {
            // Arrange
            var content = CreateContent("Hello, World!");
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: null);
            var expectedContent = "$Hello";
            var charBuffer = new char[expectedContent.Length];
            charBuffer[0] = '$';

            // Act
            document.CopyTo(0, charBuffer, 1, "Hello".Length);

            // Assert
            var copiedContent = new string(charBuffer);
            Assert.Equal(expectedContent, copiedContent);
        }

        [Fact]
        public void CopyTo_PartialCopySourceOffset()
        {
            // Arrange
            var content = CreateContent("Hello, World!");
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: null);
            var expectedContent = "World";
            var charBuffer = new char[expectedContent.Length];

            // Act
            document.CopyTo(7, charBuffer, 0, expectedContent.Length);

            // Assert
            var copiedContent = new string(charBuffer);
            Assert.Equal(expectedContent, copiedContent);
        }

        [Fact]
        public void CopyTo_WithEncoding()
        {
            // Arrange
            var content = CreateContent("Hi", encoding: Encoding.UTF8);
            var document = new DefaultRazorSourceDocument(content, Encoding.UTF8, filename: null);
            var charBuffer = new char[2];

            // Act
            document.CopyTo(0, charBuffer, 0, 2);

            // Assert
            var copiedContent = new string(charBuffer);
            Assert.Equal("Hi", copiedContent);
        }

        [Fact]
        public void CopyTo_Null_DetectsEncoding()
        {
            // Arrange
            var content = CreateContent("Hi", encoding: Encoding.UTF32);
            var document = new DefaultRazorSourceDocument(content, encoding: null, filename: null);
            var charBuffer = new char[2];

            // Act
            document.CopyTo(0, charBuffer, 0, 2);

            // Assert
            var copiedContent = new string(charBuffer);
            Assert.Equal("Hi", copiedContent);
        }

        [Fact]
        public void CopyTo_CanCopyMultipleTimes()
        {
            // Arrange
            var content = CreateContent("Hi", encoding: Encoding.UTF32);
            var document = new DefaultRazorSourceDocument(content, encoding: null, filename: null);

            // Act & Assert
            //
            // (we should be able to do this twice to prove that the underlying data isn't disposed)
            for (var i = 0; i < 2; i++)
            {
                var charBuffer = new char[2];
                document.CopyTo(0, charBuffer, 0, 2);
                var copiedContent = new string(charBuffer);
                Assert.Equal("Hi", copiedContent);
            }
        }

        private static MemoryStream CreateContent(string content = "Hello, World!", Encoding encoding = null)
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8, bufferSize: 1024, leaveOpen: true))
            {
                writer.Write(content);
            }

            return stream;
        }
    }
}
