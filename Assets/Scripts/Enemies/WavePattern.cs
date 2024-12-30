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

    public static WavePattern CreateInstance(WaveType type, int numToSpawn, GameObject primary, GameObject secondary, bool spacing)
    {
        WavePattern o = CreateInstance<WavePattern>();
        o.numToSpawn = numToSpawn;
        o.type = type;
        o.primary = primary;
        o.secondary = secondary;
        o.spacing = spacing;
        return o;
    }
}
