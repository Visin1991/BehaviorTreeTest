using System;
using System.Collections.Generic;

namespace WeiBHTLibrary
{
    public class Condition : Behavior
    {
        public Func<bool> CanPass { protected get; set; }
        public Condition() {
            Update = () =>
            {
                if (CanPass != null && CanPass())
                {
                    return Status.BhSuccess;
                }
                return Status.BhFailure;
            };

        }
    }
}
