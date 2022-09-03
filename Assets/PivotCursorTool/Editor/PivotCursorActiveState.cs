using System;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Kamgam.PivotCursorTool
{
    [InitializeOnLoad]
    public static class PivotCursorActiveState
    {
        public static bool IsActive = false;

        static PivotCursorActiveState()
        {
            bool active = false;

            // EditorTools available until Unity 2020.1 (2020.2+ does not longer have this class)
#if UNITY_2020_2_OR_NEWER
            ToolManager.activeToolChanged -= onToolChanged;
            ToolManager.activeToolChanged += onToolChanged;

            // active right from the start?
            if (ToolManager.activeToolType == typeof(PivotCursorTool))
            {
                // Remember: PivotCursorTool.Instance is still null here! 
                active = true;
                waitForInstance(active);
            }
#else
            EditorTools.activeToolChanged -= onToolChanged;
            EditorTools.activeToolChanged += onToolChanged;

            // active right from the start?
            if (EditorTools.activeToolType == typeof(PivotCursorTool))
            {
                // Remember: PivotCursorTool.Instance is still null here! 
                active = true;
                waitForInstance(active);
            }
#endif

            GlobalKeyEventHandler.OnKeyEvent -= onKeyEvent;
            GlobalKeyEventHandler.OnKeyEvent += onKeyEvent;
        }

        static void onKeyEvent(Event evt)
        {
            PivotCursorTool.HandleGlobalKey(evt);

            if (PivotCursorTool.Instance != null && IsActive)
                PivotCursorTool.Instance.HandleKeyPressEvents(evt);
        }

        static async void waitForInstance(bool active) 
        {
            float totalWaitTime = 0f; // precaution against endlessly running task
            while (PivotCursorTool.Instance == null && totalWaitTime < 3000)
            {
                await System.Threading.Tasks.Task.Delay(50);
                totalWaitTime += 50;
            }

            if (totalWaitTime >= 3000)
                return;

            IsActive = active;
            PivotCursorTool.Instance.OnToolChanged();
        }

        static void onToolChanged()
        {
#if UNITY_2020_2_OR_NEWER
            IsActive = ToolManager.activeToolType == typeof(PivotCursorTool);
#else
            IsActive = EditorTools.activeToolType == typeof(PivotCursorTool);
#endif

            if (PivotCursorTool.Instance != null)
                PivotCursorTool.Instance.OnToolChanged();
        }
    }
}
