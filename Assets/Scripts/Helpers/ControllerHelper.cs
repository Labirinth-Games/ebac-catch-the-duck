using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerHelper : MonoBehaviour
{
    [Header("References")]
    public MovimentHelper moviment;

    public void Move()
    {
        if (moviment == null)
        {
            Debug.LogError("Not exists MovimentHelper on attached");
            return; 
        }

        if (Input.GetKey(KeyCode.LeftShift))
            moviment.SetFastSpeed();
        else
            moviment.SetSpeed();

        var verticalPosition = Input.GetAxisRaw("Vertical");
        var horizontalPosition = Input.GetAxisRaw("Horizontal");

        Vector3 forward = verticalPosition * transform.forward;
        Vector3 strafe = horizontalPosition * transform.right;

        moviment.MoveTo(gameObject, forward + strafe);
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            moviment.Jump(gameObject);
    }

    #region --- Unity Events ---
    private void OnValidate()
    {
        if (moviment == null)
            moviment = GetComponent<MovimentHelper>();
    }
    #endregion
}



