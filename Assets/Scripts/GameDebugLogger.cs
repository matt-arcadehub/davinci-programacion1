using UnityEngine;

public static class GameDebugLogger
{
    public static void Log(string gameObjectName, string message)
    {
        Debug.Log($"<b>[{gameObjectName}]</b>: {message}");
    }

    public static void LogSuccess(string gameObjectName, string message)
    {
        Debug.Log($"<color=#00FF7F><b>[{gameObjectName}]</b>: {message}</color>");
    }

    public static void LogDanger(string gameObjectName, string message)
    {
        Debug.Log($"<color=#FF3333><b>[{gameObjectName}]</b>: {message}</color>");
    }

    public static void LogInfo(string gameObjectName, string message)
    {
        Debug.Log($"<color=#00D2FF><b>[{gameObjectName}]</b>: {message}</color>");
    }

    public static void LogWarning(string gameObjectName, string message)
    {
        Debug.Log($"<color=#FFD700><b>[{gameObjectName}]</b>: {message}</color>");
    }
}
