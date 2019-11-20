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
        originTransform = originTransformDebug;

        InstanciateGrid();
    }

    private void InstanciateGrid()
    {
        for(int i = 0; i < tileCount; i++)
        {
            tileGrid[i] = Instantiate(tilePrefab) as GameObject;
            tileGrid[i].transform.parent = this.transform;
            tileGrid[i].transform.localPosition = GetTilePosition(i);
            tileGrid[i].transform.localRotation = Quaternion.identity;
        }
    }

    public Vector3 GetTilePosition(int tileID)
    {
        int tileLine = GetLine(tileID);
        int tileRow = GetRow(tileID);

        if (rowCount % 2 == 0)
        {
            if (lineCount % 2 == 0)
                return new Vector3((tileRow - rowCount / 2 + 1f/2f) * Mathf.Sqrt(3) * tileSize / 4f, originTransform.position.y, (lineCount / 2 - tileLine + 1f/2f) * tileSize / 2f);
            else
                return new Vector3((tileRow - rowCount / 2 + 1f/2f) * Mathf.Sqrt(3) * tileSize / 4f , originTransform.position.y, (lineCount / 2 - tileLine) * tileSize / 2f);
        }
        else
        {
            if (lineCount % 2 == 0)
                return new Vector3((tileRow - rowCount / 2 + 1) * Mathf.Sqrt(3) * tileSize / 2f, originTransform.position.y, (lineCount / 2 - tileLine + 1f/2f) * tileSize / 2f);
            else
                return new Vector3((tileRow - rowCount / 2 + 1) * Mathf.Sqrt(3) * tileSize / 2f, originTransform.position.y, (lineCount / 2 - tileLine) * tileSize / 2f);
        }
    }

    public GameObject GetTile(int tileID)
    {
        return tileGrid[tileID];
    }

    public GameObject GetTile(int row, int line)
    {
        return tileGrid[(line/2) * (rowCount) + ((row+1)%2) * (row/2 + row%2) + (row%2) * (rowCount/2 + rowCount%2 + row/2)];
    }

    //public GameObject GetTile(int x, int y)
    //{
    //    return;
    //}

    private int GetLine(int tileID)
    {
        if (tileID % rowCount > (rowCount / 2 + rowCount % 2))
            return 2 * tileID / rowCount + 1;
        else
            return 2 * tileID / rowCount;
    }

    private int GetRow(int tileID)
    {
        return tileID % rowCount;
    }

    public List<GameObject> GetTileNeighbours(int tileID)
    {
        List<GameObject> neighbours = new List<GameObject>();

        int tileLine = GetLine(tileID);
        int tileRow = GetRow(tileID);

        if (tileLine > 1)
        {
            neighbours.Add(GetTile(tileRow, tileLine - 1));
        }
        
        if(tileRow < rowCount - 1 )
        {
            if (tileLine > 0)
            {
                neighbours.Add(GetTile(tileRow + 1, tileLine - 1));
            }
            if (tileLine < lineCount - 1)
            {
                neighbours.Add(GetTile(tileRow + 1, tileLine + 1));
            }
        }

        if (tileLine < lineCount - 2)
        {
            neighbours.Add(GetTile(tileRow, tileLine + 2));
        }

        if(tileRow > 0)
        {
            if(tileLine > 0)
            {
                neighbours.Add(GetTile(tileRow - 1, tileLine - 1));
            }
            if(tileLine < lineCount - 1)
            {
                neighbours.Add(GetTile(tileRow - 1, tileLine + 1));
            }
        }

        return neighbours;
    }
}
