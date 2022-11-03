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
    private bool SanDeath = false;
    SanityManager sanity;
    UnityEngine.AI.NavMeshAgent agent;

    MeshRenderer Rend;
    // Start is called before the first frame update
    void Start()
    {
        Rend = GetComponent<MeshRenderer>();
        pointManager = GameObject.Find("WanderMarkers").GetComponent<WanderManager>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        priority = agent.avoidancePriority;
        CurrentRoom = pointManager.GetRoom();
        agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;
        sanity = GetComponent<SanityManager>();
        Tracker = GetComponentInParent<AITracker>();
    }

    // Update is called once per frame
    void Update()
    {
        //wander code
        roomTimer += Time.deltaTime;
        if (!agent.hasPath)
        {
            if(SanDeath) // temp death check remove once flee state is implemented
            {
                Tracker.IncreaseWinCount(GetComponent<GameObject>());
            }
            // Wait in place Code for idling
            StayTime += Time.deltaTime;
            agent.avoidancePriority -= 1;
            if (StayTime > 3)
            {
                agent.speed = Random.Range(3, 7);
                agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;
                agent.avoidancePriority += 1;
                StayTime = 0;
            }
            //
        }
        // Movement between rooms code. warning the sanity guage is still visible when transitioning between rooms and there are issues with stopping between frames causing
        // the guests to rocket into rooms.
        if(agent.isOnOffMeshLink)
        {
            Rend.enabled = false;
            agent.speed = 1000;
        }
        else
        {
            Rend.enabled = true;
            agent.speed = Random.Range(3, 7);
        }

        // sends the Guest to a different room after a length of time has passed
        if (roomTimer >= timeInRoom)
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
        if (sanity.sanityLevel == 0)
        {
            // flee state code
            agent.ResetPath();
            agent.speed = 18;
            agent.angularSpeed = 220;
            agent.SetDestination(Exit.position);
            SanDeath = true; // when the state machine is done remove this and simply have it's functions be carried out upon leaving the flee state.
        }
        else if (sanity.sanityLevel < 10 /* && state.ID == idle*/)
        {
            //enter flee state
        }
    }
}
