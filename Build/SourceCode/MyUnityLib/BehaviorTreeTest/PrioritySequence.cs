using System;
using System.Collections.Generic;


namespace WeiBHTLibrary
{
    public class PrioritySequence : Sequence
    {
        private int lastChildIndex;
        public PrioritySequence() {
            Update = () =>
            {
                childIndex = 0;
                for (;;)
                {
                    Status s = GetChild(childIndex).Tick();
                    if (s != Status.BhSuccess)
                    {
                        for (int i = childIndex + 1; i <= lastChildIndex; i++) {
                            GetChild(i).Reset();
                        }
                        return s;
                    }

                    if (++childIndex == ChildCount)
                    {
                        childIndex = 0;
                        return Status.BhSuccess;
                    }
                }
            };
        }
    }
}
