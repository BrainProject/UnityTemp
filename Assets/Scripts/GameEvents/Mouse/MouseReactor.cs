using UnityEngine;
using System.Collections;

public class MouseReactor : MonoBehaviour {
    public static int SubscriberId = (int)SubscriberIds.MouseController;

    // Use this for initialization
    void Start()
    {
        InteractionEventAggregator.OnMouse.Subscribe(SubscriberId, WorkWithMouse);
    }

    void WorkWithMouse(MouseGameEventArgs ev)
    {
        if (ev.Flags != MouseEmulator.MouseEventFlags.None)
        {
            MouseEmulator.MouseEvent(ev.Flags);
        }
        else
        {
            MouseEmulator.SetCursorPosition(ev.MousePoint);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
