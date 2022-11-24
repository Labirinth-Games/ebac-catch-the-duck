using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Settings")]
    public List<AnimationStateType> animationList;

    private AnimationStateType _actualAnimation = new AnimationStateType(AnimationType.Idle);

    public void SetState(AnimationType type)
    {
        _actualAnimation = getTriggerByType(type);
        animator.SetTrigger(_actualAnimation.trigger);
    }
    
    public void SetState(AnimationType type, float value)
    {
        _actualAnimation = getTriggerByType(type);
        animator.SetFloat(_actualAnimation.trigger, value);
    }

    public void SetState(AnimationType type, bool boolean)
    {
        var newAnimation = getTriggerByType(type);

        newAnimation.boolean = boolean;
        _actualAnimation = newAnimation;
        animator.SetBool(newAnimation.trigger, newAnimation.boolean);
    }

    public bool IsCurrentAnimation(AnimationType type)
    {
        return _actualAnimation.type == type;
    }

    public bool IsCurrentAnimation(AnimationType type, bool boolean)
    {
        return _actualAnimation.type == type && _actualAnimation.boolean == boolean;
    }

    public AnimationStateType getTriggerByType(AnimationType type)
    {
        return animationList.Find(i => i.type == type);
    }

    public AnimationStateType getTriggerByName(string name)
    {
        return animationList.Find(i => i.trigger == name);
    }
}