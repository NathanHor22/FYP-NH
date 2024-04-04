using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectfader : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    Material Mat;
    //tells the object to either fade or not to fade
    public bool DoFade = false;
    // Start is called before the first frame update
    void Start()
    {
        Mat = GetComponent<Renderer>().material;
        //Initiates the opacity of the material, a as in alpha value in the material
        originalOpacity = Mat.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if(DoFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }

    void FadeNow() 
    {
        Color currentColor = Mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
        Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed *Time.deltaTime));
        Mat.color = smoothColor;
    }

    void ResetFade() 
    {
        Color currentColor = Mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
        Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed*Time.deltaTime));
        Mat.color = smoothColor;
    }
}
