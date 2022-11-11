using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public int CurrentRoom;
    int priority;
    private float roomTimer = 0;
    public float timeInRoom = 45;
    public float IdleRunTimer = 0;
    public float IdleRunTimerMax = 0;
    public Transform Exit;
    public WanderManager pointManager;
    public AITracker Tracker;
    public bool Scared = false;
    public bool Scareable;
    public bool Idle = false;
    public SanityManager sanity;
    UnityEngine.AI.NavMeshAgent agent;
    FSM fsm = new FSM();


    MeshRenderer Rend;
    public GameObject overheadUI;
    // Start is called before the first frame update
    private void Awake()
    {
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
        fsm.AddTransition(Idle, )
        fsm.SetCurrentState(Wander, this);
    }
    void Start()
    {
        Rend = GetComponentInChildren<MeshRenderer>();
        pointManager = GameObject.Find("WanderMarkers").GetComponent<WanderManager>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        priority = agent.avoidancePriority;
        CurrentRoom = pointManager.GetRoom();
        agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;
        sanity = GetComponent<SanityManager>();
        Tracker = GetComponentInParent<AITracker>();
        Exit = GameObject.Find("_ExitPoint").transform;
        Scareable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //wander code
        roomTimer += Time.deltaTime;
        if (!agent.hasPath && !SanDeath)// temp death check remove once flee state is implemented
        {
            

        }

        // Movement between rooms code.
        if (agent.isOnOffMeshLink)
        {
            agent.CompleteOffMeshLink();
            if (!SanDeath)
                agent.speed = Random.Range(3, 7);
        }


        // sends the Guest to a different room after a length of time has passed
        if (roomTimer >= timeInRoom && !SanDeath)
        {
            CurrentRoom = pointManager.GetRoom();
            roomTimer = 0;
        }

    }

    /// <summary>
    /// References the Sanity manager to decide the state of the guest.
    /// </summary>
    public void CheckSan()
    {
        Scared = true;
        if (sanity.sanityLevel <= 0 || sanity.sanityLevel < 10 && Idle == true)
        {
            // flee state code
            FleeState();
        }
        else if (sanity.sanityLevel <= 90 || Idle == true)
        {
            RunState();
            //enter run state
        }
    }

    public void FleeState()
    {
        Scareable = false;
        agent.speed = 14;
        agent.angularSpeed = 360;
        agent.SetDestination(Exit.position);
        SanDeath = true; // when the state machine is done remove this and simply have it's functions be carried out upon leaving the flee state.
    }

    public void RunState()
    {
        Idle = false;
        agent.speed += 5;
        agent.angularSpeed = 240;
        Scareable = false;
        CurrentRoom = pointManager.GetRoom();
        agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;

    }

    public void WanderState()
    {
        Scareable = true;
        Idle = false;
        agent.speed = Random.Range(3, 7);
        agent.angularSpeed = 120;
        agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;
        agent.avoidancePriority += 1;
        StayTime = 0;

    }

    public void IdleState()
    {
        if (!Idle)
        {
            Idle = true;
            agent.avoidancePriority -= 1;
        }
        StayTime += Time.deltaTime;

    }


}
