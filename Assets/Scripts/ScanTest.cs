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

    private void Update()
    {
        foreach (ImageTargetBehaviour imageTarget in imageList)
        {
            if (imageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            {
                SnapOnBoard(imageTarget);
            }
        }
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
        //Debug.Log(currentElementPosition);   
        RaycastHit raycastHit;
        //Debug.DrawRay(new Vector3(0, 0, 0), currentElementPosition);
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
    
    private void SnapOnBoard(ImageTargetBehaviour imageTarget)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(new Vector3(0, 0, 0), imageTarget.transform.position, out raycastHit, Mathf.Infinity, LayerMask.GetMask("Board")))
        {
            Mb_Enemy ennemy = imageTarget.GetComponentInChildren<Mb_Enemy>();
            Mb_Tower tower = imageTarget.GetComponentInChildren<Mb_Tower>();

            if (ennemy)
            {
                ennemy.transform.position = raycastHit.point;
                ennemy.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            }
            else if(tower)
            {
                tower.transform.position = raycastHit.point;
                tower.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            }
        }
    }
}
