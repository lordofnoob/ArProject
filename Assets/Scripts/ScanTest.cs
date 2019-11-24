using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image;
public class ScanTest : MonoBehaviour
{
    public ImageTargetBehaviour TopLeftCornerImageTarget, DownRightCornerImageTarget;
    private GameObject topPos, botPos;
    public GameObject topPoseObject, botPoseObject;
    public Text debugText, debuggerDebugText;

    private void Update()
    {
        if (TopLeftCornerImageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            debuggerDebugText.text = TopLeftCornerImageTarget.transform.position.ToString();
        }
        if (DownRightCornerImageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            debugText.text = DownRightCornerImageTarget.transform.position.ToString();
        }
    }

    public void Scan()
    {
        if (TopLeftCornerImageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            debuggerDebugText.text = TopLeftCornerImageTarget.transform.position.ToString();
            botPos = Instantiate(topPoseObject, TopLeftCornerImageTarget.transform.position, Quaternion.identity);

        }
        if (DownRightCornerImageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            debugText.text = DownRightCornerImageTarget.transform.position.ToString();
            topPos = Instantiate(botPoseObject, DownRightCornerImageTarget.transform.position, Quaternion.identity);
        }

        if (topPos != null && botPos != null)
            debugText.text = "recognised";
          //  TileManager.instance.SetTileGridTransform((topPos.transform.position - botPos.transform.position) / 2 + botPos.transform.position);

        Debug.Log("SCAN");
    }
}
