using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Arrows : MonoBehaviour
{
    public Key assignedKey;  // gets set by ArrowSpawner

    public bool MatchesKey()
    {
        if (Keyboard.current == null) return false;

        var control = Keyboard.current[assignedKey];
        return control != null && control.wasPressedThisFrame;
    }
}