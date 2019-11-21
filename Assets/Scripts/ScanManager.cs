using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ScanManager : MonoBehaviour
{
    public GameObject TargetImageContainer;
    public GameObject GameObjectInstanceContainer;
    private List<ImageTargetBehaviour> imageList = new List<ImageTargetBehaviour>();

    private void Awake()
    {
        imageList.AddRange(TargetImageContainer.GetComponentsInChildren<ImageTargetBehaviour>());
    }

    public void Scan()
    {
        Debug.Log("SCAN");
        foreach(ImageTargetBehaviour imageTarget in imageList)
        {
            if (imageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            //if(imageTarget.transform.childCount > 0 && imageTarget.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                GameObject child = imageTarget.transform.GetChild(0).gameObject;
                GameObject clone = Instantiate(child, imageTarget.transform.position, Quaternion.identity);
                clone.transform.SetParent(GameObjectInstanceContainer.transform);
                clone.transform.localScale = imageTarget.transform.localScale;
                Debug.Log("INSTANTIATE : "+clone.transform.localScale);
            }
        }
    }

    public void ChangePhase(Phase phase)
    {
        switch (phase)
        {

        }
    }
}
