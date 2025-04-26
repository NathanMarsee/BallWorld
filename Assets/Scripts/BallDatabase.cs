using UnityEngine;

[CreateAssetMenu(fileName = "BallDatabase", menuName = "BallSystem/BallDatabase")]
public class BallDatabase : ScriptableObject
{
    public BallEntry[] balls;
}
