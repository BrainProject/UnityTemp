using Assets.Scripts.Utils.GameEvents;
using UnityEngine;
using System.Collections;

public class MovingGameEventArgs : GameEventArgs
{
    public MovementIds MovementId { get; set; }

    public MovingGameEventArgs(MovementIds mId)
    {
        MovementId = mId;
    }
}
