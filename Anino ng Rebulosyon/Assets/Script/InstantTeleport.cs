using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class InstantTeleport : MonoBehaviour
{
    public enum TriggerType { OnKeyPress, OnCollision }

    [Header("Trigger Settings")]
    public TriggerType triggerType = TriggerType.OnKeyPress;
    public KeyCode interactKey = KeyCode.E;

    [Header("Teleport Settings")]
    public Transform teleportDestination;
    [Tooltip("Time (in seconds) before teleport happens.")]
    public float teleportDelay = 0f;

    [Header("Cutscene Settings (Optional)")]
    public bool playCutscene = false;
    public PlayableDirector timelineDirector;
    public bool playCutsceneOnce = false;

    [Header("Repeat Settings")]
    [Tooltip("If enabled, this can retrigger even while the player is still inside the trigger.")]
    public bool allowRetriggerInsideTrigger = false;

    private bool hasPlayedCutscene = false;

    private Transform player;
    private bool isTouching = false;
    private bool hasJustTeleported = false;
    private bool isProcessing = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("InstantTeleport: No GameObject with tag 'Player' was found.");
    }

    void Update()
    {
        // Optional: allow retrigger while still inside trigger
        if (allowRetriggerInsideTrigger && isTouching && !isProcessing)
        {
            hasJustTeleported = false;
        }

        if (triggerType == TriggerType.OnKeyPress && isTouching && !hasJustTeleported && !isProcessing)
        {
            if (Input.GetKeyDown(interactKey))
            {
                StartCoroutine(HandleTeleport());
            }
        }
    }

    private IEnumerator HandleTeleport()
    {
        if (isProcessing)
            yield break;

        isProcessing = true;
        hasJustTeleported = true;

        // Play cutscene immediately
        if (playCutscene && timelineDirector != null)
        {
            if (!playCutsceneOnce || !hasPlayedCutscene)
            {
                hasPlayedCutscene = true;
                timelineDirector.Play();
            }
        }

        // Wait before teleporting
        if (teleportDelay > 0f)
            yield return new WaitForSeconds(teleportDelay);

        TeleportPlayer();

        isProcessing = false;
    }

    private void TeleportPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("InstantTeleport: Player reference is missing.");
            return;
        }

        if (teleportDestination == null)
        {
            Debug.LogWarning("InstantTeleport: Teleport Destination is not assigned.");
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        player.position = teleportDestination.position;
        player.rotation = teleportDestination.rotation;

        if (cc != null)
            cc.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isTouching = true;

        if (triggerType == TriggerType.OnCollision && !hasJustTeleported && !isProcessing)
        {
            StartCoroutine(HandleTeleport());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isTouching = false;
        hasJustTeleported = false;
    }

    // Optional manual reset if you want to call this from a Retry button
    public void ResetTriggerState()
    {
        hasJustTeleported = false;
        isProcessing = false;
        isTouching = false;
    }

    // Optional manual reset for cutscene-once logic
    public void ResetCutsceneState()
    {
        hasPlayedCutscene = false;
    }
}