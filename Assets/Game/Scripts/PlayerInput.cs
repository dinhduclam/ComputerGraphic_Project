using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalInput;
    public float verticalInput;
    public bool MouseButtonDown;
    public bool SpaceKey;
    public bool Run;
    public bool Jump;

    // Update is called once per frame
    void Update()
    {
        if (!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }
        SpaceKey = Input.GetKey(KeyCode.Space);
        Run = Input.GetKey(KeyCode.LeftShift);
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        ClearCache();
    }

    public void ClearCache()
    {
        MouseButtonDown = false;
        SpaceKey = false;
        Run = false;
        HorizontalInput = 0;
        verticalInput = 0;
        
    }
}
