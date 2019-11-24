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
           // botPos = Instantiate(topPoseObject, middleImage.transform.position, Quaternion.identity);
            TileManager.instance.InstanciateGrid();
            TileManager.instance.SetTileGridTransform(middleImage.transform);
        }

        if (topPos != null && botPos != null)
            debugText.text = "recognised";
          //  TileManager.instance.SetTileGridTransform((topPos.transform.position - botPos.transform.position) / 2 + botPos.transform.position);

        Debug.Log("SCAN");
    }

    public void SnapOnTile(ImageTargetBehaviour objectToSnap)
    {
        GameObject clone = Instantiate(objectToSnap.transform.GetChild(0).gameObject, transform);
        clone.GetComponentInChildren<Mb_Enemy>().SetUnitPosition(GetCorrespondingTile(objectToSnap.transform.position)); 
    }

    public void SnapOnTile(Mb_Tower objectToSnap)
    {
        objectToSnap.SetUnitPosition(GetCorrespondingTile(objectToSnap.transform.position));
    }

    public void ScanForUnits()
    {
        foreach (ImageTargetBehaviour imageTarget in imageList)
        { /*
            GameObject child = imageTarget.transform.GetChild(0).gameObject;

            string itemName = "";

            if (imageTarget.GetComponentInChildren<Mb_Enemy>())
            {
                itemName = imageTarget.GetComponentInChildren<Mb_Enemy>().itemName;
            }
            else if (imageTarget.GetComponentInChildren<Mb_Tower>())
            {
                itemName = imageTarget.GetComponentInChildren<Mb_Tower>().itemName;
            }

            GameObject clone = UniversalPool.GetItem(itemName);
            */
            if (imageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            {
                SnapOnTile(imageTarget);
            }
        }
    }

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
