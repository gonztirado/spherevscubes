using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthColorModifier : MonoBehaviour
{
    private float InitTransparency = 0;


    void Update()
    {
        InitColorTransparency();
    }

    public void ChangeColorTransparency(float transparency)
    {
        Color currentColor = GetComponentInChildren<Renderer>().material.GetColor("_Color");
        GetComponentInChildren<Renderer>().material.SetColor("_Color",
            new Color(currentColor.r, currentColor.g, currentColor.b, transparency));
    }

    private void InitColorTransparency()
    {
        if (InitTransparency < 1)
        {
            InitTransparency += Time.deltaTime;
            ChangeColorTransparency(InitTransparency);
        }
    }
}