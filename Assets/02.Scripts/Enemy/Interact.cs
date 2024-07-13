using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Interact : MonoBehaviour
{
    float originalSpeed = GameManager.Instance.PlayerSpeed;

    public void PlayerSpeedInitialization(){
        GameManager.Instance.PlayerSpeed = originalSpeed;
    }
    public void PlayerSpeedDown(float delay){

        GameManager.Instance.PlayerSpeed = originalSpeed*0.5f;

        Invoke("PlayerSpeedInitialization", delay);

    }

    public void PlayerSpeedUp(float delay){

        GameManager.Instance.PlayerSpeed = originalSpeed*1.5f;

        Invoke("PlayerSpeedInitialization", delay);
    }


}
