using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map")]
public class Map : ScriptableObject
{
    public Board board;
    public Vector2 gridOffset;
    public Vector2Int[] boxSpawn;
    public Vector2Int[] playerSpawn;
}
