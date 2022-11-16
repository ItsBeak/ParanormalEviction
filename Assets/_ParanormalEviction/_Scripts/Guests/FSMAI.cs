using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    State CurrentState;
    State NextState;

    Dictionary<State, List<Transition>> TransissionMap = new Dictionary<State, List<Transition>>();

    public void AddTransition(State Source, Transition Destination)
    {
        if (!TransissionMap.TryGetValue(Source, out List<Transition> transitions))
        {
            TransissionMap.Add(Source, new List<Transition>());
        }
        if (TransissionMap[Source].Contains(Destination))
        {
            return;
        }
        TransissionMap[Source].Add(Destination);
    }

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
        foreach (var transition in TransissionMap[CurrentState])
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
    public override void Enter(AIMovement Agent)
    {
        Agent.Scareable = true;
        Agent.Idle = false;
        Agent.agent.speed = Random.Range(Agent.MinSpeed, Agent.MaxSpeed);
        Agent.agent.angularSpeed = 120;
        Agent.agent.avoidancePriority = 5;

    }
    public override void ExecuteState(AIMovement Agent)
    {
        if (!Agent.agent.hasPath)
        {
            Agent.agent.destination = Agent.pointManager.RandPointInRoom(Agent.CurrentRoom, Agent.gameObject).position;

        }

        // sends the Guest to a different room after a length of time has passed
        Agent.roomTimer += Time.deltaTime;
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
        Agent.Scareable = true;
        Agent.Idle = true;
        Agent.agent.avoidancePriority = 1;
    }
    public override void ExecuteState(AIMovement Agent)
    {
        Agent.IdleRunTimer += Time.deltaTime;

    }
    public override void Exit(AIMovement Agent)
    {
        Agent.IdleRunTimer = 0;
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
        Agent.agent.angularSpeed = 240;
        Agent.Scareable = false;
        Start = Agent.CurrentRoom;
    }
    public override void ExecuteState(AIMovement Agent)
    {
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
    }
    public override void Exit(AIMovement Agent)
    {
        Agent.Scared = false;
    }
}
public class FleeState : State
{
    public override void Enter(AIMovement Agent)
    {
        Agent.Scareable = false;
        Agent.agent.speed = Agent.FleeSpeed;
        Agent.agent.angularSpeed = 360;
        Agent.agent.avoidancePriority = 2;
        Agent.agent.SetDestination(Agent.Exit.position);
    }
    public override void ExecuteState(AIMovement Agent)
    {
        if (Vector3.Distance(Agent.agent.transform.position, Agent.Exit.position) <= 2)
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

public class WanderToIdleTrans : Transition
{
    public WanderToIdleTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {
        Debug.Log("WanderToIdleTriggered", Agent);
        if (Random.Range(0, 10) <= 6)
        {
            return true;
        }
        return false;
    }
}

public class WanderToRunTrans : Transition
{
    public WanderToRunTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {
        Debug.Log("WanderToRunTriggered", Agent);
        if (Agent.Scared && Agent.sanity.sanityLevel <= 90)
        {
            return true;
        }
        return false;
    }
}

public class WanderToFleeTrans : Transition
{
    public WanderToFleeTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {
        Debug.Log("WanderToFleeTriggered", Agent);
        if (Agent.sanity.sanityLevel <= 0)
        {
            return true;
        }
        return false;
    }
}
//____________________________________________________________________________________


// Idle To Transitions _______________________________________________________________
public class IdleToWanderTrans : Transition
{
    public IdleToWanderTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {
        Debug.Log("IdleToWanderTrans", Agent);
        if (Agent.Idle && Agent.IdleRunTimer >= Agent.IdleRunTimerMax)
        {
            return true;
        }
        return false;
    }
}



public class IdleToRunTrans : Transition
{
    public IdleToRunTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {
        Debug.Log("IdleToRunTriggered", Agent);
        if (Agent.Idle && Agent.Scared)
        {
            return true;
        }
        return false;
    }
}



public class IdleToFleeTrans : Transition
{
    public IdleToFleeTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {
        Debug.Log("IdleToFleeTriggered", Agent);
        if (Agent.Idle && Agent.sanity.sanityLevel <= 10)
        {
            return true;
        }
        return false;
    }
}
//______________________________________________________________________________


/// <summary>
/// Transition class for Run to Wander
/// </summary>
public class RunToWanderTrans : Transition
{
    public RunToWanderTrans(State Destination) : base(Destination) { }
    public override bool ChangeState(AIMovement Agent)
    {
        Debug.Log("RunToWanderTriggered", Agent);
        if (!Agent.Idle && Agent.IdleRunTimer > Agent.IdleRunTimerMax)
        {
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
