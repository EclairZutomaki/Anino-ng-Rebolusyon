using UnityEngine;

public class ObjectToggleTrigger : MonoBehaviour
{
    public enum TriggerType { OnKeyPress, OnCollision }
    public enum ToggleAction { Show, Hide, Toggle }

    [System.Serializable]
    public class ToggleObject
    {
        public GameObject target;
        public ToggleAction action = ToggleAction.Toggle;
    }

    [Header("Trigger Settings")]
    public TriggerType triggerType = TriggerType.OnKeyPress;
    public KeyCode triggerKey = KeyCode.E;

    [Header("Objects to Control")]
    public ToggleObject[] objectsToToggle;

    [Header("Optional Settings")]
    public bool startHidden = false;
    public bool destroyAfterTrigger = false;

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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (triggerType == TriggerType.OnCollision)
                HandleToggle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void HandleToggle()
    {
        foreach (var obj in objectsToToggle)
        {
            if (obj.target == null) continue;

            switch (obj.action)
            {
                case ToggleAction.Show:
                    obj.target.SetActive(true);
                    break;

                case ToggleAction.Hide:
                    obj.target.SetActive(false);
                    break;

                case ToggleAction.Toggle:
                    obj.target.SetActive(!obj.target.activeSelf);
                    break;
            }
        }

        if (destroyAfterTrigger)
            Destroy(gameObject);
    }
}
