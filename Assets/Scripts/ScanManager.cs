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

    public ImageTargetBehaviour MiddleBoardToken;

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
        //debuggerDebugText.text = Camera.main.transform.position.ToString();
        if (MiddleBoardToken.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            BoardScanCanvas.GetComponentInChildren<Button>().interactable = true;
        else
            BoardScanCanvas.GetComponentInChildren<Button>().interactable = false;
    }

    public void ResetScan() // A modifier?
    {
        attackersValidate = false;
        defendersValidate = false;
        initValidate = false;
        BoardScanCanvas.GetComponentInChildren<Button>().interactable = false;

        //allEnnemiesScanned.Clear();
        //allTowersScanned.Clear();
    }

    public void Scan()
    {
        switch (PhaseManager.instance.GetCurrentPhase())
        {
            case Phase.INIT:                                        //Ajouter le level design (Nexus + spawn + ...) Refactorer avec le PhaseManager
                //TileManager.instance.gameObject.SetActive(true);
                //TileManager.instance.SetTileGridTransform(MiddleBoardToken.transform);
                initValidate = true;
                break;
            case Phase.ATTACK:
            case Phase.DEFENCE:
                ScanForUnits();
                break;
            default:
                break;
        }
        Debug.Log("SCAN");
    }

    private void Init()
    {
        foreach (Mb_Enemy ennemy in allEnnemiesScanned)
        {
            ennemy.InitDebug();
        }
        foreach (Mb_Tower tower in allTowersScanned)
        {
            tower.InitDebug();
        }
    }

    private void ScanForUnits()
    {
        Phase currentPhase = PhaseManager.instance.GetCurrentPhase();

        foreach (ImageTargetBehaviour imageTarget in imageList)
        {
            if (imageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            {
                SnapOnTile(imageTarget);
            }
        }
    }

    public void ChangePhase(Phase phase)
    {
        Debug.Log("Change phase");
        ResetDisplay();
        //ResetScan();

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
                //PhaseManager.instance.attackers = allEnnemiesScanned;
                attackersValidate = true;
                Init();
                break;

            case Phase.DEFENCE:
                //PhaseManager.instance.defenders = allTowersScanned;
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

    public void SnapOnTile(ImageTargetBehaviour imageTarget)
    {
        string itemName = "";

        if (imageTarget.GetComponentInChildren<Mb_Enemy>())                         //Amelioration Faire une classe Unit
        {
            itemName = imageTarget.GetComponentInChildren<Mb_Enemy>().itemName;
        }
        else if (imageTarget.GetComponentInChildren<Mb_Tower>())
        {
            itemName = imageTarget.GetComponentInChildren<Mb_Tower>().itemName;
        }

        
        if (imageTarget.transform.GetChild(0).childCount > 4)
            return;

        for (int i = 0; i < imageTarget.transform.GetChild(0).childCount; i++)
        {
            GameObject clone = UniversalPool.GetItem(itemName);

            Mb_Enemy enemy;
            Mb_Tower tower;
            int tileID = GetCorrespondingTile(imageTarget.transform.position);

            if (tileID >= 0)
            {
                if (enemy = clone.GetComponentInChildren<Mb_Enemy>())
                {
                    enemy.Init(tileID, i);
                }
                else if (tower = clone.GetComponentInChildren<Mb_Tower>())
                {
                    tower.Init(tileID);
                }
            }
            else
            {
                Destroy(clone);
            }
        }
    }

    public int GetCorrespondingTile(Vector3 currentElementPosition)
    {
        //Debug.Log(currentElementPosition);   
        RaycastHit raycastHit;
        //Debug.DrawRay(new Vector3(0, 0, 0), currentElementPosition);
        if (Physics.Raycast(new Vector3(0, 0, 0), currentElementPosition, out raycastHit, Mathf.Infinity, LayerMask.GetMask("Tile")))
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
            else if (tower)
            {
                tower.transform.position = raycastHit.point;
                tower.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            }
        }
    }
}
