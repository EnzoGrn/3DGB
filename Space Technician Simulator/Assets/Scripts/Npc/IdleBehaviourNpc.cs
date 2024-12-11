using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviourNpc : StateMachineBehaviour
{
    [SerializeField] private float _TimeBeforeAnimation = 0;
    [SerializeField] private float _TimeBeforeIdle = 0;
    [SerializeField] private bool _IsIdle = false;
    
    private readonly int _MaxIdle = 2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("IdleBehaviourNpc: OnStateEnter");
        ResetIdle(animator);

        int id = Random.Range(1, _MaxIdle + 1);

        Debug.Log("IdleAnimation -> " + id);
        animator.SetFloat("IdleAnimation", id);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("IdleBehaviourNpc: OnStateUpdate -> " + stateInfo.normalizedTime);
        //if (!_IsIdle)
        //{
        //    _TimeBeforeIdle += Time.deltaTime;
        //    if (_TimeBeforeIdle >= _TimeBeforeAnimation)
        //    {
        //        _IsIdle = true;

        //        int id = Random.Range(1, _MaxIdle + 1);

        //        Debug.Log("IdleAnimation -> " + id);
        //        animator.SetFloat("IdleAnimation", id);
        //    }
        //}
        // check stateInfo.normalizedTime to see if the animation is finished
        //else
        if (stateInfo.normalizedTime % 1 > 0.98)
        {
            Debug.Log("ResetIdle");
            ResetIdle(animator);
            animator.SetBool("TriggerIdleAnimation", false);
        }
    }

    private void ResetIdle(Animator animator)
    {
        _IsIdle = false;
        _TimeBeforeIdle = 0;

        animator.SetFloat("IdleAnimation", 0);
    }

    //private IEnumerator WaitForAnimationToFinish(Animator animator, int layerIndex)
    //{
    //    // Wait for the animation to finish
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
    //    float animationDuration = stateInfo.length;
    //    float remainingTime = animationDuration - (stateInfo.normalizedTime * animationDuration);

    //    Debug.Log($"Animation {animator.name} will finish in {remainingTime} seconds");

    //    yield return new WaitForSeconds(remainingTime);

    //    ResetIdle(animator);
    //    animator.SetBool("TriggerIdleAnimation", false);
    //    Debug.Log($"Animation {animator.name} finished!");
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
