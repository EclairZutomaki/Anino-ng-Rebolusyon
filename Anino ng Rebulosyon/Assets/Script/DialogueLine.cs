using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Header("Speaker")]
    public string speakerName;
    public Color speakerNameColor = Color.white;

    [Header("Dialogue")]
    [TextArea(2, 4)]
    public string lineText;

    [Header("Voice")]
    public AudioClip voiceClip;
}