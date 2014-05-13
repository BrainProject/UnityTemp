using Assets.Scripts.Utils.GameEvents;
using UnityEngine;
using System.Collections;

public class KeyboardEventArgs : GameEventArgs
{
    public KeyCode KeyCode { get; set; }

    public KeyboardEventArgs(KeyCode kc)
    {
        KeyCode = kc;
    }
}
