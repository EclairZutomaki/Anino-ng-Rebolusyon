using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static Vector3? LoadPosition = null;

    void Start()
    {
        if (LoadPosition != null)
        {
            transform.position = LoadPosition.Value;
            LoadPosition = null;
        }
    }
}
