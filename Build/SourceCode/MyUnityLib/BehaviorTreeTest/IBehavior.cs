using System;
using System.Collections.Generic;

namespace WeiBHTLibrary
{
    public interface IBehavior
    {
        
        Status Status { get; set; }
        Action Initialize { set; }
        Func<Status> Update { set; } 
        Action<Status> Terminate { set; }

        Status Tick();
        void Reset();
    }
}