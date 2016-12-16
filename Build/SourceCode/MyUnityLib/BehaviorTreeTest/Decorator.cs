using System;
using System.Collections.Generic;

namespace WeiBHTLibrary
{
    public class Decorator : Composite
    {
        public Func<bool> canPass { protected get; set; }

        public Decorator()
        { 
            Update = () =>
            {
                if (canPass != null && canPass() && Children != null && Children.Count > 0)
                {
                    return Children[0].Tick();
                }
                return Status.BhFailure;
            };
        }
    }
}
