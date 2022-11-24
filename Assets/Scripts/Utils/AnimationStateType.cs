using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Jump,
    Walking,
    Idle,
    Gathering,
    Dead,
    Running,
    Attack,
    Falling
}

[Serializable]
public class AnimationStateType
{
    public AnimationType type;
    public string trigger;
    public bool boolean;

    public AnimationStateType(AnimationType type)
    {
        this.type = type;
    }
}
