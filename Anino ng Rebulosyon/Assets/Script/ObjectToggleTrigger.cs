using UnityEngine;
using System.Collections;

public class ObjectToggleTrigger : MonoBehaviour
{
    public enum TriggerType { OnKeyPress, OnCollision }
    public enum ToggleAction { Show, Hide, Toggle, Delete }

    [System.Serializable]
    public class ToggleObject
    {
        public GameObject target;
        public ToggleAction action = ToggleAction.Toggle;

        [Tooltip("Delay (in seconds) before showing this object. Only used if action = Show.")]
        public float showDelay = 0f;

        [Tooltip("Delay (in seconds) before hiding this object. Only used if action = Hide.")]
        public float hideDelay = 0f;
    }

    [Header("Trigger Settings")]
    public TriggerType triggerType = TriggerType.OnKeyPress;
    public KeyCode triggerKey = KeyCode.E;

    [Tooltip("List of tags allowed to trigger this. Example: Player, Draggable, etc.")]
    public string[] allowedTags = new string[] { "Player" };

    [Header("Objects to Control")]
    public ToggleObject[] objectsToToggle;

    [Header("Optional Settings")]
    public bool startHidden = false;

    [Tooltip("If true, this trigger object will be destroyed after activation.")]
    public bool destroyAfterTrigger = false;

    [Tooltip("Delay (in seconds) before destroying this object after trigger.")]
    public float destroyDelay = 0f;

    private bool playerInRange = false;

    void Start()
    {
        if (startHidden)
        {
            foreach (var obj in objectsToToggle)
                if (obj.target != null)
                    obj.target.SetActive(false);
        }
    }

    void Update()
    {
        if (triggerType == TriggerType.OnKeyPress && playerInRange && Input.GetKeyDown(triggerKey))
        {
            HandleToggle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsAllowedTag(other.tag))
        {
            playerInRange = true;

            if (triggerType == TriggerType.OnCollision)
                HandleToggle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsAllowedTag(other.tag))
            playerInRange = false;
    }

    private bool IsAllowedTag(string tagToCheck)
    {
        foreach (string tag in allowedTags)
        {
            if (tagToCheck == tag)
                return true;
        }
        return false;
    }

    private void HandleToggle()
    {
        foreach (var obj in objectsToToggle)
        {
            if (obj.target == null) continue;

            switch (obj.action)
            {
                case ToggleAction.Show:
                    if (obj.showDelay > 0f)
                        StartCoroutine(ShowWithDelay(obj.target, obj.showDelay));
                    else
                        obj.target.SetActive(true);
                    break;

                case ToggleAction.Hide:
                    if (obj.hideDelay > 0f)
                        StartCoroutine(HideWithDelay(obj.target, obj.hideDelay));
                    else
                        obj.target.SetActive(false);
                    break;

                case ToggleAction.Toggle:
                    obj.target.SetActive(!obj.target.activeSelf);
                    break;

                case ToggleAction.Delete:
                    Destroy(obj.target);
                    break;
            }
        }

        if (destroyAfterTrigger)
        {
            if (destroyDelay > 0f)
                Destroy(gameObject, destroyDelay);
            else
                Destroy(gameObject);
        }
    }

    private IEnumerator HideWithDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
            target.SetActive(false);
    }

    private IEnumerator ShowWithDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
            target.SetActive(true);
    }
}
