using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    public GameObject lookAroundUI;
    public GameObject moveUI;

    public float lookAngleThreshold = 10f; // ✅ degrees to rotate before tutorial ends
    public float moveTimer = 3f;

    private bool lookTutorialDone = false;
    private bool moveTutorialDone = false;
    private float currentTimer;
    private float initialYaw;
    private float initialPitch;

    private enum TutorialState { Look, Move, Done }
    private TutorialState state = TutorialState.Look;

    public Transform cameraTarget; // assign the CinemachineCameraTarget here

    void Start()
    {
        lookAroundUI.SetActive(true);
        moveUI.SetActive(false);

        // record starting rotation
        Vector3 euler = cameraTarget.rotation.eulerAngles;
        initialYaw = euler.y;
        initialPitch = euler.x;
    }

    void Update()
    {
        if (state == TutorialState.Look)
        {
            float yawDelta = Mathf.Abs(Mathf.DeltaAngle(initialYaw, cameraTarget.rotation.eulerAngles.y));
            float pitchDelta = Mathf.Abs(Mathf.DeltaAngle(initialPitch, cameraTarget.rotation.eulerAngles.x));
            float totalDelta = yawDelta + pitchDelta;

            // ✅ Only trigger when player has really rotated camera
            if (totalDelta >= lookAngleThreshold)
            {
                lookTutorialDone = true;
                lookAroundUI.SetActive(false);
                moveUI.SetActive(true);
                state = TutorialState.Move;
            }
        }

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
    }
}
