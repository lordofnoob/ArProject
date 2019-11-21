using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Projectile : MonoBehaviour
{
    public ParticleSystem Fire, Ice, Heavy, Piercing;

    public void SetModifier(bool fire, bool ice, bool heavy, bool piercing)
    {
        Fire.gameObject.SetActive(fire);
        Ice.gameObject.SetActive(ice);
        Heavy.gameObject.SetActive(heavy);
        Piercing.gameObject.SetActive(piercing);
    }
}
