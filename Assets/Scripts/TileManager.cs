using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int rowCount;
    public int lineCount;
    public float tileSize;

    public bool isRawBased;
    public bool isStartingBig;

    public GameObject[] tileGrid;

    private void Start()
    {
        tileGrid = new GameObject[rowCount * (lineCount/2) + (lineCount%2) * (rowCount / 2)];
    }

    public Vector3 GetTilePosition(int tileID)
    {
        int tileLine = GetLine(tileID);
        int tileRow = GetRow(tileID);
        if (rowCount%2 == 0)
        {
            //Manque le cas paire de ligne
            // A implementer
            return Vector3.zero;
        }
        else
        {
            //Manque le cas paire de ligne
            return new Vector3((lineCount/2 - tileLine) * tileSize / 2f, transform.position.y, (tileRow - rowCount / 2 + 1) * Mathf.Sqrt(3) * tileSize / 2);
        }
    }

    public GameObject GetTile(int tileID)
    {
        return tileGrid[tileID];
    }

    public GameObject GetTile(int x, int y)
    {
        return tileGrid[0];
    }

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
}
