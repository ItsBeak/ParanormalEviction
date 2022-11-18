using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FSM
{
    State CurrentState;
    State NextState;

    Dictionary<State, List<Transition>> TransitionMap = new Dictionary<State, List<Transition>>();

    public void AddTransition(State Source, Transition Destination)
    {
        if (!TransitionMap.TryGetValue(Source, out List<Transition> transitions))
        {
            TransitionMap.Add(Source, new List<Transition>());
        }
        if (TransitionMap[Source].Contains(Destination))
        {
            return;
        }
        TransitionMap[Source].Add(Destination);
    }

    /// <summary>
    /// Sets the starting state you should not use this to change states leave it to the transitions unless you want a bug infestation.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="Agent"></param>
    public void SetCurrentState(State s, AIMovement Agent)
    {
        if (CurrentState != null)
            CurrentState.Exit(Agent);
        CurrentState = s;
        CurrentState.Enter(Agent);
        NextState = s;

    }

    /// <summary>
    /// Executes the current state then checks for valid transitions if one is found next state = transission state
    /// then if current state != next state exit current state current state = next state enter current (formerly next) state 
    /// </summary>
    /// <param name="Agent"></param>
    public void UpdateState(AIMovement Agent)
    {
        CurrentState.ExecuteState(Agent);
        List<Transition> Transitions;
        if (TransitionMap.TryGetValue(CurrentState, out Transitions))
        {
            foreach (var transition in Transitions)
            {
                if (transition.ChangeState(Agent))
                {
                    NextState = transition.GetNextState();
                    break;
                }
            }
            if (CurrentState != NextState)
            {
                CurrentState.Exit(Agent);
                CurrentState = NextState;
                CurrentState.Enter(Agent);
            }
        }
    }
}

public abstract class State
{

    public virtual void Enter(AIMovement Agent) { }
    public abstract void ExecuteState(AIMovement Agent);
    public virtual void Exit(AIMovement Agent) { }

}

public abstract class Transition
{
    State NextState;
    public Transition(State s)
    {
        NextState = s;
    }
    public State GetNextState() { return NextState; }
    public abstract bool ChangeState(AIMovement Agent);
}

public class WanderState : State
{
    bool Wait = false;
    public override void Enter(AIMovement Agent)
    {
        Agent.Scareable = true;
        Agent.agent.speed = Random.Range(Agent.MinSpeed, Agent.MaxSpeed);
        Agent.agent.angularSpeed = 360;
        Agent.agent.avoidancePriority = 5;
        Agent.animator.SetTrigger("Walking");
        Agent.agent.destination = Agent.pointManager.RandPointInRoom(Agent.CurrentRoom, Agent.gameObject).position;
        Debug.Log("Entering Wander", Agent.gameObject);
        Wait = false;
    }
    public override void ExecuteState(AIMovement Agent)
    {
        // Updates the room change timer
        Agent.roomTimer += Time.deltaTime;

        // If agent is wandering but isn't moving somewhere make it go somewhere.
        if (!Agent.agent.hasPath)
        {
            if (Wait)
            {
                Agent.agent.destination = Agent.pointManager.RandPointInRoom(Agent.CurrentRoom, Agent.gameObject).position;
                Wait = false;
            }
            else
                Wait = true;

        }

        // Code for moving across offmeshlinks between rooms
        if (Agent.agent.isOnOffMeshLink)
        {
            Agent.agent.CompleteOffMeshLink();
        }
        // sends the Guest to a different room after a length of time has passed
        if (Agent.roomTimer >= Agent.timeInRoom)
        {
            Agent.CurrentRoom = Agent.pointManager.GetRoom();
            Agent.roomTimer = 0;
        }


    }
    public override void Exit(AIMovement Agent)
    {

    }
}

public class IdleState : State
{
    public override void Enter(AIMovement Agent)
    {
        Agent.Idle = true;
        Agent.agent.avoidancePriority = 1;
        Agent.animator.SetTrigger("Idle");
        Debug.Log("Entering Idle", Agent.gameObject);
    }
    public override void ExecuteState(AIMovement Agent)
    {

        Agent.IdleRunTimer += Time.deltaTime;

    }
    public override void Exit(AIMovement Agent)
    {
        Agent.IdleRunTimer = 0;
        Agent.Idle = false;
    }
}

public class RunState : State
{
    int Start;
    public override void Enter(AIMovement Agent)
    {
        Agent.agent.avoidancePriority = 3;
        Agent.Idle = false;
        Agent.agent.speed += 5;
        Agent.agent.angularSpeed = 600;
        Agent.Scareable = false;
        Start = Agent.CurrentRoom;
        Agent.animator.SetTrigger("Scared");
    }
    public override void ExecuteState(AIMovement Agent)
    {
        // if the agent is not running return without executing the update code.
        if (!Agent.animator.GetCurrentAnimatorStateInfo(0).IsName("_Running"))
            return;


        //run timer increase
        Agent.IdleRunTimer += Time.deltaTime;
        if (Agent.Scared)
        {
            Start = Agent.CurrentRoom;
            Agent.Scared = false;
        }
        while (Agent.CurrentRoom == Start) { Agent.CurrentRoom = Agent.pointManager.GetRoom(); }
        if (!Agent.agent.hasPath)
        {
            Agent.agent.destination = Agent.pointManager.RandPointInRoom(Agent.CurrentRoom, Agent.gameObject).position;
        }
        // Movement between rooms
        if (Agent.agent.isOnOffMeshLink)
        {
            Agent.agent.CompleteOffMeshLink();
        }
    }
    public override void Exit(AIMovement Agent)
    {
        Agent.Scared = false;
        Agent.IdleRunTimer = 0;
    }
}
public class FleeState : State
{
    public override void Enter(AIMovement Agent)
    {
        Agent.animator.SetTrigger("Scared");
        Agent.Scareable = false;
        Agent.agent.ResetPath(); // line to try stopping the agent in place
        Agent.agent.angularSpeed = 720;
        Agent.agent.avoidancePriority = 2;
        Agent.agent.speed = Agent.FleeSpeed;
    }
    public override void ExecuteState(AIMovement Agent)
    {
        if (!Agent.animator.GetCurrentAnimatorStateInfo(0).IsName("_Running"))
            Agent.agent.SetDestination(Agent.Exit.position);
        if (Agent.agent.isOnOffMeshLink)
        {
            this.Exit(Agent);
        }
        else if (Vector3.Distance(Agent.agent.transform.position, Agent.Exit.position) <= 2)
        {
            this.Exit(Agent);
        }
    }
    public override void Exit(AIMovement Agent)
    {
        Agent.Tracker.IncreaseWinCount(Agent.GetComponent<GameObject>());
        GameObject.Destroy(Agent.gameObject);
    }
}

// Wander To Transitions _________________________________________________________

/// <summary>
/// 2nd priority transition for Wander leads to Idle
/// </summary>
public class WanderToIdleTrans : Transition
{
    public WanderToIdleTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {

        if (!Agent.agent.hasPath)
        {
            Debug.Log("WanderToIdleTriggered", Agent);
            return true;
        }
        return false;
    }
}

/// <summary>
/// 3rd priority transition for Wander leads to Run
/// </summary>
public class WanderToRunTrans : Transition
{
    public WanderToRunTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {

        if (Agent.Scared && Agent.sanity.sanityLevel <= 90)
        {
            Debug.Log("WanderToRunTriggered", Agent);
            return true;
        }
        return false;
    }
}

/// <summary>
/// 1st priority transition for Wander leads to Flee
/// </summary>
public class WanderToFleeTrans : Transition
{
    public WanderToFleeTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {

        if (Agent.sanity.sanityLevel <= 0)
        {
            Debug.Log("WanderToFleeTriggered", Agent);
            return true;
        }
        return false;
    }
}
//____________________________________________________________________________________


// Idle To Transitions _______________________________________________________________
/// <summary>
/// 2nd priority transition for Idle leads to Wander
/// </summary>
public class IdleToWanderTrans : Transition
{
    public IdleToWanderTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {

        if (Agent.Idle && Agent.IdleRunTimer >= Agent.IdleTimerMax)
        {
            Debug.Log("IdleToWanderTrans", Agent);
            return true;
        }
        return false;
    }
}


/// <summary>
/// 3rd priority transition for Idle leads to Run
/// </summary>
public class IdleToRunTrans : Transition
{
    public IdleToRunTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {

        if (Agent.Idle && Agent.Scared)
        {
            Debug.Log("IdleToRunTriggered", Agent);
            return true;
        }
        return false;
    }
}


/// <summary>
/// 1st priority transition for Idle leads to Flee
/// </summary>
public class IdleToFleeTrans : Transition
{
    public IdleToFleeTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {

        if (Agent.Idle && Agent.sanity.sanityLevel <= 10)
        {
            Debug.Log("IdleToFleeTriggered", Agent);
            return true;
        }
        return false;
    }
}
//______________________________________________________________________________


/// <summary>
/// Transition class for Run to Wander priority N/A
/// </summary>
public class RunToWanderTrans : Transition
{
    public RunToWanderTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {

        if (!Agent.Idle && Agent.IdleRunTimer > Agent.RunTimerMax)
        {
            Debug.Log("RunToWanderTriggered", Agent);
            return true;
        }
        return false;
    }
}

/*
 Setup example

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
 */
