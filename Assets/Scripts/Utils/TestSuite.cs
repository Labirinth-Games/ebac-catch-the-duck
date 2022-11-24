using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestSuite : MonoBehaviour
{
    public UnityEvent OnAction;
    public KeyCode key;

    [Header("Controller Move")]
    public bool activeMoviment = false;
    public float speed = 3f;

    [Header("Camera Look")]
    public GameObject camAttachment;
    public bool activeCameraMouse = false;
    public float sensibility = .2f;

    Cinemachine.CinemachineImpulseSource source;

    private float _mouseY = 0;
    private float _mouseX = 0;

    public void MovimentTest()
    {
        var verticalPosition = Input.GetAxis("Vertical");
        var horizontalPosition = Input.GetAxis("Horizontal");

        Vector3 forward = verticalPosition * transform.forward;
        Vector3 strafe = horizontalPosition * transform.right;
        var move = forward + strafe;

        if (move != Vector3.zero)
            gameObject.transform.position += move * speed * Time.deltaTime;

        if (activeCameraMouse)
        {
            _mouseY -= Input.GetAxis("Mouse Y");
            _mouseX += Input.GetAxis("Mouse X");

            camAttachment.transform.eulerAngles = new Vector3(_mouseY * sensibility, _mouseX * sensibility, 0);
            transform.eulerAngles = new Vector3(0, _mouseX * sensibility, 0);
        }
    }

    public void Fire()
    {
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        source.GenerateImpulse(Camera.main.transform.forward);
    }

    private void Update()
    {
        if (activeMoviment)
            MovimentTest();

        if (Input.GetKeyDown(key))
            OnAction?.Invoke();
    }
}
