using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Spawner Stat")]
public class SpawnerStat : ScriptableObject
{
    public float spawnRate;
    public float spawnScale;

    public int maxItemsFromSpace;
}
