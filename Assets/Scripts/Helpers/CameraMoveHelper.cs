using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMoveHelper : Singleton<CameraMoveHelper>
{
    [Header("References")]
    public GameObject person;
    public GameObject camAttachmentPosition;

    [Header("Settings")]
    public bool lockMouse = false;
    public float sensibility = 2f;

    private float _mouseX = 0f;
    private float _mouseY = 0f;

    private bool _stopMoveCam = false;
    private bool _isPause = false;

    public void IsLockMouse()
    {
        if (!lockMouse) return;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Freezing()
    {
        _stopMoveCam = true;

        Cursor.visible = !Cursor.visible; ;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Unfreezing()
    {
        _stopMoveCam = false;

        IsLockMouse();
    }

    #region --- Unity Events ---
    private void Start()
    {
        IsLockMouse();
        GameManager.Instance?.OnPauseEvent.AddListener(OnPause);
        GameManager.Instance?.OnResumeEvent.AddListener(OnResume);
    }

    private void Update()
    {
        if (_stopMoveCam || _isPause)
            return;

        _mouseY -= Input.GetAxis("Mouse Y");
        _mouseX += Input.GetAxis("Mouse X");

        var eulerAngles = new Vector3(_mouseY * sensibility, _mouseX * sensibility, 0);

        camAttachmentPosition.transform.eulerAngles = eulerAngles;
        person.transform.eulerAngles = new Vector3(0, _mouseX * sensibility, 0);

        if (!Input.GetKey(KeyCode.LeftAlt))
            person.transform.eulerAngles = new Vector3(0, eulerAngles.y, 0);
    }
    #endregion

    #region Game Flow
    public void OnPause()
    {
        _isPause = true;
    }

    public void OnResume()
    {
        _isPause = false;
    }
    #endregion
}
