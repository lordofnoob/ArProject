using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Enemy : MonoBehaviour
{
    [Header("Characteritics")]
    public Sc_Monsters monsterCharacteristics;
    public allCharacterisitcs monsterUpdatedCharacteristics;

    [Header("GraphicPart")]
    public Animator anim;

    private void Awake()
    {
        monsterUpdatedCharacteristics = monsterCharacteristics.monsterBaseCharacteristics;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


