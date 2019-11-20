using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int rowCount;
    public int lineCount;
    public float tileSize;

    public GameObject tilePrefab;
    public Transform originTransformDebug;

    public bool isRawBased;
    public bool isStartingBig;

    private GameObject[] tileGrid;

    private int tileCount;
    private Transform originTransform;

    private void Start()
    {
        tileCount = rowCount * (lineCount / 2) + (lineCount % 2) * (rowCount / 2 + rowCount % 2);
        tileGrid = new GameObject[tileCount];
        
        if(originTransformDebug)
        {
            originTransform = originTransformDebug;
        }
        else
        {
            originTransform.position = Vector3.zero;
            originTransform.rotation = Quaternion.identity;
        }

        InstanciateGrid();
    }

    private void InstanciateGrid()
    {
        for(int i = 0; i < tileCount; i++)
        {
            tileGrid[i] = Instantiate(tilePrefab) as GameObject;
            tileGrid[i].transform.parent = this.transform;
            tileGrid[i].transform.localPosition = GetTilePosition(i);
            tileGrid[i].transform.localRotation = originTransform.rotation;
        }
    }

    public void SetDynamicPathFinding(int goalTileID)
    {

    }

    public GameObject GetTile(int tileID)
    {
        return tileGrid[tileID];
    }

    public GameObject GetTile(int row, int line)
    {
        return tileGrid[GetTileID(row, line)];
    }

    public int GetTileID(int row, int  line)
    {
        return (line / 2) * (rowCount) + ((row + 1) % 2) * (row / 2) + (row % 2) * ((rowCount / 2) + (rowCount % 2) - 1 + ((row + 1) / 2));
    }

    //public GameObject GetTile(int x, int y)
    //{
    //    return;
    //}

    public Vector3 GetTilePosition(int tileID)
    {
        int tileLine = GetLine(tileID);
        int tileRow = GetRow(tileID);

        if (rowCount % 2 == 0)
        {
            if (lineCount % 2 == 0)
                return new Vector3((tileRow - rowCount / 2 + 1f/2f) * Mathf.Sqrt(3) * tileSize, originTransform.position.y, (lineCount / 2 - tileLine - 1f/2f) * tileSize);
            else
                return new Vector3((tileRow - rowCount / 2 + 1f/2f) * Mathf.Sqrt(3) * tileSize, originTransform.position.y, (lineCount / 2 - tileLine) * tileSize);
        }
        else
        {
            if (lineCount % 2 == 0)
                return new Vector3((tileRow - rowCount/2) * Mathf.Sqrt(3) * tileSize, originTransform.position.y, (lineCount / 2 - tileLine - 1f/2f) * tileSize); //(tileRow - rowCount / 2 + 1) * Mathf.Sqrt(3) * tileSize / 2f
            else
                return new Vector3((tileRow - rowCount/2) * Mathf.Sqrt(3) * tileSize, originTransform.position.y, (lineCount / 2 - tileLine) * tileSize);
        }
    }


    private int GetLine(int tileID)
    {
        if (tileID % rowCount > (rowCount / 2 + rowCount % 2 - 1))
            return 2 * (tileID / rowCount) + 1;  
        else
            return 2 * ((tileID + 1) / rowCount);
    }

    private int GetRow(int tileID)
    {
        if (tileID % rowCount > ((rowCount / 2) + (rowCount % 2) - 1))
            return 2 * (tileID % rowCount - (rowCount / 2 + rowCount % 2)) + 1;
        else
            return 2 * (tileID % rowCount);
    }

    public List<int> GetTileNeighbours(int tileID)
    {
        List<int> neighbours = new List<int>();

        int tileLine = GetLine(tileID);
        int tileRow = GetRow(tileID);

        if (tileLine > 1)
        {
            neighbours.Add(GetTileID(tileRow, tileLine - 1));
        }
        
        if(tileRow < rowCount - 1 )
        {
            if (tileLine > 0)
            {
                neighbours.Add(GetTileID(tileRow + 1, tileLine - 1));
            }
            if (tileLine < lineCount - 1)
            {
                neighbours.Add(GetTileID(tileRow + 1, tileLine + 1));
            }
        }

        if (tileLine < lineCount - 2)
        {
            neighbours.Add(GetTileID(tileRow, tileLine + 2));
        }

        if(tileRow > 0)
        {
            if(tileLine > 0)
            {
                neighbours.Add(GetTileID(tileRow - 1, tileLine - 1));
            }
            if(tileLine < lineCount - 1)
            {
                neighbours.Add(GetTileID(tileRow - 1, tileLine + 1));
            }
        }

        return neighbours;
    }

    // A Tester
    public List<List<GameObject>> GetTileInRange(int centerTileID, int range)
    {
        List<List<int>> tilesInRange = new List<List<int>>();
        List<int> discoveredTilesID = new List<int>();

        discoveredTilesID.Add(centerTileID);

        tilesInRange.Add(GetTileNeighbours(centerTileID));
        discoveredTilesID.AddRange(GetTileNeighbours(centerTileID)); // A mettre dans une variable?

        for(int i = 1; i < range; i++)
        {
            List<int> tilesInRangeI = new List<int>();
            
            foreach(int tileID in tilesInRange[tilesInRange.Count-1])
            {
                List<int> currentTileNeighbours = GetTileNeighbours(tileID);

                //Remove duplicate
                for(i = currentTileNeighbours.Count - 1; i >= 0; i--)
                {
                    foreach(int discoveredTile in discoveredTilesID)
                    {
                        if(discoveredTile == currentTileNeighbours[i])
                        {
                            currentTileNeighbours.RemoveAt(i);
                            break;
                        }
                    }
                }

                tilesInRangeI.AddRange(currentTileNeighbours);
                discoveredTilesID.AddRange(currentTileNeighbours);
            }

            tilesInRange.Add(tilesInRangeI);
        }

        return CastRangeList(tilesInRange);
    }

    private List<List<GameObject>> CastRangeList(List<List<int>> tileListIDs)
    {
        List<List<GameObject>> tileList = new List<List<GameObject>>();

        for(int i = 0; i < tileListIDs.Count; i++)
        {
            tileList.Add(IDListToGameObjectList(tileListIDs[i]));
        }

        return tileList;
    }

    private List<GameObject> IDListToGameObjectList(List<int> iDList)
    {
        List<GameObject> gameObjectList = new List<GameObject>();

        foreach(int iD in iDList)
        {
            gameObjectList.Add(GetTile(iD));
        }

        return gameObjectList;
    }
}
