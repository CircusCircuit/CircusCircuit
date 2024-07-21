using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageMng : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.Play("Boss_stage_BGM", 0.8f, SoundType.BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
