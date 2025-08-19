using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Arrows : MonoBehaviour
{
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

    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            StartCoroutine(GlowEffect(arrowMaterialL));
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            StartCoroutine(GlowEffect(arrowMaterialR));
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            StartCoroutine(GlowEffect(arrowMaterialD));
        }
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            StartCoroutine(GlowEffect(arrowMaterialU));
        }
    }

    IEnumerator GlowEffect(Material mat)
    {
        mat.SetColor("_EmissionColor", Color.yellow * 2f);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_EmissionColor", Color.black);
    }
}