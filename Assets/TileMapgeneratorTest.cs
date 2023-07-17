using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapgeneratorTest : MonoBehaviour
{
    public int maxRoomSize = 8;
    public int minRoomSize = 3;
    public int maxRooms = 10;
    public int minRooms = 5;
    public TileBase floorTile;
    public Tilemap floorMap;
    public Vector2Int[,] roomsList;
    public int 

    // Start is called before the first frame update
    void Start()
    {

        PickRoomPosition(); 
    }

    private void PickRoomPosition()
    {
        //Pick how many rooms will be crated
        int roomsNumber = UnityEngine.Random.Range(minRooms, maxRooms);
        //Define how many rooms will be in array
        roomsList = new Vector2Int[roomsNumber + 1, 2];
        //How many rooms are created yet
        int roomsCounter = 0;
        //Location for first room
        Vector2Int roomCenterLocation = new Vector2Int(0,0);
        //Room dimensions
        Vector2Int roomDimension = new Vector2Int(5, 5);
        //Bool for overlap
        bool isOverlaping = false;
        //While script isn't ganerate all rooms...
        while (roomsCounter <= roomsNumber)
        {
            //If it's not first room
            if(roomsCounter != 0)
            {
                //Repeat until all tiles where room should be are free
                while (!isOverlaping)
                {
                    //Pick random position on the map
                    roomCenterLocation = new Vector2Int(UnityEngine.Random.Range(-40, 40), UnityEngine.Random.Range(-40, 40));
                    Debug.Log("Location Picked");
                    //Save room dimension
                    roomDimension = new Vector2Int(UnityEngine.Random.Range(minRoomSize, maxRoomSize + 1), UnityEngine.Random.Range(minRoomSize, maxRoomSize + 1));
                    //Check if room is overlaping
                    isOverlaping = CheckRoomOverlap(roomCenterLocation, roomDimension);
                }
            }
            //Add center position and dimensions of the room
            Debug.Log("Adding " + roomsCounter + " room to list");
            roomsList[roomsCounter, 0] = roomCenterLocation;
            roomsList[roomsCounter, 1] = roomDimension;
            //Generate room on the map
            GenerateRoom(roomCenterLocation, roomDimension);
            //Append counter
            roomsCounter++;
        }
    }

    private bool CheckRoomOverlap(Vector2Int roomCentralLocation, Vector2Int roomDimension)
    {
        Debug.Log("Checking overlap");
        for (int i = roomCentralLocation.x - ((roomDimension.x + 2) / 2); i < roomCentralLocation.x + ((roomDimension.x + 2) / 2); i++)
        {
            for (int j = roomCentralLocation.y - ((roomDimension.y + 2) / 2); j < roomCentralLocation.y + ((roomDimension.y + 2) / 2); j++)
            {

                if (floorMap.HasTile(new Vector3Int(i,j,0)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void GenerateRoom(Vector2Int roomCentralLocation, Vector2Int roomDimension)
    {
        Debug.Log("Generating room");
        Debug.Log(roomCentralLocation);
        Debug.Log(roomDimension);
        for (int i = roomCentralLocation.x - (roomDimension.x / 2); i < roomCentralLocation.x + (roomDimension.x / 2); i++)
        {
            for (int j = roomCentralLocation.y - (roomDimension.y / 2); j < roomCentralLocation.y + (roomDimension.y / 2); j++)
            {    
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                Debug.Log(tilePosition);
                floorMap.SetTile(tilePosition, floorTile);
            }
        }
    }
}
