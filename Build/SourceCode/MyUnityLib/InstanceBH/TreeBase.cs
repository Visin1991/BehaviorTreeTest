using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeBase : MonoBehaviour
{
    float halfwidth;
    float halfheight;
    //===========================================================================================================================
    /// <summary>
    /// QLL : Queue,List,List
    /// Use QLL design because it is much more  Easy to manipulate
    /// Add delete new behavior at runtime.
    /// </summary>
    BehaviorMainTaskQueue BMTQ;                              //Main Task
    ParallelTaskList PTL;                                    //ParallelTasks which will be remove from the list when the behavior success.
    public List<WeiBHTLibrary.Behavior> parallelRepetendBH;  //Contain all the repetend Behavior which need to be Tick every frame. Normal it is a passitive action
    //===========================================================================================================================
    /// <summary>
    /// Other Angent: like Physic Detection, PathFiding Angent, Still Angent...
    /// </summary>
    public AiUtility.FieldOfView fieldOfView;
    public NavMeshAgent navMeshAngent;
    public GameObject firePosition;
    public GameObject firePartical;
    [HideInInspector]
    public Destructable ds;
    public Transform[] partrolPos;
    public System.UInt64 numberFrame = 0;
    public Transform hidePos;

    [HideInInspector]
    public Vector3 searchPos;
    //===========================================================================================================================
    /// <summary>
    /// Flag Information for Behavior
    /// </summary>
    [HideInInspector]
    public List<string> viewTags;

    public enum AiStatu { Partoal, Fight, Sleep, Search, Hide }
    [HideInInspector]
    public AiStatu aiStatu = AiStatu.Fight;

    [HideInInspector]
    public GameObject player;

    public bool isGetHurt = false;
    public bool isFindPlayer = false;

    public struct BHInfo
    {
        public Vector3 currentDestination;
    }
    public BHInfo bhInfo;
    //===========================================================================================================================
    void Start()
    {
        halfwidth = Screen.width / 2;
        halfheight = Screen.height / 2;

        BMTQ = new BehaviorMainTaskQueue();
        PTL = new ParallelTaskList();
        parallelRepetendBH = new List<WeiBHTLibrary.Behavior>();

        fieldOfView = new AiUtility.FieldOfView(transform.GetChild(0), 10, 180);
        navMeshAngent = GetComponent<NavMeshAgent>();
        ds = GetComponent<Destructable>();

        parallelRepetendBH.Add(new LookAround(this));
        parallelRepetendBH.Add(new WalkAround(this));
        parallelRepetendBH.Add(new SenseAndEmotion(this));
        BMTQ.Add(new ToKillPlayer(this));
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10);
        foreach (WeiBHTLibrary.Behavior b in parallelRepetendBH)
        {
            b.Tick();
        }

        PTL.ListTick();

        BMTQ.QueueTick();
        numberFrame++;

        ds.hp += 0.01f;
        if (ds.hp > 100)
        {
            ds.hp = 100;
        }
    }

    //===========================================================================================================================
    public void Fire()
    {
        GameObject g = Instantiate(firePartical, firePosition.transform.position, Quaternion.identity);
        g.GetComponent<FireBall>().SetDirection(transform.forward);
    }

    public float GetCurrentHealth()
    {
        return ds.hp;
    }

    public float GetCurrentHealthPercentage()
    {
        return ds.hp / ds.maxHp;
    }

    public void SetNewDestination(Vector3 newPos)
    {
        if (bhInfo.currentDestination != newPos)
        {
            bhInfo.currentDestination = newPos;
            navMeshAngent.SetDestination(newPos);
        }
    }

    public bool IsArriveDestiniation()
    {
        return (navMeshAngent.remainingDistance <= navMeshAngent.stoppingDistance && !navMeshAngent.pathPending);
    }

    void OnGUI()
    {
        string healthInfo;
        if (GetCurrentHealth() >= 50)
        {
            healthInfo = "CurrentEnemy Health: " + GetCurrentHealth().ToString();
        }
        else
        {
            healthInfo = "Health Too Low, Try to escap" + GetCurrentHealth().ToString();
        }
        GUI.Label(new Rect(halfwidth, halfheight + 50, 200, 50), healthInfo);

    }
}


