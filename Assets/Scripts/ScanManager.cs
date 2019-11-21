using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Image = UnityEngine.UI.Image;

public class ScanManager : MonoBehaviour
{
    public static ScanManager instance;

    public GameObject TargetImageContainer;
    public GameObject GameObjectInstanceContainer;
    private List<ImageTargetBehaviour> imageList = new List<ImageTargetBehaviour>();

    public Image BoardScanCanvas, CardsScanCanvas, ConfirmValitation;

    public bool attackersValidate, defendersValidate, initValidate;


    private void Awake()
    {
        instance = this;

        ResetDisplay();
        ResetBool();
        imageList.AddRange(TargetImageContainer.GetComponentsInChildren<ImageTargetBehaviour>());
    }

    public void ResetBool()
    {
        attackersValidate = false;
        defendersValidate = false;
        initValidate = false;
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
        Debug.Log("Change phase");
        ResetDisplay();

        switch (phase)
        {
            case Phase.INIT:
                //SHOW SCANNING BOARD DISPLAY
                BoardScanCanvas.gameObject.SetActive(true);
                break;
            case Phase.ATTACK:
            case Phase.DEFENCE:
                //SHOW SCANNING CARDS DISPLAY
                CardsScanCanvas.gameObject.SetActive(true);
                break;
            default:
                break;
        }

    }

    void ResetDisplay()
    {
        CardsScanCanvas.gameObject.SetActive(false);
        BoardScanCanvas.gameObject.SetActive(false);
        ConfirmValitation.gameObject.SetActive(false);
    }

    public void AskForConfirmation()
    {
        ResetDisplay();
        ConfirmValitation.gameObject.SetActive(true);
    }

    public void Confirm()
    {
        switch (PhaseManager.instance.GetCurrentPhase())
        {
            case Phase.INIT:
                initValidate = true;
                break;

            case Phase.ATTACK:
                attackersValidate = true;
                break;

            case Phase.DEFENCE:
                defendersValidate = true;
                break;

            default:
                break;
        }
    }

    public void Return()
    {
        ResetDisplay();

        switch (PhaseManager.instance.GetCurrentPhase())
        {
            case Phase.INIT:
                BoardScanCanvas.gameObject.SetActive(true);
                break;

            case Phase.ATTACK:
            case Phase.DEFENCE:
                CardsScanCanvas.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }
}
