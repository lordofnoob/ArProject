using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image;
public class ScanTest : MonoBehaviour
{
    public ImageTargetBehaviour middleImage;
    private GameObject topPos, botPos;
    public GameObject topPoseObject, botPoseObject;
    public Text debugText, debuggerDebugText;

 

    public void Scan()
    {
        if (middleImage.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            debuggerDebugText.text = middleImage.transform.position.ToString();
           // botPos = Instantiate(topPoseObject, middleImage.transform.position, Quaternion.identity);
            TileManager.instance.InstanciateGrid();
            TileManager.instance.SetTileGridTransform(middleImage.transform);
        }

        if (topPos != null && botPos != null)
            debugText.text = "recognised";
          //  TileManager.instance.SetTileGridTransform((topPos.transform.position - botPos.transform.position) / 2 + botPos.transform.position);

        Debug.Log("SCAN");
    }

    public void SnapOnTile(Mb_Enemy objectToSnap)
    {
        objectToSnap.SetUnitPosition(GetCorrespondingTile(objectToSnap.transform.position)); 
    }

    public void SnapOnTile(Mb_Tower objectToSnap)
    {
        objectToSnap.SetUnitPosition(GetCorrespondingTile(objectToSnap.transform.position));
    }

    public int GetCorrespondingTile(Vector3 currentElementPosition)
    {
        int layerMask = LayerMask.NameToLayer("Tile");
        RaycastHit raycastHit;
        if (Physics.Raycast(Camera.current.transform.position, currentElementPosition - Camera.current.transform.position, out raycastHit, (currentElementPosition - Camera.current.transform.position).magnitude, layerMask))
        {
            TileInfo hitTile = raycastHit.collider.gameObject.GetComponent<TileInfo>();
            if (hitTile)
            {
                return hitTile.tileID;
            }
            else
            {
                Debug.Log("Not a tile!");
                return -2;
            }
                
        }
        Debug.Log("No collision");
        return -1;
    }
}
