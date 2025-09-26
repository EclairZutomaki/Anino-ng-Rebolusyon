using UnityEngine;
using Cinemachine;

public class CameraTransitionManager : MonoBehaviour
{

    public CinemachineVirtualCamera currentCamera;

    void Start()
    {
        currentCamera.Priority++; // Increase the priority of the current camera to ensure it is active
    }

    // Update is called once per frame
    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--; // Decrease the priority of the current camera

        currentCamera = target; // Set the new camera as the current camera

        currentCamera.Priority++; // Increase the priority of the new camera to make it active
    }
}
