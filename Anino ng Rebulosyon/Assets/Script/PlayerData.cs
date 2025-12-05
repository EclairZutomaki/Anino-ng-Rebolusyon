using System;

[Serializable]
public class PlayerData
{
    public float[] position;
    public string sceneName;

    public PlayerData(UnityEngine.Vector3 pos, string scene)
    {
        position = new float[3];
        position[0] = pos.x;
        position[1] = pos.y;
        position[2] = pos.z;

        sceneName = scene;
    }
}
