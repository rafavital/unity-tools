//---------------------------------------------------
// <copyright file="InputControls.cs" company="Rafael Vital Lacerda Alves"
//       Author: Rafael Vital Lacerda Alves
//       Copyright (c) 2021 Rafael Vital Lacerda Alves
// </copyright>
//---------------------------------------------------

// adapted from https://forum.unity.com/threads/unity-5-keycode-selection-in-editor-window.949017/ 

using UnityEngine;
using UnityEditor;

public static class InputControls
{
    public static KeyCode KeyCodeField(Rect controlRect, KeyCode keyCode)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Keyboard);

        KeyCode retVal = keyCode;

        Event evt = Event.current;

        switch (evt.GetTypeForControl(controlID))
        {
            case EventType.Repaint:
                {
                    GUIStyle style = GUI.skin.GetStyle("TextField");
                    //GUIStyle style = EditorStyles.toolbar;

                    if (style == GUIStyle.none)
                    {
                        //Debug.Log("GUI Style not found");
                    }

                    if (GUIUtility.keyboardControl == controlID)
                    {
                        style.Draw(controlRect, new GUIContent("Press a key"), controlID);
                    }
                    else
                    {
                        style.Draw(controlRect, new GUIContent(keyCode.ToString()), controlID);
                    }
                    break;
                }
            case EventType.MouseDown:
                {
                    if (controlRect.Contains(Event.current.mousePosition) && Event.current.button == 0 && GUIUtility.hotControl == 0)
                    {
                        //Debug.Log("mouse down event, control id: " + controlID + ", hot control: " + GUIUtility.hotControl);
                        GUIUtility.hotControl = controlID;
                        GUIUtility.keyboardControl = controlID;
                        evt.Use();
                    }
                    break;
                }
            case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                        evt.Use();
                    }
                    break;

                }
            case EventType.KeyDown:
                {
                    //Debug.Log("key down, control id: " + controlID + ", hot control: " + GUIUtility.hotControl);
                    if (GUIUtility.keyboardControl == controlID)
                    {
                        //Debug.Log("hotcontrol");
                        retVal = Event.current.keyCode;
                        GUIUtility.hotControl = 0;
                        GUIUtility.keyboardControl = 0;
                        evt.Use();
                    }
                    break;
                }
            case EventType.KeyUp:
                {
                    break;
                }
        }
        return retVal;
    }

    public static KeyCode KeyCodeFieldLayout(KeyCode keyCode)
    {
        return KeyCodeField(EditorGUILayout.GetControlRect(), keyCode);
    }

}