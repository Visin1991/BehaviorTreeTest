using System.Collections;
using System.Collections.Generic;
using WeiBHTLibrary;

public class BehaviorMainTaskQueue{

    private Behavior currentMainTask;
    Queue<Behavior> mainTaskQueue;
    private bool hasTask;

    public BehaviorMainTaskQueue() {
        currentMainTask = null;
        mainTaskQueue = new Queue<Behavior>();
        hasTask = false;
    }

    public void QueueTick()
    {
        if (hasTask) {
            Status s = currentMainTask.Tick();
            if (s == Status.BhSuccess) {
                if (mainTaskQueue.Count > 0)
                {
                    currentMainTask = mainTaskQueue.Dequeue();
                    return;
                }
                else {
                    hasTask = false;
                }
            }
        }
    }

    public void Add(Behavior b) {
        if (!hasTask)
        {
            currentMainTask = b;
            hasTask = true;
        }
        else
            mainTaskQueue.Enqueue(b);
    }

    public void Clean() {
        mainTaskQueue.Clear();
    }
}

public class ParallelTaskList {
    private List<Behavior> parallenTaskelist;

    public ParallelTaskList() {
        parallenTaskelist = new List<Behavior>();
    }

    public void ListTick() {

       foreach (Behavior b in parallenTaskelist)
        {
            Status s = b.Tick();
            if (s == Status.BhSuccess) {
                parallenTaskelist.Remove(b);
            }
        }
    }

    public void Add(Behavior b) {
        parallenTaskelist.Add(b);
    }

    public void Clean() {
        parallenTaskelist.Clear();
    }
}
