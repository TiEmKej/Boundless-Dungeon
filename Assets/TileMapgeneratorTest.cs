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
    public int minDistanceToRoom = 5;
    public int mapSize = 50;
    public TileBase floorTile;
    public Tilemap floorMap;
    public Vector2Int[,] roomsList;
    public Vector2Int[,] closestRoomPairs;
    public int roomsNumber;

    // Start is called before the first frame update
    void Start()
    {
        PickRoomPosition();
        FindCloseRooms();
        GenerateCoridors();
    }

    private void PickRoomPosition()
    {
        //Pick how many rooms will be crated
        roomsNumber = UnityEngine.Random.Range(minRooms, maxRooms);
        //Define how many rooms will be in array
        roomsList = new Vector2Int[roomsNumber + 1, 2];
        //How many rooms are created yet
        int roomsCounter = 0;
        //Location for first room
        Vector2Int roomCenterLocation = new Vector2Int(0,0);
        //Room dimensions
        Vector2Int roomDimension = new Vector2Int(5, 5);
        //Bool for overlap
        bool isOverlaping = true;
        //While script isn't ganerate all rooms...
        while (roomsCounter <= roomsNumber)
        {
            //If it's not first room
            if(roomsCounter != 0)
            {
                isOverlaping = true;
                //Repeat until all tiles where room should be are free
                while (isOverlaping)
                {
                    //Pick random position on the map
                    roomCenterLocation = new Vector2Int(UnityEngine.Random.Range(-mapSize, mapSize), UnityEngine.Random.Range(-mapSize, mapSize));
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
        for (int i = roomCentralLocation.x - ((roomDimension.x + minDistanceToRoom*2) / 2); i < roomCentralLocation.x + ((roomDimension.x + minDistanceToRoom * 2) / 2); i++)
        {
            for (int j = roomCentralLocation.y - ((roomDimension.y + minDistanceToRoom * 2) / 2); j < roomCentralLocation.y + ((roomDimension.y + minDistanceToRoom * 2) / 2); j++)
            {
                // Check if there is tile under cords
                if (floorMap.HasTile(new Vector3Int(i,j,0)))
                {
                    // if yes, generate new position
                    Debug.Log("OVERLAP");
                    return true;
                }
            }
        }
        // if no, place the room at the generated cords
        return false;
    }

    private void GenerateRoom(Vector2Int roomCentralLocation, Vector2Int roomDimension)
    {
        Debug.Log("Generating room");
        for (int i = roomCentralLocation.x - (roomDimension.x / 2); i < roomCentralLocation.x + (roomDimension.x / 2); i++)
        {
            for (int j = roomCentralLocation.y - (roomDimension.y / 2); j < roomCentralLocation.y + (roomDimension.y / 2); j++)
            {   
                // Set the floor in genereted position
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                floorMap.SetTile(tilePosition, floorTile);
            }
        }
    }

    private void FindCloseRooms()
    {
        // Create a set of closest rooms
        closestRoomPairs = new Vector2Int[roomsNumber + 1, 2];
        for (int i = 1; i < roomsNumber; i++)
        {
            double distance = double.PositiveInfinity;
            for (int j = 1; j < roomsNumber; j++)
            {
                // calculate distance from one room to everyother one by one
                double newDistance = Math.Sqrt(Math.Pow((roomsList[i - 1, 0].x - roomsList[j - 1, 0].x), 2) + Math.Pow((roomsList[i - 1, 0].y - roomsList[j - 1, 0].y), 2));
                // if it's calculating distance between the same room go to next step
                if (newDistance == 0)
                {
                    continue;
                }
                // if new distance is smaller than old, than set new closest room
                else if (newDistance < distance)
                {
                    distance = newDistance;
                    closestRoomPairs[i - 1, 0] = roomsList[i - 1, 0];
                    closestRoomPairs[i - 1, 1] = roomsList[j - 1, 0];
                }
            }
            Debug.Log(closestRoomPairs[i - 1, 0] + " | " + closestRoomPairs[i - 1, 1]);
        }
    }

    private void GenerateCoridors()
    {
        for (int i = 0; i <= roomsNumber+1; i++)
        {
            int x1 = closestRoomPairs[i, 0].x;
            int x2 = closestRoomPairs[i, 1].x;
            int y1 = closestRoomPairs[i, 0].y;
            int y2 = closestRoomPairs[i, 1].y;
            while (x1 != x2) 
            {
                floorMap.SetTile(new Vector3Int(x1,y1,0), floorTile);
                if(x1 < x2)
                {
                    x1++;
                }
                else
                {
                    x1--;
                }
            }
            while (y1 != y2)
            {
                floorMap.SetTile(new Vector3Int(x1, y1, 0), floorTile);
                if (y1 < y2)
                {
                    y1++;
                }
                else
                {
                    y1--;
                }
            }

        }
    }
}
