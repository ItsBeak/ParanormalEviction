using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public int CurrentRoom;
    int priority;
    private float roomTimer = 0;
    public float timeInRoom = 45;
    float StayTime = 0;
    public Transform Exit;
    public WanderManager pointManager;
    public AITracker Tracker;
    public bool SanDeath = false;
    public bool Scareable = true;
    public bool Idle = false;
    SanityManager sanity;
    UnityEngine.AI.NavMeshAgent agent;

    MeshRenderer Rend;
    public GameObject overheadUI;
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        CheckSan();

        //wander code
        roomTimer += Time.deltaTime;
        if (!agent.hasPath && !SanDeath)// temp death check remove once flee state is implemented
        {

            // Wait in place Code for idling
            IdleState();
            if (StayTime > 3)
            {
                WanderState();
            }
            //
        }
        // Movement between rooms code. warning the sanity guage is still visible when transitioning between rooms and there are issues with stopping between frames causing
        // the guests to rocket into rooms.

        if (agent.isOnOffMeshLink)
        {
            agent.CompleteOffMeshLink();
            if(!SanDeath)
            agent.speed = Random.Range(3, 7);
        }

        if (SanDeath && Vector3.Distance(agent.transform.position, Exit.position) <= 2)
        {
            Tracker.IncreaseWinCount(GetComponent<GameObject>());
            Destroy(this.gameObject);
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
        agent.speed += 5;
        agent.angularSpeed = 240;
        Scareable = false;
        pointManager.GetRoom();

    }

    public void WanderState()
    {
        Scareable = true;
        agent.speed = Random.Range(3, 7);
        agent.angularSpeed = 120;
        agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;
        agent.avoidancePriority += 1;
        StayTime = 0;
        Idle = false;
    }

    public void IdleState()
    {
        StayTime += Time.deltaTime;
        agent.avoidancePriority -= 1;
        Idle = true;
    }
}
