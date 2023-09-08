using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; //Storing 2D array of instatinated rooms
    public int rows;
    public int cols; 
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    public Room[,] grid;
    public int mapSeed = 13; //defualt seed
    public enum RandomType { Seeded, Random, MapOfTheDay }
    public RandomType randomType = RandomType.MapOfTheDay;


    private void Start()
    {
    //GenerateMap();
    }

    public void ChangeRandomization(bool isMapOfDay) //Change randomiziation settings
    {
        if (isMapOfDay)
        {
            randomType = RandomType.MapOfTheDay;
        }
        else
        {
            randomType = RandomType.Random;
        }
    }

    public void ChangePlayerAmount(bool isMultiplayer) 
    {
    
    }

    //Returns random room from roomPrefabs
    public GameObject RandomRoomPrefab()
    {
        return roomPrefabs[UnityEngine.Random.Range(0,roomPrefabs.Length)];
    
    }

    public int DateToInt(DateTime dateToUse) 
    {
        //add up the current date on the system clock and return it
        return (dateToUse.Month * 1000000) + (dateToUse.Day * 10000) + dateToUse.Year;
    }

    public int DateAndTimeToInt(DateTime dateToUse) 
    {   //see above lmao
        return DateToInt(dateToUse) + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    public void GenerateMap()
    {
        switch (randomType) 
        {
            case RandomType.Random:
                mapSeed = DateAndTimeToInt(DateTime.Now);
                break;
            case RandomType.Seeded:
                break;
            case RandomType.MapOfTheDay:
                mapSeed = DateToInt(DateTime.Now.Date);
                break;
            default:
                Debug.LogWarning("unknown type of random map. whuhh??!");
                break;
        }
        UnityEngine.Random.InitState(mapSeed);
        // clear out the grid we had before (colum is x row is y)
        grid = new Room[cols, rows];

        // for each grid row that we have
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            // for each column in said grid row
            for (int currentCol = 0; currentCol < cols; currentCol++)
            {
                // figure out the location of the rooms
                float xPosition = roomWidth * currentCol;
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                // and then create a new grid at the location 
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // set the parent object
                tempRoomObj.transform.parent = this.transform;

                // name the room
                tempRoomObj.name = "Room_" + currentCol + "," + currentRow;

                // and get the room object 
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // and finally save it to the grid array
                grid[currentCol, currentRow] = tempRoom;

                if (currentRow == 0) 
                {
                    grid[currentCol, currentRow].doorSouth.SetActive(true);
                }
                if (currentRow == (rows - 1))
                {
                    grid[currentCol, currentRow].doorNorth.SetActive(true);
                }
                if (currentCol == 0)
                {
                    grid[currentCol, currentRow].doorWest.SetActive(true);
                }
                if (currentCol == (cols - 1))
                {
                    grid[currentCol, currentRow].doorEast.SetActive(true);
                }
            }
        }
    }
    // maybe need a  map generated inboke here?
    
    public void OpenDoors() 
    { 
        // pass 
    }

}

