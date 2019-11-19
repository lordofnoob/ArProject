using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalPool : MonoBehaviour
{
    
    private static Dictionary<string, PoolItem> pooledItems = new Dictionary<string, PoolItem>();
    private static UniversalPool up;
    [Header("Pooling")]
    [SerializeField] private PoolItem[] poolItems;
    private static GameObject folderItem;

    [Header("Debug Mode")]
    [SerializeField]private bool DebugMode;
    private static bool dm;
    private static Dictionary<string, int> debugMax = new Dictionary<string, int>();
    private static Dictionary<string, int> debugAmount = new Dictionary<string, int>();

    void Start()
    {
        dm = DebugMode;
        folderItem = new GameObject(); //Produit une coquille
        if (up == null)
        {
            up = this;
        }
       for (int i = 0; i < poolItems.Length;i++)
       {
            if (!pooledItems.ContainsKey(poolItems[i].name))

            {
                pooledItems.Add(poolItems[i].name, poolItems[i]);
            } else
            {
                Debug.LogError("Name already in the list, no duplicates allowed ! (" + poolItems[i].name + ")");
            }
       }
        InitialPool();
    }

    void InitialPool()
    {
        Dictionary<string, PoolItem> cloneList = new Dictionary<string, PoolItem>();
        foreach (KeyValuePair<string, PoolItem> entry in pooledItems)
        {
            folderItem.name = entry.Key;
            GameObject folder = Instantiate(folderItem, transform);
            PoolItem poolRef = entry.Value;
            poolRef.availableIndexes = new Stack<int>();
            poolRef.objectInstances = new GameObject[poolRef.poolSize];
            poolRef.status = new bool[poolRef.poolSize];
            for (int i = 0; i < poolRef.poolSize; i++)
            {
                GameObject go = Instantiate(poolRef.go, Vector3.down * 100, Quaternion.identity, folder.transform);
                go.name = i.ToString("");
                go.SetActive(false);
                poolRef.objectInstances[i] = go;
                poolRef.status[i] = false;
                poolRef.availableIndexes.Push(i);
            }
            cloneList.Add(entry.Key, poolRef);
            if(DebugMode)
                debugMax.Add(entry.Key, 0);
        }
        pooledItems = cloneList;
        if (DebugMode)
            debugAmount = debugMax;

    }

    public static GameObject GetItem(string itemName)
    {
        if (pooledItems.ContainsKey(itemName))
        {
            if (dm)
            {
                debugAmount[itemName]++;
                if (debugAmount[itemName] < debugMax[itemName])
                {
                    debugMax[itemName] = debugAmount[itemName];
                }
            }
            if (pooledItems[itemName].availableIndexes.Count != 0)
            {
                int outindex = pooledItems[itemName].availableIndexes.Pop();
                pooledItems[itemName].status[outindex] = true;
                pooledItems[itemName].objectInstances[outindex].SetActive(true);
                
                return pooledItems[itemName].objectInstances[outindex];
            } else
            {
                Debug.LogError("No \"" + itemName + "\" left in the pool. Set it's \"PoolAmount\" parameter higher !");
                return null;
            }
        }
        else
        {
            Debug.LogError("Unknown Item asked to Pool (" + itemName + ")");
            return null;
        }
    }

    public static void ReturnItem(GameObject backItem, string itemName)
    {
        if (pooledItems.ContainsKey(itemName))
        {
            if (dm)
            {
                debugAmount[itemName]--;
            }
            int inIndex = 0;
            int.TryParse(backItem.name, out inIndex);
            pooledItems[itemName].availableIndexes.Push(inIndex);
            backItem.SetActive(false);
            pooledItems[itemName].status[inIndex] = false;
        }
        else
        {
            Debug.LogError("Unknown Item returned to Pool (" + itemName + ")" + " desactivating item...");
            backItem.SetActive(false);
        }
    }

    private void OnApplicationQuit()
    {
        if (DebugMode)
        {
            foreach (KeyValuePair<string, PoolItem> entry in pooledItems)
            {
                Debug.LogWarning(entry.Key + " : (" + debugMax[entry.Key] + ") Instanciated at once out of (" + pooledItems[entry.Key].poolSize + ") available items in the pool.");
                if (debugMax[entry.Key] < pooledItems[entry.Key].poolSize-30)
                {
                    Debug.LogError("Warning, Largely oversized pool, try removing a few elements ... Actual delta : (" + (pooledItems[entry.Key].poolSize - debugMax[entry.Key]) + ")");
                }
                else if (debugMax[entry.Key] > pooledItems[entry.Key].poolSize)
                {
                    Debug.LogError("Warning, Pool too small, try adding a few elements ... Actual overflow : (" + (debugMax[entry.Key]-pooledItems[entry.Key].poolSize) + ")");
                } else
                {
                    Debug.Log("Optimal Pool condition !");
                }
            }
        }
    }
}

[System.Serializable]
public struct PoolItem
{
    public string name;
    public GameObject go;
    public int poolSize;

    [HideInInspector]
    public bool[] status;
    [HideInInspector]
    public GameObject[] objectInstances;
    [HideInInspector]
    public Stack<int> availableIndexes;
}

