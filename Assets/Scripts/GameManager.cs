using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject Grass;
    public GameObject Dirt;

    public Dictionary<Vector3Int, GameObject> Blocks = new Dictionary<Vector3Int, GameObject>();
    protected Vector3Int _initialWorldSize = new Vector3Int(3, 3, 3);
    protected int _playerSpawnGrassRange = 10;
    protected int _playerSpawnBelow = 3;
    protected int _blockSpawnTime = 5;
    private GameObject _player;

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartUp();
    }

    protected virtual void StartUp()
    {
        //Spawn top level grass blocks
        for (int x = -_initialWorldSize.x; x < _initialWorldSize.x; x++)
        {
            for (int z = -_initialWorldSize.z; z < _initialWorldSize.z; z++)
            {
                Blocks.Add(new Vector3Int(x, 0, z), Instantiate(Grass, new Vector3(x, 0, z), Quaternion.identity, transform));
            }
        }
        //Spawn below dirt blocks
        for (int x = -_initialWorldSize.x; x < _initialWorldSize.x; x++)
        {
            for (int y = -_initialWorldSize.y * 2; y < 0; y++)
            {
                for (int z = -_initialWorldSize.z; z < _initialWorldSize.z; z++)
                {
                    Blocks.Add(new Vector3Int(x, y, z), Instantiate(Dirt, new Vector3(x, y, z), Quaternion.identity, transform));
                }
            }
        }
        StartCoroutine(SpawnNewBlocks(_blockSpawnTime));
    }

    //Checks if blocks needs to be spawned within a certain range of the players position
    //Makes use of StartCoroutine() to create a method that is called once every x second
    protected virtual IEnumerator SpawnNewBlocks(float waitTime)
    {
        while (true)
        {
            //Translating the player coordinates to integer 
            Vector3Int playerLocation = Vector3Int.FloorToInt(_player.transform.position);

            for (int x = playerLocation.x - _playerSpawnGrassRange; x < playerLocation.x + _playerSpawnGrassRange; x++)
            {
                for (int z = playerLocation.z - _playerSpawnGrassRange; z < playerLocation.z + _playerSpawnGrassRange; z++)
                {
                    if (!Blocks.ContainsKey(new Vector3Int(x, 0, z)))
                    {
                        Blocks.Add(new Vector3Int(x, 0, z), Instantiate(Grass, new Vector3(x, 0, z), Quaternion.identity, transform));
                    }
                }
            }

            for (int x = playerLocation.x - _playerSpawnBelow; x < playerLocation.x + _playerSpawnBelow; x++)
            {
                for (int y = playerLocation.y - _playerSpawnBelow; y < 0; y++)
                {
                    for (int z = playerLocation.z - _playerSpawnBelow; z < playerLocation.z + _playerSpawnBelow; z++)
                    {
                        if (!Blocks.ContainsKey(new Vector3Int(x, y, z)))
                        {
                            Blocks.Add(new Vector3Int(x, y, z), Instantiate(Dirt, new Vector3(x, y, z), Quaternion.identity, transform));
                        }
                    }
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}