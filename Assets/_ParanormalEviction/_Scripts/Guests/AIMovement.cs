using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    // Make some Get Set functions for some of these later__________________________________________________________________________!!!!!!
    public int CurrentRoom;
    public float roomTimer = 0;
    public float timeInRoom = 32;

    /// <summary>
    /// Dual use timer for both Idle and Run State Duration
    /// </summary>
    public float IdleRunTimer = 0;
    public float RunTimerMax = 10;
    public float IdleTimerMax = 4;
    public Transform Exit;
    public WanderManager pointManager;
    public AITracker Tracker;
    public bool Scared = false;
    public bool Scareable;
    public bool Idle = false;
    public SanityManager sanity;
    public UnityEngine.AI.NavMeshAgent agent;
    public float MaxSpeed = 7;
    public float MinSpeed = 2;
    public float FleeSpeed = 14;
    FSM fsm = new FSM();
    MeshRenderer Rend;
    public GameObject overheadUI;



    private void Awake()
    {       
        //setting up the Agent
        Rend = GetComponentInChildren<MeshRenderer>();    
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();


        //Setting up state Machine
        State Wander = new WanderState();
        State Idle = new IdleState();
        State Run = new RunState();
        State Flee = new FleeState();

        Transition WToI = new WanderToIdleTrans(Idle);
        Transition WToR = new WanderToRunTrans(Run);
        Transition WToF = new WanderToFleeTrans(Flee);
        Transition IToW = new IdleToWanderTrans(Wander);
        Transition IToR = new IdleToRunTrans(Run);
        Transition IToF = new IdleToFleeTrans(Flee);
        Transition RToW = new RunToWanderTrans(Wander);

        fsm.AddTransition(Wander, WToI);
        fsm.AddTransition(Wander, WToR);
        fsm.AddTransition(Wander, WToF);
        fsm.AddTransition(Idle, IToW);
        fsm.AddTransition(Idle, IToR);
        fsm.AddTransition(Idle, IToF);
        fsm.AddTransition(Run, RToW);
        fsm.SetCurrentState(Wander, this);
    }
    void Start()
    { 
        pointManager = GameObject.Find("WanderMarkers").GetComponent<WanderManager>();
        CurrentRoom = pointManager.GetRoom();
        agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;        
        sanity = GetComponent<SanityManager>();
        Tracker = GetComponentInParent<AITracker>();        
        Exit = GameObject.Find("_ExitPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Executes the current state then checks for valid transitions if one is found next state = transission state
        // then exit current state enter next state current state = next state
        fsm.UpdateState(this);
        
        // Movement between rooms code.
        if (agent.isOnOffMeshLink)
        {
            //Rend.enabled = false; This solution failed.
            agent.CompleteOffMeshLink();
            //Rend.enabled = true;
            agent.speed = Random.Range(MinSpeed, MaxSpeed);
        }


    }

    /// <summary>
    /// References the Sanity manager to decide the state of the guest.
    /// </summary>
    public void CheckSan()
    {
    }

}
