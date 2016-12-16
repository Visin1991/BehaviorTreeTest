using System;
using System.Collections.Generic;

namespace WeiBHTLibrary
{
    public class Behavior : IBehavior
    {
        //--------------------------------------------------
        public Action Initialize { protected get; set; }
        public Func<Status> Update { protected get; set; }
        public Action<Status> Terminate { protected get; set; }
        //--------------------------------------------------
        //--------------------------------------------------
        public Status Status { get; set; }
        public Composite Parent { get; set; }
        //--------------------------------------------------


        public Status Tick() {

            if (Status == Status.BhInvalid && Initialize != null) {
                Initialize();
            }

            Status = Update();

            //if (Status != Status.BhRunning && Terminate != null) {Terminate(Status);}

            return Status;
        }

        public virtual void Reset() {
            Status = Status.BhInvalid;
        }

    }
}
