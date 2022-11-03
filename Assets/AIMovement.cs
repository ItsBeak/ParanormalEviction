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
    public AIManager pointManager;

    UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        priority = agent.avoidancePriority;
        CurrentRoom = pointManager.GetRoom();
        agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;
    }

    // Update is called once per frame
    void Update()
    {


        roomTimer += Time.deltaTime;
        if (!agent.hasPath)
        {
            StayTime += Time.deltaTime;
            agent.avoidancePriority = 0;
            if (StayTime > 3)
            {
                agent.speed = Random.Range(3, 7);
                agent.destination = pointManager.RandPointInRoom(CurrentRoom, gameObject).position;
                agent.avoidancePriority = priority;
                StayTime = 0;
            }
        }

        if (roomTimer >= timeInRoom)
        {
            CurrentRoom = pointManager.GetRoom();
            roomTimer = 0;
        }

    }
}
