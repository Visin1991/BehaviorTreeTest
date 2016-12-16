using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

namespace MyDelegate
{
    public static class DelegateManager
    {
        //**Create** a method for a delegate.
        public static void DelegateMethod(string message)
        {
            System.Console.WriteLine("DelegateManager DelegateMethod " + message);
        }

    }

    public class DelegateTest
    {
        //**declares** a delegate named Del that can encapsulate a method that takes a string as an argument and returns void:
        public delegate void Del(string message);

        // Instantiate the delegate.
        Del handler = DelegateManager.DelegateMethod;

        // Call the delegate directly
        public void TestFunc()
        {
            handler("Hello World");
        }

        // prepare a string and pass the string to another method. 
        public void MethodWithCallback(int param1, int param2, Del callback)
        {
            callback("The number is: " + (param1 + param2).ToString());
        }

        //pass the delegate as a parameter.
        public void Test2()
        {
            MethodWithCallback(1, 2, handler);
        }

    }

    public class MethodClass
    {
        public void Method1(string message)
        {
            System.Console.WriteLine("MethodClass Method1 " + message);
        }
        public void Method2(string message)
        {
            System.Console.WriteLine("MethodClass Method2 " + message);
        }
    }

    public class MathodClassTest
    {

        //**declares** a delegate named Del that can encapsulate a method that takes a string as an argument and returns void:
        public delegate void Del(string message);

        //error: A field initalizer cannot reference the nonostatic field, method or property...

        //when we dont put the code below to Test() function we will the error above

        //Because Del is a instance variable, Method1 is a instance method of a instance class
        //there is no guarantee that MethodClass will be initialized before MathodClassTest;

        void Test()
        {
            MethodClass obj = new MethodClass();
            Del d1 = obj.Method1;
            Del d2 = obj.Method2;
            Del d3 = DelegateManager.DelegateMethod;

            //Both types of assignment are valid.
            Del allMethodsDelegate;
            allMethodsDelegate = d1 + d2;

            //multicasting 
            allMethodsDelegate(" Called1...");
            //" MethodClass Method1 Called1..."
            //" MethodClass Method2 Called1..."

            allMethodsDelegate += d3;

            //multicasting
            allMethodsDelegate(" Called2....");
            //" MethodClass Method1 Called2..."
            //" MethodClass Method2 Called2..."
            //"DelegateManager DelegateMethod Called2..."

            //remove Method1
            allMethodsDelegate -= d1;
            allMethodsDelegate(" Called3....");
            //" MethodClass Method2 Called2..."
            //"DelegateManager DelegateMethod Called2..."

            int invocationCount = d1.GetInvocationList().GetLength(0);
        }


        //However we can use a static method of a static class to initialize a instance variable
        //because static class will initialize before a instance class
        Del d1 = MethodClass2.Method1;
        Del d2 = MethodClass2.Method2;
        Del d3 = DelegateManager.DelegateMethod;
    }

    public static class MethodClass2
    {
        public static void Method1(string message) { }
        public static void Method2(string message) { }
    }
}

namespace MyEvent_
{
    //=======================================
    namespace NormalType {
        // Define a class to hold custom event info
        public class CustomEventArgs : EventArgs
        {
            public CustomEventArgs(string s)
            {
                message = s;
            }
            private string message;

            public string Message
            {
                get { return message; }
                set { message = value; }
            }
        }

        // Class that publishes an event
        class Publisher
        {
            // Declare a delegate in your publishing class. 
            public delegate void CustomEventHandler(object sender, CustomEventArgs a);

            // declare your event inside your publishing class and use your delegate
            public event CustomEventHandler raiseCustomEvent;

            public void DoSomething()
            {
                // Write some code that does something useful here
                // then raise the event. You can also raise an event
                // before you execute a block of code.

                //Wrap raiseCustomEvent inside the function
                OnRaiseCustomEvent(new CustomEventArgs("Did something"));

            }

            // Wrap event invocations inside a protected virtual method
            // to allow derived classes to override the event invocation behavior
            protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
            {
                // Make a temporary copy of the event to avoid possibility of
                // a race condition if the last subscriber unsubscribes
                // immediately after the null check and before the event is raised.
                CustomEventHandler handler = raiseCustomEvent;

                // Event will be null if there are no subscribers
                if (handler != null)
                {
                    // Format the string to send inside the CustomEventArgs parameter
                    e.Message += String.Format(" at {0}", DateTime.Now.ToString());

                    // Use the () operator to raise the event.
                    handler(this, e);  //this will call the function of Subscriber.HandleCustomEvent(object sender,CustomoEventArgs e)
                }
            }
        }

        //Class that subscribes to an event
        class Subscriber
        {
            private string id;
            public Subscriber(string ID, Publisher pub)
            {
                id = ID;
                // Subscribe to the event using C# 2.0 syntax
                pub.raiseCustomEvent += HandleCustomEvent;
            }

            // Define what actions to take when the event is raised.
            void HandleCustomEvent(object sender, CustomEventArgs e)
            {
                Console.WriteLine(id + " received this message: {0}", e.Message);
            }
        }

        class Program
        {
            static void MyMain(string[] args)
            {
                Publisher pub = new Publisher();
                Subscriber sub1 = new Subscriber("sub1", pub);
                Subscriber sub2 = new Subscriber("sub2", pub);

                // Call the method that raises the event.
                pub.DoSomething();

                // Keep the console window open
                Console.WriteLine("Press Enter to close this window.");
                Console.ReadLine();

            }
        }
    }
    //=======================================

    //=======================================
    namespace GenericEventType
    {
        // Define a class to hold custom event info
        public class CustomEventArgs : EventArgs
        {
            public CustomEventArgs(string s)
            {
                message = s;
            }
            private string message;

            public string Message
            {
                get { return message; }
                set { message = value; }
            }
        }

        // Class that publishes an event
        class Publisher
        {

            // Declare the event using EventHandler<T>
            public event EventHandler<CustomEventArgs> raiseCustomEvent;

            public void DoSomething()
            {
                // Write some code that does something useful here
                // then raise the event. You can also raise an event
                // before you execute a block of code.
                OnRaiseCustomEvent(new CustomEventArgs("Did something"));

            }

            // Wrap event invocations inside a protected virtual method
            // to allow derived classes to override the event invocation behavior
            protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
            {
                // Make a temporary copy of the event to avoid possibility of
                // a race condition if the last subscriber unsubscribes
                // immediately after the null check and before the event is raised.
                EventHandler<CustomEventArgs> handler = raiseCustomEvent;

                // Event will be null if there are no subscribers
                if (handler != null)
                {
                    // Format the string to send inside the CustomEventArgs parameter
                    e.Message += String.Format(" at {0}", DateTime.Now.ToString());

                    // Use the () operator to raise the event.
                    handler(this, e);//this will call the function of Subscriber.HandleCustomEvent(object sender,CustomoEventArgs e)
                }
            }
        }

        //Class that subscribes to an event
        class Subscriber
        {
            private string id;
            public Subscriber(string ID, Publisher pub)
            {
                id = ID;
                // Subscribe to the event using C# 2.0 syntax
                pub.raiseCustomEvent += HandleCustomEvent;
            }

            // Define what actions to take when the event is raised.
            void HandleCustomEvent(object sender, CustomEventArgs e)
            {
                Console.WriteLine(id + " received this message: {0}", e.Message);
            }
        }

        class Program
        {
            static void MyMain(string[] args)
            {
                Publisher pub = new Publisher();
                Subscriber sub1 = new Subscriber("sub1", pub);
                Subscriber sub2 = new Subscriber("sub2", pub);

                // Call the method that raises the event.
                pub.DoSomething();

                // Keep the console window open
                Console.WriteLine("Press Enter to close this window.");
                Console.ReadLine();

            }
        }
    }
    //=======================================

    //=======================================
    //Raise Base Class Events in Derived Classes
    namespace BaseClassEvents
    {
        // Special EventArgs class to hold info about Shapes.
        public class ShapeEventArgs : EventArgs
        {
            private double newArea;

            public ShapeEventArgs(double d)
            {
                newArea = d;
            }
            public double NewArea
            {
                get { return newArea; }
            }
        }

        // Base class event publisher
        public abstract class Shape
        {
            protected double area;

            public double Area
            {
                get { return area; }
                set { area = value; }
            }
            // The event. Note that by using the generic EventHandler<T> event type
            // we do not need to declare a separate delegate type.
            public event EventHandler<ShapeEventArgs> ShapeChanged;

            public abstract void Draw();

            //The event-invoking method that derived classes can override.
            protected virtual void OnShapeChanged(ShapeEventArgs e)
            {
                // Make a temporary copy of the event to avoid possibility of
                // a race condition if the last subscriber unsubscribes
                // immediately after the null check and before the event is raised.
                EventHandler<ShapeEventArgs> handler = ShapeChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        public class Circle : Shape
        {
            private double radius;
            public Circle(double d)
            {
                radius = d;
                area = 3.14 * radius * radius;
            }
            public void Update(double d)
            {
                radius = d;
                area = 3.14 * radius * radius;
                OnShapeChanged(new ShapeEventArgs(area));
            }
            protected override void OnShapeChanged(ShapeEventArgs e)
            {
                // Do any circle-specific processing here.

                // Call the base class event invocation method.
                base.OnShapeChanged(e);
            }
            public override void Draw()
            {
                Console.WriteLine("Drawing a circle");
            }
        }

        public class Rectangle : Shape
        {
            private double length;
            private double width;
            public Rectangle(double length, double width)
            {
                this.length = length;
                this.width = width;
                area = length * width;
            }
            public void Update(double length, double width)
            {
                this.length = length;
                this.width = width;
                area = length * width;
                OnShapeChanged(new ShapeEventArgs(area));
            }
            protected override void OnShapeChanged(ShapeEventArgs e)
            {
                // Do any rectangle-specific processing here.

                // Call the base class event invocation method.
                base.OnShapeChanged(e);
            }
            public override void Draw()
            {
                Console.WriteLine("Drawing a rectangle");
            }

        }

        // Represents the surface on which the shapes are drawn
        // Subscribes to shape events so that it knows
        // when to redraw a shape.
        public class ShapeContainer
        {
            List<Shape> _list;

            public ShapeContainer()
            {
                _list = new List<Shape>();
            }

            public void AddShape(Shape s)
            {
                _list.Add(s);
                // Subscribe to the base class event.
                s.ShapeChanged += HandleShapeChanged;
            }

            // ...Other methods to draw, resize, etc.

            private void HandleShapeChanged(object sender, ShapeEventArgs e)
            {
                Shape s = (Shape)sender;

                // Diagnostic message for demonstration purposes.
                Console.WriteLine("Received event. Shape area is now {0}", e.NewArea);

                // Redraw the shape here.
                s.Draw();
            }
        }

        class Test
        {

            static void MyMain2(string[] args)
            {
                //Create the event publishers and subscriber
                Circle c1 = new Circle(54);
                Rectangle r1 = new Rectangle(12, 9);
                ShapeContainer sc = new ShapeContainer();

                // Add the shapes to the container.
                sc.AddShape(c1);
                sc.AddShape(r1);

                // Cause some events to be raised.
                c1.Update(57);
                r1.Update(7, 7);

                // Keep the console window open in debug mode.
                System.Console.WriteLine("Press any key to exit.");
                // System.Console.ReadKey();
            }
        }
    }
    /* Output:
            Received event. Shape area is now 10201.86
            Drawing a circle
            Received event. Shape area is now 49
            Drawing a rectangle
     */
    //=======================================

    namespace WrapTwoInterfaceEvents
    {
        public interface IDrawingObject
        {
            // Raise this event before drawing
            // the object.
            event EventHandler OnDraw;
        }

        public interface IShape
        {
            // Raise this event after drawing
            // the shape.
            event EventHandler OnDraw;
        }


        // Base class event publisher inherits two
        // interfaces, each with an OnDraw event
        public class Shape : IDrawingObject, IShape
        {
            // Create an event for each interface event
            event EventHandler PreDrawEvent;
            event EventHandler PostDrawEvent;

            object objectLock = new System.Object();

            // Explicit interface implementation required.
            // Associate IDrawingObject's event with
            // PreDrawEvent
            event EventHandler IDrawingObject.OnDraw
            {
                add
                {
                    lock (objectLock)
                    {
                        PreDrawEvent += value;
                    }
                }
                remove
                {
                    lock (objectLock)
                    {
                        PreDrawEvent -= value;
                    }
                }
            }
            // Explicit interface implementation required.
            // Associate IShape's event with
            // PostDrawEvent
            event EventHandler IShape.OnDraw
            {
                add
                {
                    lock (objectLock)
                    {
                        PostDrawEvent += value;
                    }
                }
                remove
                {
                    lock (objectLock)
                    {
                        PostDrawEvent -= value;
                    }
                }


            }

            // For the sake of simplicity this one method
            // implements both interfaces. 
            public void Draw()
            {
                // Raise IDrawingObject's event before the object is drawn.
                EventHandler handler = PreDrawEvent;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
                Console.WriteLine("Drawing a shape.");

                // RaiseIShape's event after the object is drawn.
                handler = PostDrawEvent;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
        }

        public class Subscriber1
        {
            // References the shape object as an IDrawingObject
            public Subscriber1(Shape shape)
            {
                IDrawingObject d = (IDrawingObject)shape;
                d.OnDraw += new EventHandler(d_OnDraw);
            }

            void d_OnDraw(object sender, EventArgs e)
            {
                Console.WriteLine("Sub1 receives the IDrawingObject event.");
            }
        }
        // References the shape object as an IShape
        public class Subscriber2
        {
            public Subscriber2(Shape shape)
            {
                IShape d = (IShape)shape;
                d.OnDraw += new EventHandler(d_OnDraw);
            }

            void d_OnDraw(object sender, EventArgs e)
            {
                Console.WriteLine("Sub2 receives the IShape event.");
            }
        }

        public class Program
        {
            static void MyMain3(string[] args)
            {
                Shape shape = new Shape();
                Subscriber1 sub = new Subscriber1(shape);
                Subscriber2 sub2 = new Subscriber2(shape);
                shape.Draw();

                // Keep the console window open in debug mode.
                System.Console.WriteLine("Press any key to exit.");
                //System.Console.ReadKey();
            }
        }
    }
    /* Output:
        Sub1 receives the IDrawingObject event.
        Drawing a shape.
        Sub2 receives the IShape event.
    */
    //=======================================

    //=======================================
    namespace ThreadSynchronization
    {
        class ThreadingExample
        {
            static AutoResetEvent autoEvent;

            static void DoWork()
            {
                Console.WriteLine("   worker thread started, now waiting on event...");
                autoEvent.WaitOne();
                Console.WriteLine("   worker thread reactivated, now exiting...");
            }

            static void MyMain4()
            {
                autoEvent = new AutoResetEvent(false);

                Console.WriteLine("main thread starting worker thread...");
                Thread t = new Thread(DoWork);
                t.Start();

                Console.WriteLine("main thread sleeping for 1 second...");
                Thread.Sleep(1000);

                Console.WriteLine("main thread signaling worker thread...");
                autoEvent.Set();
            }
        }
    }

    //=======================================
    namespace UseADictionaryToStoreEventInstances
    {
        public delegate void EventHandler1(int i);
        public delegate void EventHandler2(string s);

        public class PropertyEventsSample
        {
            private System.Collections.Generic.Dictionary<string, System.Delegate> eventTable;

            public PropertyEventsSample()
            {
                eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>();
                eventTable.Add("Event1", null);
                eventTable.Add("Event2", null);
            }

            public event EventHandler1 Event1
            {
                add
                {
                    lock (eventTable)
                    {
                        eventTable["Event1"] = (EventHandler1)eventTable["Event1"] + value;
                    }
                }
                remove
                {
                    lock (eventTable)
                    {
                        eventTable["Event1"] = (EventHandler1)eventTable["Event1"] - value;
                    }
                }
            }

            public event EventHandler2 Event2
            {
                add
                {
                    lock (eventTable)
                    {
                        eventTable["Event2"] = (EventHandler2)eventTable["Event2"] + value;
                    }
                }
                remove
                {
                    lock (eventTable)
                    {
                        eventTable["Event2"] = (EventHandler2)eventTable["Event2"] - value;
                    }
                }
            }

            internal void RaiseEvent1(int i)
            {
                EventHandler1 handler1;
                if (null != (handler1 = (EventHandler1)eventTable["Event1"]))
                {
                    handler1(i);
                }
            }

            internal void RaiseEvent2(string s)
            {
                EventHandler2 handler2;
                if (null != (handler2 = (EventHandler2)eventTable["Event2"]))
                {
                    handler2(s);
                }
            }
        }

        public class TestClass
        {
            public static void Delegate1Method(int i)
            {
                System.Console.WriteLine(i);
            }

            public static void Delegate2Method(string s)
            {
                System.Console.WriteLine(s);
            }

            static void MyMain5()
            {
                PropertyEventsSample p = new PropertyEventsSample();

                p.Event1 += new EventHandler1(TestClass.Delegate1Method);
                p.Event1 += new EventHandler1(TestClass.Delegate1Method);
                p.Event1 -= new EventHandler1(TestClass.Delegate1Method);
                p.RaiseEvent1(2);

                p.Event2 += new EventHandler2(TestClass.Delegate2Method);
                p.Event2 += new EventHandler2(TestClass.Delegate2Method);
                p.Event2 -= new EventHandler2(TestClass.Delegate2Method);
                p.RaiseEvent2("TestString");

                // Keep the console window open in debug mode.
                System.Console.WriteLine("Press any key to exit.");
                //System.Console.ReadKey();
            }
        }
        /* Output:
            2
            TestString
        */
    }


}

namespace sealedAVirtualFunction
{
    //-----------------------------------------------------------
    //In the following example, Z inherits from Y but Z cannot override the virtual function F that is declared in X and sealed in Y.
    class X
    {
        protected virtual void F() { Console.WriteLine("X.F"); }
        protected virtual void F2() { Console.WriteLine("X.F2"); }
    }
    class Y : X
    {
        sealed protected override void F() { Console.WriteLine("Y.F"); }
        protected override void F2() { Console.WriteLine("Y.F2"); }
    }
    class Z : Y
    {
        // Attempting to override F causes compiler error CS0239.
        // protected override void F() { Console.WriteLine("C.F"); }

        // Overriding F2 is allowed.
        protected override void F2() { Console.WriteLine("Z.F2"); }
    }
}