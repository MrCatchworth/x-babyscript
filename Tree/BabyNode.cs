﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBabyScript.Tree
{
    public abstract class BabyNode
    {
        public readonly BabyNode[] Children;

        public BabyNode(BabyNode[] children)
        {
            Children = children ?? new BabyNode[0];
        }
    }
}
