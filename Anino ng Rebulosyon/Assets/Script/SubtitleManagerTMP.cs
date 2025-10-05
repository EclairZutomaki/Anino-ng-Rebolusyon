using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManagerTMP : MonoBehaviour
{
    public static SubtitleManagerTMP Instance;
    [Header("TMP UI Reference")]
    public TextMeshProUGUI subtitleTMP;
    private Coroutine currentRoutine;

    private void Awake()
    {
        Instance = this;

        if (subtitleTMP != null)
            subtitleTMP.text = "";
    }

    public static void ShowSubtitle(string text, float duration)
    {
        if (Instance == null) return;

        if (Instance.currentRoutine != null)
            Instance.StopCoroutine(Instance.currentRoutine);

        Instance.currentRoutine = Instance.StartCoroutine(Instance.DisplaySubtitle(text, duration));
    }

    private IEnumerator DisplaySubtitle(string text, float duration)
    {
        subtitleTMP.text = text;
        yield return new WaitForSeconds(duration);
        subtitleTMP.text = "";
    }
}
