using UnityEngine;
using UnityEditor.ShortcutManagement;

namespace Kamgam.PivotCursorTool
{
    partial class PivotCursorTool
    {
        [Shortcut("Activate Cursor Tool", /*KEY*/KeyCode.U/*KEY*/, ShortcutModifiers.None)] // This line is changed by the settings.
        public static void PivotCursorToolShortcut()
        {
            Activate(null);
        }
    }
}
