// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Parser.SyntaxTree;

namespace Microsoft.AspNetCore.Razor.Chunks.Generators
{
    public class SetBaseTypeChunkGenerator : SpanChunkGenerator
    {
        public SetBaseTypeChunkGenerator(string baseType)
        {
            BaseType = baseType;
        }

        public string BaseType { get; }

        public override void GenerateChunk(Span target, ChunkGeneratorContext context)
        {
            context.ChunkTreeBuilder.AddSetBaseTypeChunk(BaseType, target);
        }

        public override string ToString()
        {
            return "Base:" + BaseType;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SetBaseTypeChunkGenerator;
            return other != null &&
                string.Equals(BaseType, other.BaseType, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return BaseType.GetHashCode();
        }
    }
}
