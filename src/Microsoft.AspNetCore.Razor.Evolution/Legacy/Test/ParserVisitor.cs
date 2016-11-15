// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Evolution.Legacy
{
    internal abstract class ParserVisitor
    {
        public virtual void VisitBlock(Block block)
        {
            VisitStartBlock(block);

            for (var i = 0; i < block.Children.Count; i++)
            {
                block.Children[i].Accept(this);
            }

            VisitEndBlock(block);
        }

        public virtual void VisitStartBlock(Block block)
        {
        }

        public virtual void VisitEndBlock(Block block)
        {
        }

        public virtual void VisitSpan(Span span)
        {
        }
    }
}