using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] Transform objContainer;
    [SerializeField] GameObject spawnObj;

    private void Start()
    {
        SpawnAtPoint(spawnObj, points[0]);
        SpawnAtPoint(spawnObj, points[1]);
    }

    public void SpawnAtPoint(GameObject obj, Transform spawnPoint)
    {
        GameObject temp = GameObject.Instantiate(obj, spawnPoint.position, Quaternion.identity);
        temp.transform.parent = objContainer;
    }

}
