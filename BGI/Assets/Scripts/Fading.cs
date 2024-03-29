﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{

    public float FadeRate;
    private Image image;
    private float targetAlpha;
    // Use this for initialization
    void Start()
    {
        this.image = this.GetComponent<Image>();
        this.targetAlpha = this.image.color.a;
    }
    void Update()
    {
        Color curColor = this.image.color;
        float alphaDiff = Mathf.Abs(curColor.a - this.targetAlpha);
        if (alphaDiff > 0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, this.FadeRate * Time.deltaTime);
            this.image.color = curColor;
        }
    }

    public void FadeOut(float fr)
    {
        FadeRate = fr;
        this.targetAlpha = 0.0f;
    }

    public void FadeIn(float fr)
    {
        FadeRate = fr;
        this.targetAlpha = 1.0f;
    }

    
}

