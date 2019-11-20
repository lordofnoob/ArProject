using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFX : MonoBehaviour
{
    public GameObject[] projectiles;
    public Vector2[] pSpeed;
    public int actualID;

    public void spawnFX()
    {
        GameObject GO = Instantiate(projectiles[actualID], transform.position, Quaternion.identity);
        GO.GetComponent<Rigidbody>().AddForce(pSpeed[actualID].x, pSpeed[actualID].y, 0, ForceMode.Impulse);
        actualID = (actualID + 1) % projectiles.Length;
    }
}
