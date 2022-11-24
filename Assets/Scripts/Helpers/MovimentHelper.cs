using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MovimentHelper : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 1f;
    public float fastSpeed = 2f;
    public float jumpForce = 4f;

    private float _currentSpeed;

    public void MoveTo(GameObject target, Vector3 position)
    {
        position.y = 0; // non fly
        target.transform.position += position * _currentSpeed * Time.deltaTime;
    }

    public void Jump(GameObject target)
    {
        target.transform.DOMoveY(jumpForce, .2f);
    }

    public void SetFastSpeed(float fastSpeed)
    {
        this.fastSpeed = fastSpeed;

        _currentSpeed = fastSpeed;
    }
    public void SetFastSpeed()
    {
        _currentSpeed = fastSpeed;
    }
    
    public void SetSpeed(float speed)
    {
        this.speed = speed;

        _currentSpeed = speed;
    }
    
    public void SetSpeed()
    {
        _currentSpeed = speed;
    }
}


