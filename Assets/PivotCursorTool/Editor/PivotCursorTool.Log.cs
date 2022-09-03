using UnityEngine;

namespace Kamgam.PivotCursorTool
{
    partial class PivotCursorTool
    {
        public static void Log(string message, LogLevel logLevel)
        {
            var settings = PivotCursorToolSettings.GetOrCreateSettings();

            if ((int)logLevel < (int)settings.LogLevel)
                return;

            switch (logLevel)
            {
                case LogLevel.Log:
                    Debug.Log("Cursor Tool: " + message);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning("Cursor Tool: " + message);
                    break;
                case LogLevel.Error:
                    Debug.LogError("Cursor Tool: " + message);
                    break;
            }
        }

        public static void Log(string message)
        {
            Log(message, LogLevel.Log);
        }

        public static void LogWarning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public static void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }
    }
}
