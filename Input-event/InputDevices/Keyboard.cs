﻿using Input_Event.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Input_event.InputDevices
{
    public class Keyboard : InputDevice
    {
        public class InputEventArgs : InputEventArgs<InputEventCode>{}

        public event EventHandler<InputEventArgs> ButtonDownEvent;
        public event EventHandler<InputEventArgs> ButtonUpEvent;

        public enum InputEventCode
        {
            KEY_RESERVED = 0,
            KEY_ESC = 1,
            KEY_1 = 2,
            KEY_2 = 3,
            KEY_3 = 4,
            KEY_4 = 5,
            KEY_5 = 6,
            KEY_6 = 7,
            KEY_7 = 8,
            KEY_8 = 9,
            KEY_9 = 10,
            KEY_0 = 11,
            KEY_MINUS = 12,
            KEY_EQUAL = 13,
            KEY_BACKSPACE = 14,
            KEY_TAB = 15,
            KEY_Q = 16,
            KEY_W = 17,
            KEY_E = 18,
            KEY_R = 19,
            KEY_T = 20,
            KEY_Y = 21,
            KEY_U = 22,
            KEY_I = 23,
            KEY_O = 24,
            KEY_P = 25,
            KEY_LEFTBRACE = 26,
            KEY_RIGHTBRACE = 27,
            KEY_ENTER = 28,
            KEY_LEFTCTRL = 29,
            KEY_A = 30,
            KEY_S = 31,
            KEY_D = 32,
            KEY_F = 33,
            KEY_G = 34,
            KEY_H = 35,
            KEY_J = 36,
            KEY_K = 37,
            KEY_L = 38,
            KEY_SEMICOLON = 39,
            KEY_APOSTROPHE = 40,
            KEY_GRAVE = 41,
            KEY_LEFTSHIFT = 42,
            KEY_BACKSLASH = 43,
            KEY_Z = 44,
            KEY_X = 45,
            KEY_C = 46,
            KEY_V = 47,
            KEY_B = 48,
            KEY_N = 49,
            KEY_M = 50,
            KEY_COMMA = 51,
            KEY_DOT = 52,
            KEY_SLASH = 53,
            KEY_RIGHTSHIFT = 54,
            KEY_KPASTERISK = 55,
            KEY_LEFTALT = 56,
            KEY_SPACE = 57,
            KEY_CAPSLOCK = 58,
            KEY_F1 = 59,
            KEY_F2 = 60,
            KEY_F3 = 61,
            KEY_F4 = 62,
            KEY_F5 = 63,
            KEY_F6 = 64,
            KEY_F7 = 65,
            KEY_F8 = 66,
            KEY_F9 = 67,
            KEY_F10 = 68,
            KEY_NUMLOCK = 69,
            KEY_SCROLLLOCK = 70,
            KEY_KP7 = 71,
            KEY_KP8 = 72,
            KEY_KP9 = 73,
            KEY_KPMINUS = 74,
            KEY_KP4 = 75,
            KEY_KP5 = 76,
            KEY_KP6 = 77,
            KEY_KPPLUS = 78,
            KEY_KP1 = 79,
            KEY_KP2 = 80,
            KEY_KP3 = 81,
            KEY_KP0 = 82,
            KEY_KPDOT = 83
        }

        public Keyboard(InputEvent ie) : base(ie)
        {
            base._eventFilePath = ie.EventFilePath;
        }

        public override void DispatchEvent(ushort code, uint value)
        {
            InputEventCode keyCode = (InputEventCode)code;
            InputEventArgs e = new InputEventArgs()
            {
                Type = EventType.EV_KEY,
                Code = keyCode,
                Value = value
            };

            if (value == 1) OnButtonDown(e);
        }

        public virtual void OnButtonDown(InputEventArgs e)
        {
            ButtonDownEvent?.Invoke(this, e);
        }

        public virtual void OnButtonUp(InputEventArgs e)
        {
            ButtonUpEvent?.Invoke(this, e);
        }
    }
}
