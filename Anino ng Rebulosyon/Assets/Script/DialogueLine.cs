using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;      // Who’s talking
    [TextArea(2, 4)]
    public string lineText;         // Their line
    public AudioClip voiceClip;     // Optional voice
}
