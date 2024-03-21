using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawP_AttackPower : MonoBehaviour
{
    TextMeshProUGUI powerText;

    private void Awake()
    {
        powerText = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

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
        powerText.text = "POWER " + GameManager.Instance.M_AttackDamage * 100 + "%";
    }
}
