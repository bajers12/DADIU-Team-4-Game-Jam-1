using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Checker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool canBePressed;
    public Key key;
    public GameObject arrowL;  // assign in Inspector
    public GameObject arrowR;
    public GameObject arrowD;  // assign in Inspector
    public GameObject arrowU;
    // assign in Inspector
    private Material arrowMaterialL;
    private Material arrowMaterialR;
    private Material arrowMaterialD;
    private Material arrowMaterialU;

    void Start()
    {
        arrowMaterialL = arrowL.GetComponent<Renderer>().material;
        arrowMaterialR = arrowR.GetComponent<Renderer>().material;
        arrowMaterialD = arrowD.GetComponent<Renderer>().material;
        arrowMaterialU = arrowU.GetComponent<Renderer>().material;

        arrowMaterialL.EnableKeyword("_EMISSION");
        arrowMaterialR.EnableKeyword("_EMISSION");
        arrowMaterialD.EnableKeyword("_EMISSION");
        arrowMaterialU.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[key].wasPressedThisFrame)
        {
            if (canBePressed)
            {
                if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
                {
                    StartCoroutine(GlowEffect(arrowMaterialL));
                    BeatChecker.instance.IsOnBeat(out float closestBeat);
                }
                if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
                {
                    StartCoroutine(GlowEffect(arrowMaterialR));
                    BeatChecker.instance.IsOnBeat(out float closestBeat);
                }
                if (Keyboard.current.upArrowKey.wasPressedThisFrame)
                {
                    StartCoroutine(GlowEffect(arrowMaterialU));
                    BeatChecker.instance.IsOnBeat(out float closestBeat);
                }
                if (Keyboard.current.downArrowKey.wasPressedThisFrame)
                {
                    StartCoroutine(GlowEffect(arrowMaterialD)); 
                    BeatChecker.instance.IsOnBeat(out float closestBeat); 
                }
                
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activate")
        {
            canBePressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activate")
        {
            canBePressed = false;
            BeatChecker.instance.IsOnBeat(out float closestBeat);
        }
    }
    IEnumerator GlowEffect(Material mat)
    {
        mat.SetColor("_EmissionColor", Color.yellow * 2f);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_EmissionColor", Color.black);
        gameObject.SetActive(false);
    }
}
