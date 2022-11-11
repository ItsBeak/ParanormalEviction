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
    
    }
    public override void ExecuteState(AIMovement Agent)
    {


    }
    public override void Exit(AIMovement Agent) 
    {

    }
}

public class IdleState : State
{
    public override void Enter(AIMovement Agent) 
    { 
    
    }
    public override void ExecuteState(AIMovement Agent)
    {


    }
    public override void Exit(AIMovement Agent) 
    {
    
    }
}

public class RunState : State
{
    public override void Enter(AIMovement Agent) 
    {
    
    }
    public override void ExecuteState(AIMovement Agent)
    {


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
    
    }
    public override void ExecuteState(AIMovement Agent)
    {


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
        if (Agent.IdleRunTimer > Agent.IdleRunTimerMax)
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
        if (Agent.Scared)
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
        if (Agent.sanity.sanityLevel <= 10)
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
        if (Agent.sanity.sanityLevel <= 10)
        {
            return true;
        }
        return false;
    }
}

/*
 Setup example

        State Idle = new IdleState();
        State Wander = new WanderState();
        Transition WToI = new WanderToIdleTrans(Idle);
        FSM fSM = new FSM();
        fSM.AddTransition(Wander, WToI);
        fSM.SetCurrentState(Wander, Agent);
 */
