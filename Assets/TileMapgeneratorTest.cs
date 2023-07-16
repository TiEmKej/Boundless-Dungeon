using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapgeneratorTest : MonoBehaviour
{
    public int maxRoomSize = 12;
    public int minRoomSize = 6;
    public TileBase floorTile;
    public Tilemap floorMap;
    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom(); 
    }

    private void GenerateRoom()
    {
        int roomWidth = UnityEngine.Random.Range(minRoomSize, maxRoomSize+1) ;
        int roomHeight = UnityEngine.Random.Range(minRoomSize, maxRoomSize + 1);
        for (int i = -roomWidth / 2; i < roomWidth / 2; i++)
        {
            for (int j = -roomHeight / 2; j < roomHeight / 2; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                floorMap.SetTile(tilePosition, floorTile);
            }
        }
        Debug.Log("Wielkoœæ pokoju: " + roomWidth + " x " + roomHeight);
    }
}
