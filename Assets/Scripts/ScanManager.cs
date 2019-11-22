using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image;

public class ScanManager : MonoBehaviour
{
    public static ScanManager instance;

    public GameObject TargetImageContainer;
    public GameObject PoolGameObjectContainer;
    private List<ImageTargetBehaviour> imageList = new List<ImageTargetBehaviour>();

    public GameObject temporaryTopPose, temporaryBotPose;
    private GameObject topPos, botPos;
    public Text debugText, debuggerDebugText;

    public WorldCenterTrackableBehaviour centerOfMap;

    public List<Mb_Enemy> allEnnemiesScanned = new List<Mb_Enemy>();
    public List<Mb_Tower> allTowersScanned = new List<Mb_Tower>();

    public ImageTargetBehaviour TopLeftCornerImageTarget, DownRightCornerImageTarget;

    public Image BoardScanCanvas, CardsScanCanvas, ConfirmValitation;

    [HideInInspector]public bool attackersValidate, defendersValidate, initValidate;


    private void Awake()
    {
        instance = this;

        VuforiaARController.RegisterARController();
        
        ResetScan();
        imageList.AddRange(TargetImageContainer.GetComponentsInChildren<ImageTargetBehaviour>());
        
        BoardScanCanvas.GetComponentInChildren<Button>().interactable = true;
        CardsScanCanvas.GetComponentInChildren<Button>().interactable = true;
        ConfirmValitation.GetComponentInChildren<Button>().interactable = true;
    }

    private void Update()
    {
        debuggerDebugText.text = Camera.main.transform.position.ToString();
    }

    public void ResetScan() // A modifier?
    {/*
        attackersValidate = false;
        defendersValidate = false;
        initValidate = false;
        BoardScanCanvas.GetComponentInChildren<Button>().interactable = false;

        allEnnemiesScanned.Clear();
        allTowersScanned.Clear();*/
    }

    public void Scan()
    {
        switch (PhaseManager.instance.GetCurrentPhase())
        {
            case Phase.INIT:
                if (TopLeftCornerImageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
                {
                    debuggerDebugText.text = TopLeftCornerImageTarget.transform.position.ToString();
                    botPos = Instantiate(temporaryBotPose, DownRightCornerImageTarget.transform.position, Quaternion.identity);

                }
                if (DownRightCornerImageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
                {
                    debugText.text = DownRightCornerImageTarget.transform.position.ToString();
                    topPos = Instantiate(temporaryTopPose, DownRightCornerImageTarget.transform.position, Quaternion.identity);
                }

                if (topPos!=null && botPos!=null)
                    TileManager.instance.SetTileGridTransform((topPos.transform.position - botPos.transform.position) /2 + botPos.transform.position);
                initValidate = true;
                break;
            case Phase.ATTACK:
            case Phase.DEFENCE:
                ScanForUnits();
                Init();
                break;
            default:
                break;
        }
        Debug.Log("SCAN");
    }

    private void Init()
    {
        Phase currentPhase = PhaseManager.instance.GetCurrentPhase();
        if (currentPhase == Phase.ATTACK)
        {
            foreach (Mb_Enemy ennemy in allEnnemiesScanned)
            {
                ennemy.Init();
            }
        }
        else if(currentPhase == Phase.DEFENCE)
        {
            foreach (Mb_Tower tower in allTowersScanned)
            {
                tower.Init();
            }
        }
    }

    private void ScanForUnits()
    {
        Phase currentPhase = PhaseManager.instance.GetCurrentPhase();

        foreach (ImageTargetBehaviour imageTarget in imageList)
        {
            if (imageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            //if(imageTarget.transform.childCount > 0 && imageTarget.transform.GetChild(0).gameObject.activeInHierarchy)
            {
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
                //Instantiate(child, imageTarget.transform.position, Quaternion.identity);
                clone.transform.position = imageTarget.transform.position;
                clone.transform.SetParent(PoolGameObjectContainer.transform); //Je comprend pas
                clone.transform.localScale = imageTarget.transform.localScale;
                //Debug.Log("INSTANTIATE : " + clone.transform.localScale);

                if (currentPhase == Phase.ATTACK && clone.GetComponentInChildren<Mb_Enemy>())
                {
                    PhaseManager.instance.attackers.Add(clone.GetComponentInChildren<Mb_Enemy>());
                }
                else if (currentPhase == Phase.DEFENCE && clone.GetComponentInChildren<Mb_Tower>())
                {
                    PhaseManager.instance.defenders.Add(clone.GetComponentInChildren<Mb_Tower>());
                }
            }
        }
    }

    private void worldPositionToTile()
    {
        Phase currentPhase = PhaseManager.instance.GetCurrentPhase();
        if(currentPhase == Phase.ATTACK)
        {
            foreach(Mb_Enemy ennemy in allEnnemiesScanned)
            {
                
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
                PhaseManager.instance.attackers = allEnnemiesScanned;
                attackersValidate = true;
                break;

            case Phase.DEFENCE:
                PhaseManager.instance.defenders = allTowersScanned;
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
                ResetScan();
                CardsScanCanvas.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }
}
