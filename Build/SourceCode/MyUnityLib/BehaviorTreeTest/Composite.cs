using System;
using System.Collections.Generic;

namespace WeiBHTLibrary
{
    //Composite 综合 
    public abstract class Composite : Behavior
    {
        protected List<Behavior> Children { get; set; }
        protected Composite() {
            Children = new List<Behavior>();
            Initialize = () => { }; 
            Terminate = Status => { }; 
            Update = () => Status.BhRunning;
        }
        public Behavior GetChild(int index) {
            return Children[index];
        }
        public int ChildCount {
            get { return Children.Count; }
        }
        public void Add(Composite composite) {
            Children.Add(composite);
        }
        public T Add<T>() where T: Behavior,new()
        {
            var t = new T { Parent = this};
            Children.Add(t);
            return t;
        }

        public override void Reset()
        {
            Status = Status.BhInvalid;
            foreach (var behavior in Children) {
                behavior.Reset();
            }
        }
    }
}
