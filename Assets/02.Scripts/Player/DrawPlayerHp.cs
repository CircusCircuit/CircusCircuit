using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPlayerHp : MonoBehaviour
{
    GameObject player;
    [SerializeField] Sprite full, half, empty;

    Image first, second, third, fourth;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        first = transform.GetChild(3).gameObject.GetComponent<Image>();
        second = transform.GetChild(2).gameObject.GetComponent<Image>();
        third = transform.GetChild(1).gameObject.GetComponent<Image>();
        fourth = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawHp();
    }

    void DrawHp()
    {
        switch (player.GetComponent<PlayerController>().getHp())
        {
            case 4:
                fourth.sprite = full;
                third.sprite = full;
                second.sprite = full;
                first.sprite = full;
                return;
            case 3.5f:
                fourth.sprite = half;
                third.sprite = full;
                second.sprite = full;
                first.sprite = full;
                return;
            case 3:
                fourth.sprite = empty;
                third.sprite = full;
                second.sprite = full;
                first.sprite = full;
                return;
            case 2.5f:
                fourth.sprite = empty;
                third.sprite = half;
                second.sprite = full;
                first.sprite = full;
                return;
            case 2:
                fourth.sprite = empty;
                third.sprite = empty;
                second.sprite = full;
                first.sprite = full;
                return;
            case 1.5f:
                fourth.sprite = empty;
                third.sprite = empty;
                second.sprite = half;
                first.sprite = full;
                return;
            case 1:
                fourth.sprite = empty;
                third.sprite = empty;
                second.sprite = empty;
                first.sprite = full;
                return;
            case 0.5f:
                fourth.sprite = empty;
                third.sprite = empty;
                second.sprite = empty;
                first.sprite = half;
                return;
            case 0:
                fourth.sprite = empty;
                third.sprite = empty;
                second.sprite = empty;
                first.sprite = empty;
                return;
        }

    }
}
