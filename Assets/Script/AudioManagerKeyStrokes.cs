using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManagerKeyStrokes : MonoBehaviour
{
   public StudioEventEmitter upEmitter, downEmitter, leftEmitter, rightEmitter;
    private void Start()
    {
        
    }


    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
          leftEmitter.Play();
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            rightEmitter.Play();
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            downEmitter.Play();
        }
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            upEmitter.Play();
        }
    }
}
