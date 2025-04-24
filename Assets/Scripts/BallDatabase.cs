using UnityEngine;

[CreateAssetMenu(fileName = "BallDatabase", menuName = "BallSystem/BallDatabase")]
public class BallDatabase : ScriptableObject
{
    public GameObject[] ballPrefabs; // Drag all ball prefabs here in inspector
}
