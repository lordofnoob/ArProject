using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }
        
        instance = this;
        InstanciateGrid();
    }

    public int rowCount;
    public int lineCount;
    public float tileSize;

    public GameObject tilePrefab;
    public Transform tileSetterTransform;

    public float unitScaleRatio = 1f;

    //public bool isRawBased;
    //public bool isStartingBig;

    private TileInfo[] tileGrid;

    private int tileCount;
    private Transform originTransform;

    private void Start()
    {
        tileCount = rowCount * (lineCount / 2) + (lineCount % 2) * (rowCount / 2 + rowCount % 2);
        tileGrid = new TileInfo[tileCount];
        
        originTransform = tileSetterTransform;
    }

    public void InstanciateGrid()
    {
        for(int i = 0; i < tileCount; i++)
        {
            GameObject newTile = Instantiate(tilePrefab) as GameObject;
            tileGrid[i] = newTile.GetComponent<TileInfo>();
            tileGrid[i].gameObject.transform.parent = instance.transform;
            tileGrid[i].gameObject.transform.localPosition = GetTilePosition(i);
            tileGrid[i].gameObject.transform.localRotation = originTransform.localRotation;
            tileGrid[i].tileID = i;
        }
    }

    public void SetTileGridTransform(Transform targetTransform)
    {
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }

    public void SetPathFinding(int goalTileID)
    {
        Queue<int> tileToExploreFrom = new Queue<int>();
        tileToExploreFrom.Enqueue(goalTileID);

        TileManager.instance.GetTileInfo(goalTileID).AddPath(goalTileID);
        TileManager.instance.GetTileInfo(goalTileID).distanceFromGoal = 0;

        while (tileToExploreFrom.Count > 0)
        {
            int currentTile = tileToExploreFrom.Dequeue();
            List<int> currentTileNeighboors = GetTileNeighbours(currentTile);

            foreach (int closeTileID in currentTileNeighboors)
            {
                TileInfo closeTileInfo = GetTileInfo(closeTileID);

                if (!CheckWalkability(closeTileInfo))
                {
                    continue;
                }
                if (closeTileInfo.distanceFromGoal >= 0)
                {
                    if (closeTileInfo.distanceFromGoal == GetTileInfo(currentTile).distanceFromGoal - 1)
                    {
                        GetTileInfo(currentTile).AddPath(closeTileID);
                    }

                    continue;
                }

                closeTileInfo.distanceFromGoal = GetTileInfo(currentTile).distanceFromGoal + 1;
                closeTileInfo.AddPath(currentTile);

                tileToExploreFrom.Enqueue(closeTileID);
            }
        }
    }

    public void SetUnitPosition(GameObject unit, int spawnTileID)
    {
        unit.transform.parent = transform;
        unit.transform.localPosition = GetTilePosition(spawnTileID);
        unit.transform.localRotation = Quaternion.Euler(0, -180, 0);
        unit.transform.localScale *= unitScaleRatio;
    }

    public void ResetTiles()
    {
        foreach (TileInfo tile in tileGrid)
        {
            tile.ResetTile();
        }
    }

    public GameObject GetTile(int tileID)
    {
        return tileGrid[tileID].gameObject;
    }

    public GameObject GetTile(int row, int line)
    {
        return tileGrid[GetTileID(row, line)].gameObject;
    }

    public TileInfo GetTileInfo(int tileID)
    {
        return tileGrid[tileID];
    }

    public TileInfo GetTileInfo(int row, int line)
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
                return new Vector3((tileRow - rowCount / 2 + 1f/2f) * Mathf.Sqrt(3) * tileSize, 0, (lineCount / 2 - tileLine - 1f/2f) * tileSize);
            else
                return new Vector3((tileRow - rowCount / 2 + 1f/2f) * Mathf.Sqrt(3) * tileSize, 0, (lineCount / 2 - tileLine) * tileSize);
        }
        else
        {
            if (lineCount % 2 == 0)
                return new Vector3((tileRow - rowCount/2) * Mathf.Sqrt(3) * tileSize, 0, (lineCount / 2 - tileLine - 1f/2f) * tileSize);
            else
                return new Vector3((tileRow - rowCount/2) * Mathf.Sqrt(3) * tileSize, 0, (lineCount / 2 - tileLine) * tileSize);
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

    public List<List<int>> GetTileIDInRange(int centerTileID, int range)
    {
        List<List<int>> tilesInRange = new List<List<int>>();
        List<int> discoveredTilesID = new List<int>();

        discoveredTilesID.Add(centerTileID);

        tilesInRange.Add(GetTileNeighbours(centerTileID));
        discoveredTilesID.AddRange(GetTileNeighbours(centerTileID)); // A mettre dans une variable?

        for (int i = 1; i < range; i++)
        {
            List<int> tilesInRangeI = new List<int>();

            foreach (int tileID in tilesInRange[tilesInRange.Count - 1])
            {
                List<int> currentTileNeighbours = GetTileNeighbours(tileID);

                //Remove duplicate
                for (i = currentTileNeighbours.Count - 1; i >= 0; i--)
                {
                    foreach (int discoveredTile in discoveredTilesID)
                    {
                        if (discoveredTile == currentTileNeighbours[i])
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

        return tilesInRange;
    }

    public List<List<GameObject>> GetTileInRange(int centerTileID, int range)
    {
        return CastRangeListToGameObject(GetTileIDInRange(centerTileID, range));
    }

    public List<List<TileInfo>> GetTileInfoInRange(int centerTileID, int range)
    {
        return CastRangeListToTileInfo(GetTileIDInRange(centerTileID, range));
    }

    private List<List<GameObject>> CastRangeListToGameObject(List<List<int>> tileListIDs)
    {
        List<List<GameObject>> tileList = new List<List<GameObject>>();

        for(int i = 0; i < tileListIDs.Count; i++)
        {
            tileList.Add(IDListToGameObjectList(tileListIDs[i]));
        }

        return tileList;
    }

    private List<List<TileInfo>> CastRangeListToTileInfo(List<List<int>> tileListIDs)
    {
        List<List<TileInfo>> tileList = new List<List<TileInfo>>();

        for (int i = 0; i < tileListIDs.Count; i++)
        {
            tileList.Add(IDListToTileInfoList(tileListIDs[i]));
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

    private List<TileInfo> IDListToTileInfoList(List<int> iDList)
    {
        List<TileInfo> tileInfoList = new List<TileInfo>();

        foreach (int iD in iDList)
        {
            tileInfoList.Add(GetTileInfo(iD));
        }

        return tileInfoList;
    }

    private bool CheckWalkability(TileInfo tileInfo)
    {
        switch(tileInfo.tileType)
        {
            case TileType.GROUND:
            case TileType.ATTACKSPAWN:  
                return true;
            case TileType.DEFENCESPAWN:
            case TileType.OBSTACLE:
            default: 
                return false;
        }
    }

}
