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
    private bool hasPlayedCutscene = false;

    private Transform player;
    private bool isTouching = false;
    private bool hasJustTeleported = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (triggerType == TriggerType.OnKeyPress && isTouching && !hasJustTeleported)
        {
            if (Input.GetKeyDown(interactKey))
                StartCoroutine(HandleTeleport());
        }
    }

    private IEnumerator HandleTeleport()
    {
        hasJustTeleported = true;

        // 🎬 Play the cutscene immediately
        if (playCutscene && timelineDirector != null)
        {
            if (!playCutsceneOnce || (playCutsceneOnce && !hasPlayedCutscene))
            {
                hasPlayedCutscene = true;
                timelineDirector.Play();
            }
        }

        // ⏳ Then wait before teleporting
        if (teleportDelay > 0f)
            yield return new WaitForSeconds(teleportDelay);

        TeleportPlayer();
    }

    private void TeleportPlayer()
    {
        if (player == null || teleportDestination == null) return;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.position = teleportDestination.position;
        player.rotation = teleportDestination.rotation;

        if (cc) cc.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        isTouching = true;

        if (triggerType == TriggerType.OnCollision && !hasJustTeleported)
            StartCoroutine(HandleTeleport());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        isTouching = false;
        hasJustTeleported = false;
    }
}
