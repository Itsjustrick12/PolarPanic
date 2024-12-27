using UnityEngine;
public enum WaveType
{
    Random,
    Corners,
    CornerLine,
    Middle

}
[CreateAssetMenu(fileName = "New Wave", menuName = "EnemyWave")]
public class WavePattern : ScriptableObject
{
    public int numToSpawn = 1;
    public WaveType type = WaveType.Random;
    public GameObject primary = null;
    public GameObject secondary = null;
    public bool spacing = false;
}
