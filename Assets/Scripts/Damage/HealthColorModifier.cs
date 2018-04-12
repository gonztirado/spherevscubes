using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthColorModifier : MonoBehaviour
{
    private float _initTransparency = 0;


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
    
    public void ResetInitTransparency()
    {
        _initTransparency = 0;
    }

    private void InitColorTransparency()
    {
        if (_initTransparency < 1)
        {
            _initTransparency += Time.deltaTime;
            ChangeColorTransparency(_initTransparency);
        }
    }
    
    
}