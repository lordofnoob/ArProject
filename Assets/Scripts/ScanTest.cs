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
}
