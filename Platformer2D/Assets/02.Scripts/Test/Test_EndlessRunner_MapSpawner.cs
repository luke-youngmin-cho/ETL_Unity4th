using Platformer.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EndlessRunner_MapSpawner : MonoBehaviour
{
    [SerializeField] private List<Test_EndlessRunner_MapTile> _tilePrefabs;
    [SerializeField] private int _initNum;

    private LinkedList<Test_EndlessRunner_MapTile> _tiles = new LinkedList<Test_EndlessRunner_MapTile>();

    private void Awake()
    {
        
    }

    private void SpawnRandomly()
    {
        Test_EndlessRunner_MapTile last = _tiles.Last.Value;
        Test_EndlessRunner_MapTile spawning = _tilePrefabs[Random.Range(0, _tilePrefabs.Count)];
        Test_EndlessRunner_MapTile tile =
            Instantiate(spawning,
                        last.transform.position + Vector3.right * (last.length / 2.0f + spawning.length / 2.0f),
                        Quaternion.identity);

        _tiles.AddLast(tile);
    }
}
