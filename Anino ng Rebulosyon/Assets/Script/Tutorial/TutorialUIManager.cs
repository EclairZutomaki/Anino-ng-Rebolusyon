using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject lookAroundUI;
    public GameObject moveUI;
    public GameObject runUI; // 🆕 UI for Shift to Run

    [Header("Look Settings")]
    public float lookAngleThreshold = 10f; // degrees to rotate before tutorial ends

    [Header("Move Settings")]
    public float moveTimer = 3f;

    [Header("Run Settings")]
    public float runPressHideTime = 3f; // 🕒 how long before hiding after pressing Shift
    public float runAutoHideTime = 5f;  // 🕒 how long before hiding if not pressed

    [Header("References")]
    public Transform cameraTarget; // assign the CinemachineCameraTarget here

    private bool lookTutorialDone = false;
    private bool moveTutorialDone = false;
    private bool runTutorialActive = false;

    private float currentTimer;
    private float initialYaw;
    private float initialPitch;

    private float runTimer = 0f;
    private bool shiftPressed = false;

    private enum TutorialState { Look, Move, Done }
    private TutorialState state = TutorialState.Look;

    void Start()
    {
        lookAroundUI.SetActive(true);
        moveUI.SetActive(false);
        if (runUI != null) runUI.SetActive(false);

        // record starting rotation
        Vector3 euler = cameraTarget.rotation.eulerAngles;
        initialYaw = euler.y;
        initialPitch = euler.x;
    }

    void Update()
    {
        // 🧭 LOOK tutorial
        if (state == TutorialState.Look)
        {
            float yawDelta = Mathf.Abs(Mathf.DeltaAngle(initialYaw, cameraTarget.rotation.eulerAngles.y));
            float pitchDelta = Mathf.Abs(Mathf.DeltaAngle(initialPitch, cameraTarget.rotation.eulerAngles.x));
            float totalDelta = yawDelta + pitchDelta;

            if (totalDelta >= lookAngleThreshold)
            {
                lookTutorialDone = true;
                lookAroundUI.SetActive(false);
                moveUI.SetActive(true);
                state = TutorialState.Move;
            }
        }

        // 🚶 MOVE tutorial
        else if (state == TutorialState.Move)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                currentTimer += Time.deltaTime;
                if (currentTimer >= moveTimer)
                {
                    moveUI.SetActive(false);
                    state = TutorialState.Done;
                }
            }
        }

        // 🏃 RUN tutorial logic (triggered externally)
        if (runTutorialActive)
        {
            runTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                shiftPressed = true;
                runTimer = 0f; // reset timer for 3s hide delay
            }

            if (shiftPressed && runTimer >= runPressHideTime)
            {
                HideRunUI();
            }
            else if (!shiftPressed && runTimer >= runAutoHideTime)
            {
                HideRunUI();
            }
        }
    }

    // 🧩 Called by another trigger when player enters a run zone
    public void TriggerRunUI()
    {
        if (runUI == null) return;

        runUI.SetActive(true);
        runTutorialActive = true;
        runTimer = 0f;
        shiftPressed = false;
    }

    private void HideRunUI()
    {
        if (runUI == null) return;

        runUI.SetActive(false);
        runTutorialActive = false;
    }
}
