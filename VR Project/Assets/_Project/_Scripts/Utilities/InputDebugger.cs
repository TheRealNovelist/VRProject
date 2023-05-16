using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputDebugger : MonoBehaviour
{
    public List<InputDebuggerMember> members;

    private void Update()
    {
        foreach (var member in members)
        {
            member.Update();
        }
    }

    [System.Serializable]
    public class InputDebuggerMember
    {
        [SerializeField] private KeyCode key;

        [SerializeField] private UnityEvent OnKeyDown;
        [SerializeField] private UnityEvent OnKey;
        [SerializeField] private UnityEvent OnKeyUp;
    
        // Update is called once per frame
        public void Update()
        {
            if (Input.GetKeyDown(key)) OnKeyDown.Invoke();
            if (Input.GetKey(key)) OnKey.Invoke();
            if (Input.GetKeyUp(key)) OnKeyUp.Invoke();
        }
    }
}
