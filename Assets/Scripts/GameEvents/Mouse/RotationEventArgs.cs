using Assets.Scripts.Utils.GameEvents;
using UnityEngine;
using System.Collections;

public class RotationEventArgs : GameEventArgs {
    public Vector3 V3Angles { get; set; }

    public RotationEventArgs(Vector3 v3)
    {
        V3Angles = v3;
    }
}
