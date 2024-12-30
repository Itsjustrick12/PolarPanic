
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
enum SpawnerState
{
    SPAWNING,
    COOLDOWN,
    READY,
    STOPPED
}
public enum Difficulty
{
    Easy,
    Medium,
    Hard
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
    //[SerializeField] List<GameObject> enemies;

    //Used for alligning enemies to the 16x16 grid positions
    [SerializeField] float offset = 0.5f;

    //Used for determining the amount of enemies present for determining when to spawn next wave
    [SerializeField] int numAlive = 0;

    //Used for determining the spawning of enemies
    [SerializeField] SpawnerState currState = SpawnerState.COOLDOWN;
    [SerializeField] float waveDelay = 5f;
    public float timer = 0f;

    [SerializeField] List<WavePattern> patterns;

    //For Endless
    public GameObject[] basicEnemies;
    public GameObject[] advancedEnemies;

    [SerializeField] Difficulty currDifficulty = Difficulty.Easy;
    //Determines what wave number to increase difficulty to medium
    public int waveForMedium = 5;
    //Determine what wave number to increase to hard
    public int waveForHard = 10;

    private int currPat = 0;

    //endless
    public int wavesSpawned = 0;

    public static EnemySpawner instance;

    [SerializeField] bool on = false;
    [SerializeField] bool endless = true;

    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] SoundClip spawnerSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

            //Enter state to spawn next wave
            if ((numAlive == 0 && currState == SpawnerState.READY) && timer > waveDelay)
            {
                currState = SpawnerState.SPAWNING;
                soundPlayer.PlaySound(spawnerSound);
                SpawnPattern(patterns[0]);
                patterns.RemoveAt(0);

                if (currPat + 1 > patterns.Count - 1 && !endless)
                {

                    currState = SpawnerState.STOPPED;

                }
                else
                {
                    //Enter waiting state after wave spawn
                    timer = 0f;
                    currState = SpawnerState.COOLDOWN;
                    currPat++;
                }

                if (endless)
                {
                    currPat = 0;
                    wavesSpawned++;
                    if (wavesSpawned >= waveForHard)
                    {
                        currDifficulty = Difficulty.Hard;
                    }
                    else if (wavesSpawned >= waveForMedium)
                    {
                        currDifficulty = Difficulty.Medium;
                    }

                    if(patterns.Count == 0)
                    {
                        patterns.Add(GeneratePattern());
                    }
                }

            }
            else if (numAlive == 0 && currState == SpawnerState.STOPPED)
            {
                //GAME OVER STATE HERE
                Debug.Log("ALL WAVES CLEARED");
                on = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if ((numAlive == 0 && currState == SpawnerState.COOLDOWN) && on)
        {
            timer += Time.deltaTime;
            if (timer > waveDelay)
            {
                currState = SpawnerState.READY;
            }
        }
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
                positions[i - (int)TL.x].Add(newPoint);
            }
        }
    }

    public WavePattern GeneratePattern()
    {
        WaveType type = WaveType.Random;

        int amt = 1;

        int randomPattern;

        bool includeSpacing = false;

        if (Random.Range(0,3) == 2)
        {
            includeSpacing = true;
        }

        //Pick Random Primary and Secondary from the set supplied
        GameObject randomPrimary;
        GameObject randomSecondary;

        randomPattern = Random.Range(0, 4);
        switch (randomPattern)
        {
            case 0:

                type = WaveType.CornerLine;
                if (currDifficulty != Difficulty.Easy)
                {
                    if (currDifficulty == Difficulty.Medium)
                    {
                        amt = 2;
                    }
                    else if (currDifficulty == Difficulty.Hard){
                        amt = 3;
                    }

                    if (includeSpacing)
                    {
                        amt++;
                    }
                }

                break;
            case 1:

                type = WaveType.Random;
                if (currDifficulty == Difficulty.Easy)
                {
                    amt = 4;
                }
                else if (currDifficulty == Difficulty.Medium)
                {
                    amt = Random.Range(5, 8);
                }
                else
                {
                    amt = Random.Range(8, 13);
                }

                break;
            case 2:

                type = WaveType.Corners;;
                if (currDifficulty != Difficulty.Easy)
                {
                    if (currDifficulty == Difficulty.Medium)
                    {
                        amt = 2;
                    }
                    else if (currDifficulty == Difficulty.Hard)
                    {
                        amt = 3;
                    }

                    if (includeSpacing)
                    {
                        amt++;
                    }
                }

                break;
            case 3:
                type = WaveType.Middle;
                if (currDifficulty != Difficulty.Easy)
                {
                    amt = 2;
                }
                break;
            default:
                break;
        }

        if (currDifficulty == Difficulty.Easy)
        {
            randomPrimary = PickRandomEnemy(true);
            randomSecondary = PickRandomEnemy(true);
        }
        else if (currDifficulty == Difficulty.Medium)
        {
            randomPrimary = PickRandomEnemy(false);
            randomSecondary = PickRandomEnemy(true);
        }
        else
        {
            randomPrimary = PickRandomEnemy(false);
            randomSecondary = PickRandomEnemy(false);
        }


        WavePattern newPattern = ScriptableObject.CreateInstance<WavePattern>();
        newPattern = WavePattern.CreateInstance(type, amt, randomPrimary, randomSecondary, includeSpacing);
        return newPattern;

        
    }

    private GameObject PickRandomEnemy(bool basic)
    {
        if (!basic && advancedEnemies.Length > 0)
        {
            return advancedEnemies[Random.Range(0, advancedEnemies.Length)];
        }
        return basicEnemies[Random.Range(0, basicEnemies.Length)];
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
            case WaveType.Corners:
                CornerPattern(pat);
                break;
            case WaveType.Middle:
                MiddlePattern(pat);
                break;
            default:
                break;
        }
    }

    private void MiddlePattern(WavePattern pat)
    {

        GameObject objToSpawn = pat.primary;


        int midpoint = (positions.Count - 1) / 2;

        //Center
        SpawnAtPoint(objToSpawn, midpoint, midpoint);
        SpawnAtPoint(objToSpawn, midpoint + 1, midpoint);
        SpawnAtPoint(objToSpawn, midpoint, midpoint + 1);
        SpawnAtPoint(objToSpawn, midpoint + 1, midpoint + 1);

        objToSpawn = pat.secondary;

        if (pat.numToSpawn > 1)
        {
            //Top left
            SpawnAtPoint(objToSpawn, midpoint - 1, midpoint);
            SpawnAtPoint(objToSpawn, midpoint, midpoint - 1);


            //Top Right
            SpawnAtPoint(objToSpawn, midpoint + 2, midpoint);
            SpawnAtPoint(objToSpawn, midpoint + 1, midpoint - 1);

            //Bottom left
            SpawnAtPoint(objToSpawn, midpoint - 1, midpoint + 1);
            SpawnAtPoint(objToSpawn, midpoint, midpoint + 2);


            //Bottom Right
            SpawnAtPoint(objToSpawn, midpoint + 1, midpoint + 2);
            SpawnAtPoint(objToSpawn, midpoint + 2, midpoint + 1);
        }

    }

    private void CornerPattern(WavePattern pat)
    {
        bool localSpacing = false;
        for (int i = 0; i < pat.numToSpawn; i++)
        {
            if (pat.spacing && !localSpacing)
            {
                localSpacing = true;
            }
            else
            {
                localSpacing = false;
            }

            if (!localSpacing)
            {
                GameObject objToSpawn = pat.secondary;
                if (i == 0)
                {
                    objToSpawn = pat.primary;
                }

                if (i == 0)
                {
                    SpawnAtPoint(objToSpawn, 0, 0); //Top Left
                    SpawnAtPoint(objToSpawn, 0, positions[0].Count - 1); //Bottom Left
                    SpawnAtPoint(objToSpawn, positions.Count - 1, 0); //Top Right
                    SpawnAtPoint(objToSpawn, positions.Count - 1, positions[0].Count - 1); //Bottom Right
                }
                else
                {
                    SpawnAtPoint(objToSpawn, i, 0);
                    SpawnAtPoint(objToSpawn, 0, i);

                    SpawnAtPoint(objToSpawn, positions.Count - 1 - i, 0); //Top Right
                    SpawnAtPoint(objToSpawn, 0, positions[i].Count - 1 - i); //Top Right

                    SpawnAtPoint(objToSpawn, i, positions[i].Count - 1); //Bottom Left
                    SpawnAtPoint(objToSpawn, positions.Count - 1 - i, positions[i].Count - 1); //Bottom Left

                    SpawnAtPoint(objToSpawn, positions.Count - 1, i); //Bottom Right
                    SpawnAtPoint(objToSpawn, positions.Count - 1, positions[i].Count - 1 - i); //Bottom Right

                }

            }
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
        bool localSpacing = false;
        for (int i = 0; i < pat.numToSpawn; i++)
        {
            if (pat.spacing && !localSpacing)
            {
                localSpacing = true;
            }
            else
            {
                localSpacing = false;
            }

            if (!localSpacing)
            {
                GameObject objToSpawn = pat.secondary;
                if (pat.numToSpawn - 1 == i || (pat.numToSpawn - 2 == i && pat.spacing))
                {
                    objToSpawn = pat.primary;
                }

                SpawnAtPoint(objToSpawn, i, i); //Top Left
                SpawnAtPoint(objToSpawn, positions.Count - i - 1, i); //Top Right
                SpawnAtPoint(objToSpawn, i, positions[i].Count - i - 1); //Bottom Left
                SpawnAtPoint(objToSpawn, positions.Count - i - 1, positions[i].Count - i - 1); //Bottom Right

            }
        }
    }

    public void SpawnAtPoint(GameObject obj, int x, int y)
    {
        if (positions.Capacity > 0)
        {
            //Debug.Log("Attempting Spawning at X: " + x + ", " + y);
            SpawnAtPoint(obj, positions[x][y]);
        }
    }

    public void SpawnAtRandom(GameObject obj)
    {
        if (positions.Count > 0)
        {
            SpawnAtPoint(obj, positions[GetRandomX()][GetRandomY()]);
        }
    }
    int GetRandomX()
    {
        return Random.Range(0, positions.Count);
    }
    int GetRandomY()
    {
        return Random.Range(0, positions[0].Count);
    }

    public void SpawnAtPoint(GameObject obj, Vector3 spawnPoint)
    {
        if (Mathf.Abs(offset) > 0)
        {
            spawnPoint.Set(spawnPoint.x + offset, spawnPoint.y - offset, 0);
        }
        //GameObject temp = GameObject.Instantiate(obj, spawnPoint, Quaternion.identity);
        //temp.transform.parent = objContainer;

        //test for spawn circle
        GameObject spawnCircle = GameObject.Instantiate(spawnObj, spawnPoint, Quaternion.identity);
        spawnCircle.GetComponent<SpawnCircle>().SetObj(obj);
        numAlive++;
    }

    public void KillEnemy(int amt)
    {
        numAlive -= amt;
    }

}