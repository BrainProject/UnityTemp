using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public static class MouseEmulator {
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(uint X, uint Y);
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out MousePoint pos);

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public uint X;
        public uint Y;

        public MousePoint(uint x, uint y)
        {
            X = x;
            Y = y;
        }

    }

    [Flags]
    public enum MouseEventFlags
    {
        None = 0x0,
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        MiddleDown = 0x00000020,
        MiddleUp = 0x00000040,
        Move = 0x00000001,
        Absolute = 0x00008000,
        RightDown = 0x00000008,
        RightUp = 0x00000010,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        HWheel = 0x01000
    }

//    public static uint None = 0x0;

    public static void SetCursorPosition(uint X, uint Y)
    {
        SetCursorPos(X, Y);
    }

    public static void SetCursorPosition(MousePoint point)
    {
        SetCursorPos(point.X, point.Y);
    }

    public static MousePoint GetCursorPosition()
    {
        MousePoint currentMousePoint;
        var gotPoint = GetCursorPos(out currentMousePoint);
        if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
        return currentMousePoint;
    }

    public static void MouseEvent(MouseEventFlags value)
    {
        MousePoint position = GetCursorPosition();

        mouse_event
            ((uint)value,
             position.X,
             position.Y,
             0,
             0)
            ;
    }
}
