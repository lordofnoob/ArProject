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

    private List<ImageTargetBehaviour> imageList = new List<ImageTargetBehaviour>();
    public GameObject TargetImageContainer;

    private void Awake()
    {
        imageList.AddRange(TargetImageContainer.GetComponentsInChildren<ImageTargetBehaviour>());
    }

    public void Scan()
    {
        if (middleImage.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            debuggerDebugText.text = middleImage.transform.position.ToString();
           
            TileManager.instance.InstanciateGrid();
            TileManager.instance.SetTileGridTransform(middleImage.transform);
        }

        Debug.Log("SCAN");
    }

    public void ScanForUnits()
    {
        foreach (ImageTargetBehaviour imageTarget in imageList)
        { 
            if (imageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            {
                SnapOnTile(imageTarget);
            }
        }
    }

    public void SnapOnTile(ImageTargetBehaviour objectToSnap)
    {
        GameObject clone = Instantiate(objectToSnap.transform.GetChild(0).gameObject);
        TileManager.instance.SetUnitPosition(clone, GetCorrespondingTile(objectToSnap.transform.position)); 
    }

    //public void SnapOnTile(Mb_Tower objectToSnap)
    //{
    //    objectToSnap.SetUnitPosition(GetCorrespondingTile(objectToSnap.transform.position));
    //}

    public int GetCorrespondingTile(Vector3 currentElementPosition)
    {
        Debug.Log(currentElementPosition);   
        RaycastHit raycastHit;
        Debug.DrawRay(new Vector3(0, 0, 0), currentElementPosition);
        if (Physics.Raycast(new Vector3(0,0,0), currentElementPosition, out raycastHit, Mathf.Infinity, LayerMask.GetMask("Tile")))
        {
          
            TileInfo hitTile = raycastHit.collider.gameObject.GetComponentInParent<TileInfo>();
            if (hitTile)
            {
                Debug.Log(hitTile.tileID);
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
