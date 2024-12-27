
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum SpawnerState
{
    SPAWNING,
    WAITING
}
public class EnemySpawner : MonoBehaviour
{
    //Determines the size of spawnable grid
    //x pos of topLeft MUST be < x pos of bottom Right
    //y pos of topLeft MUST be > y pos of bottom Right
    [SerializeField] Transform topLeft;
    [SerializeField] Transform bottomRight;

    //List of all possible positions on the grid determined by the variables aboves
    private List<List<Vector3>> positions;

    //Parent obj for spawned items for hierarchy prettiness
    [SerializeField] Transform objContainer;

    [SerializeField] GameObject spawnObj;
    [SerializeField] List<GameObject> enemies;

    //Used for alligning enemies to the 16x16 grid positions
    [SerializeField] float offset = 0.5f;

    //Used for determining the amount of enemies present for determining when to spawn next wave
    [SerializeField] int numAlive = 0;

    //Used for determining the spawning of enemies
    [SerializeField] SpawnerState currState = SpawnerState.WAITING;
    [SerializeField] float waveDelay = 5f;

    [SerializeField] WavePattern[] patterns;
    public int currPat = 0;

    [SerializeField] bool on = false;

    private void Start()
    {
        if (on)
        {
            positions = new List<List<Vector3>>();
            GeneratePoints();
        }

    }

    private void Update()
    {
        if (on)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                //test function to kill random enemy from those alive
                if (enemies.Capacity > 0 && numAlive > 0)
                {
                    Destroy(enemies[0]);
                    enemies.RemoveAt(0);
                    numAlive--;
                }

            }

            if ((numAlive == 0 && currState == SpawnerState.WAITING) && (currPat >= 0 && currPat < patterns.Length))
            {
                currState = SpawnerState.SPAWNING;
                Debug.Log("Spawning new wave...");
                SpawnPattern(patterns[currPat]);
                StartCoroutine(StartDelay());
            }
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(waveDelay);
        currState = SpawnerState.WAITING;
        currPat++;
    }

    //generates spawn points based off the range of values
    public void GeneratePoints()
    {
        Vector3 TL = topLeft.position;
        Vector3 BR = bottomRight.position;

        for (int i = (int)TL.x; i < (int)BR.x; i++)
        {
            //Add new row for each position
            positions.Add(new List<Vector3>());
            for (int j = (int)TL.y; j > (int)BR.y; j--)
            {
                Vector3 newPoint = new Vector3(i, j, 0);

                //Add new vector 3
                positions[i-(int)TL.x].Add(newPoint);
            }
        }
    }

    public void SpawnPattern(WavePattern pat)
    {
        switch (pat.type)
        {
            case WaveType.CornerLine:
                CornerLine(pat);
                break;
            case WaveType.Random:
                RandomPattern(pat);
                break;

            default:
                break;
        }
    }

    private void RandomPattern(WavePattern pat)
    {
        
        for (int i = 0; i < pat.numToSpawn; i++)
        {
            if (pat.secondary != null)
            {
                int randomNum = Random.Range(0, 3);
                if (randomNum == 0)
                {
                    SpawnAtRandom(pat.secondary);
                }
                else
                {
                    SpawnAtRandom(pat.primary);
                }
            }
            else
            {
                SpawnAtRandom(pat.primary);
            }
        }
    }

    private void CornerLine(WavePattern pat)
    {
        for (int i = 0; i < pat.numToSpawn; i++)
        {
            GameObject objToSpawn = pat.secondary;
            if (pat.numToSpawn - 1 == i)
            {
                objToSpawn = pat.primary;
            }
            for (int j = 0; j <= 3; j++)
            {
                switch (j)
                {
                    case 0:
                        SpawnAtPoint(objToSpawn, i, i); //Top Left
                        break;
                    case 1:
                        SpawnAtPoint(objToSpawn, positions.Capacity-i-1, i); //Top Right
                        break;
                    case 2:
                        SpawnAtPoint(objToSpawn, i, positions[i].Capacity - i -1); //Bottom Left
                        break;
                    case 3:
                        SpawnAtPoint(objToSpawn, positions.Capacity - i - 1, positions[i].Capacity - i - 1); //Bottom Right
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void SpawnAtPoint(GameObject obj, int x, int y)
    {
        if (positions.Capacity > 0)
        {
            SpawnAtPoint(obj, positions[x][y]);
        }
    }

    public void SpawnAtRandom(GameObject obj)
    {
        if (positions.Capacity > 0)
        {
            SpawnAtPoint(obj, positions[GetRandomX()][GetRandomY()]);
        }
    }
    int GetRandomX()
    {
        return Random.Range(0, positions.Capacity);
    }
    int GetRandomY()
    {
        return Random.Range(0, positions[0].Capacity);
    }

    public void SpawnAtPoint(GameObject obj, Vector3 spawnPoint)
    {
        if (Mathf.Abs(offset) > 0)
        {
            spawnPoint.Set(spawnPoint.x + offset, spawnPoint.y - offset, 0);
        }
        GameObject temp = GameObject.Instantiate(obj, spawnPoint, Quaternion.identity);
        temp.transform.parent = objContainer;
        
        enemies.Add(temp);
        numAlive++;
    }

}
