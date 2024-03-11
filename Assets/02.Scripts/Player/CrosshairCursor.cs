using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    public static CrosshairCursor instance;

    public Vector2 mouseCursorPos;

    private void Awake()
    {
        instance = this;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
    }
}
