using Assets.Scripts.Utils.GameEvents;
using UnityEngine;
using System.Collections;

public class MouseGameEventArgs : GameEventArgs
{
    public MouseEmulator.MouseEventFlags Flags { get; set; }
    public MouseEmulator.MousePoint MousePoint { get; set; }
    public MouseGameEventArgs(MouseEmulator.MouseEventFlags flags, MouseEmulator.MousePoint mousePoint)
    {
        Flags = flags;
        MousePoint = mousePoint;
    }
}
