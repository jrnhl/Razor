// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    internal class DefaultRazorSourceDocument : RazorSourceDocument
    {
        private MemoryStream _stream;
        private string _content;

        public DefaultRazorSourceDocument(MemoryStream stream, Encoding encoding, string filename)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            _stream = stream;
            Encoding = encoding;
            Filename = filename;
        }

        private string Content
        {
            get
            {
                if (_content == null)
                {
                    InitializeContent();
                }

                return _content;
            }
        }

        public override char this[int position] => Content[position];

        public override Encoding Encoding { get; }

        public override string Filename { get; }

        public override int Length => Content.Length;

        public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) =>
            Content.CopyTo(sourceIndex, destination, destinationIndex, count);

        private void InitializeContent()
        {
            var copy = new MemoryStream(_stream.ToArray());
            var reader = Encoding == null
                ? new StreamReader(copy, detectEncodingFromByteOrderMarks: true)
                : new StreamReader(copy, Encoding);
            using (reader)
            {
                var text = reader.ReadToEnd();

                if (Encoding != null && Encoding != reader.CurrentEncoding)
                {
                    throw new InvalidOperationException($"The set {nameof(Encoding)} does not match the provided content's encoding.");
                }

                _content = text;
            }
        }
    }
}
