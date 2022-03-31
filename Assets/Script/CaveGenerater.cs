using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveGenerater : MonoBehaviour
{
    //map
    bool[,] map;
    public int width;
    public int height;

    //Initialise Map
    public int generateChance;

    //CellularAutomata
    public int repeatTimes;
    public int deathLimit;
    public int birthLimit;

    //Map Check
    public bool isMapCheck;
    int[,] integerMap;
    int largestCave;
    List<int> caveList;
    public int cavePercentage;
    int retryTimes;

    //Generate Start & End Point
    Vector2 startPoint;
    Vector2 endPoint;

    //Map Generate
    public Tile[] tile = new Tile[5];
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCave();
    }
    public void GenerateCave()
    {
        bool successfulGenerate = false;
        retryTimes = 0;
        do
        {
            map = new bool[width, height];
            retryTimes++;
            InitialiseMap();

            CellularAutomata();

            if (CheckCaveSize())
                successfulGenerate = true;
            else
                successfulGenerate = false;

        } while (retryTimes < 5 && !successfulGenerate);

        if (successfulGenerate)
        {
            StartEndGeneration();
            GenerateMap();
        }
        else
        {
            Debug.LogWarning("Fail to generate, please check your variable setting and try again.");
        }
    }

    void InitialiseMap()
    {
        //each cell
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //based on the generateChance random spawn the cell
                if(Random.Range(0,100) < 100 - generateChance)
                    map[x,y] = true;
            }
        }
    }

    void CellularAutomata()
    {
        int count;

        for (int t = 0; t < repeatTimes; t++)
        {
            bool[,] tempMap = new bool[width, height];

            //each cell
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    count = 0;

                    //each nearby cell
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            //check it is current cell or not
                            if (i == 0 && j == 0)
                                break;
                            //cehck it is over array or not
                            else if (x + i < 0 || y + j < 0 || x + i >= width || y + j >= height)
                                count++;
                            //check it is live cell or not
                            else if (map[x + i, y + j] == true)
                                count++;
                        }
                    }

                    if (map[x, y])
                    {
                        //if the cell is around by more death cells it will be kill
                        if (count < deathLimit)
                            tempMap[x, y] = false;
                        else
                            tempMap[x, y] = true;
                    }
                    else
                    {
                        //if the cell is around by more live cells it will revive
                        if (count > birthLimit)
                            tempMap[x, y] = true;
                        else
                            tempMap[x, y] = false;
                    }
                }
            }

            map = tempMap;
        }
    }

    bool IsCave(int posX, int posY)
    {
        //check it is over the array or not
        if (posX < 0 || posX >= width || posY < 0 || posY >= height)
            return false;
        //check it is live cell & not be assign or not
        else if (integerMap[posX, posY] != 1)
            return false;

        return true;
    }

    bool CheckCaveSize()
    {
        int caveNo = 1;
        integerMap = new int[width, height];
        caveList = new List<int>();

        //Copy the map to integerMap
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    integerMap[x, y] = 0;
                else if (map[x, y])
                    integerMap[x, y] = 0; //0 is rock
                else
                    integerMap[x, y] = 1; //1 is path
            }
        }

        //each cell of the map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (integerMap[x, y] == 1)
                {
                    caveNo++;
                    caveList.Add(0);
                    Queue<Vector2> queue = new Queue<Vector2>();

                    //add the cell's position to the queue
                    queue.Enqueue(new Vector2(x, y));
                    integerMap[x, y] = caveNo; //change the cell to checked
                    caveList[caveNo - 2]++; //increase the size of the cave

                    do //do when the queue is not empty
                    {
                        Vector2 currentCell = queue.Dequeue();
                        int posX = (int)currentCell.x;
                        int posY = (int)currentCell.y;

                        //Check nearby cell is live cell or not
                        // TOP cell
                        if (IsCave(posX, posY - 1))
                        {
                            integerMap[posX, posY - 1] = caveNo; //change the cell to checked
                            caveList[caveNo - 2]++; //increase the size of the cave
                            queue.Enqueue(new Vector2(posX, posY - 1));
                        }
                            
                        // BOTTOM cell
                        if (IsCave(posX, posY + 1))
                        {
                            integerMap[posX, posY + 1] = caveNo; //change the cell to checked
                            caveList[caveNo - 2]++; //increase the size of the cave
                            queue.Enqueue(new Vector2(posX, posY + 1));
                        }
                            
                        // Left cell
                        if (IsCave(posX - 1, posY))
                        {
                            integerMap[posX - 1, posY] = caveNo; //change the cell to checked
                            caveList[caveNo - 2]++; //increase the size of the cave
                            queue.Enqueue(new Vector2(posX - 1, posY));
                        }
                            
                        // Right cell
                        if (IsCave(posX + 1, posY))
                        {
                            integerMap[posX + 1, posY] = caveNo; //change the cell to checked
                            caveList[caveNo - 2]++; //increase the size of the cave
                            queue.Enqueue(new Vector2(posX + 1, posY));
                        }
                            
                    } while (queue.Count != 0);
                }
            }
        }

        

        //check the largest cave
        for (int i = 0; i < caveList.Count; i++)
        {
            if (i == 0)
                largestCave = 0;
            else if (caveList[largestCave] < caveList[i])
                largestCave = i;
        }

        if(isMapCheck)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (integerMap[x, y] != largestCave + 2 && integerMap[x, y] != 0)
                    {
                        integerMap[x, y] = 0;
                    }
                }
            }
        }

        //check the cave size is match we want ot not
        if (caveList.Count == 0 || ((float)caveList[largestCave] / (width * height)) * 100 < cavePercentage)
            return false;

        return true;
    }

    void StartEndGeneration()
    {
        Vector2 vStartPoint = new Vector2(0, 0);
        Vector2 vEndPoint = new Vector2(0, 0);
        bool vStartFound = false;
        bool vEndFound = false;

        //find the vectical start point
        for (int x = width - 1; x >= 0; x--)
        {
            if (vStartFound)
                break;
            for (int y = 0; y < height; y++)
            {
                if (integerMap[x, y] == largestCave + 2)
                {
                    vStartPoint = new Vector2(x, y);
                    vStartFound = true;
                    break;
                }
            }
        }

        //find the vectical end point
        for (int x = 0; x < width; x++)
        {
            if (vEndFound)
                break;
            for (int y = height - 1; y >= 0; y--)
            {
                if (integerMap[x, y] == largestCave + 2)
                {
                    vEndPoint = new Vector2(x, y);
                    vEndFound = true;
                    break;
                }  
            }
        }

        Vector2 hStartPoint = new Vector2(0, 0);
        Vector2 hEndPoint = new Vector2(0, 0);
        bool hStartFound = false;
        bool hEndFound = false;

        //find the horizontal start point
        for (int y = height - 1; y >= 0; y--)
        {
            if (hStartFound)
                break;
            for  (int x = width - 1; x >= 0; x--)
            {
                if (integerMap[x, y] == largestCave + 2)
                {
                    hStartPoint = new Vector2(x, y);
                    hStartFound = true;
                    break;
                } 
            }
        }

        //find the horizontal end point
        for (int y = 0; y < height; y++)
        {
            if (hEndFound)
                break;
            for (int x = 0; x < width; x++) 
            {
                if (integerMap[x, y] == largestCave + 2 )
                {
                    hEndPoint = new Vector2(x, y);
                    hEndFound = true;
                    break;
                }       
            }
        }

        float vLenght = Vector2.Distance(vStartPoint, vEndPoint);
        float hLenght = Vector2.Distance(hStartPoint, hEndPoint);

        if (vLenght > hLenght)
        {
            startPoint = vStartPoint;
            endPoint = vEndPoint;
        }
        else
        {
            startPoint = hStartPoint;
            endPoint = hEndPoint;
        }
    }

    void GenerateMap()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (integerMap[x, y] == 0)
                {
                    if (y > 0 && integerMap[x, y - 1] != 0)
                    {
                        tilemap.SetTile(tilemap.WorldToCell(new Vector3(x, y, 0)), tile[2]);
                    }
                    else
                        tilemap.SetTile(tilemap.WorldToCell(new Vector3(x, y, 0)), tile[0]);
                }   
                else
                    tilemap.SetTile(tilemap.WorldToCell(new Vector3(x, y, 0)), tile[1]);
            }
        }
        tilemap.SetTile(tilemap.WorldToCell(new Vector3(startPoint.x, startPoint.y, 0)), tile[3]);
        tilemap.SetTile(tilemap.WorldToCell(new Vector3(endPoint.x, endPoint.y, 0)), tile[4]);
    }
}
