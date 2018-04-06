using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthColorModifier : MonoBehaviour
{
    public float blinkTime = 0.1f;
    public Color blinkColor = Color.red;
    private bool shining;

    private Color _originalColor;

    public void Blink()
    {
        if (!shining)
        {
            _originalColor = GetComponentInChildren<Renderer>().material.GetColor("_Color");
            GetComponentInChildren<Renderer>().material.SetColor("_Color", blinkColor);
            shining = true;
            Invoke("RemoveBlink", blinkTime);
        }
    }

    public void RemoveBlink()
    {
        GetComponentInChildren<Renderer>().material.SetColor("_Color", _originalColor);
        shining = false;
    }
}