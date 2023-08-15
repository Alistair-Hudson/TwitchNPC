using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : State
{
    private string _action = "";

    public ActionState(StateMachine stateMachine, string action) : base(stateMachine)
    {
        _action = action;
    }

    public override void Enter()
    {
        (stateMachine as ChatStateMachine).Animator.CrossFadeInFixedTime(_action.ToLower(), 0.1f);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime((stateMachine as ChatStateMachine).Animator, "Action") >= 1)
        {
            stateMachine.SwitchState(new IdleState(stateMachine));
        }
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        var currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        var nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0;
        }
    }
}
