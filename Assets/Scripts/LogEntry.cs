using UnityEngine;

[System.Serializable]
public enum LogCategory
{
    Tutorial,
    Character_John,
    Character_Cynthia,
    Character_Steven
}

[System.Serializable]
public class LogEntry
{
    public string id;
    public string title;
    [TextArea(5, 20)]
    public string content;  // Use "[PAGE]" to separate pages
    public int pointsRequired;
    public bool unlocked = false;
    public LogCategory category;
}
