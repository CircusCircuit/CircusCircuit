using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawP_AttackPower : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI powerTxt;
    [SerializeField] TextMeshProUGUI featherTxt;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += UpdateAttackPower;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateAttackPower(Scene scene, LoadSceneMode mode)
    {
        powerTxt.text = "POWER " + GameManager.Instance.M_AttackDamage + "%";
        featherTxt.text = "Free Feather " + GameManager.Instance.FreeFeather + "%";
    }
}
