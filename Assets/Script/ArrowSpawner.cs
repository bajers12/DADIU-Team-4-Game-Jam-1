using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowSpawner : MonoBehaviour
{
     private ChosenView chosenView;

    [SerializeField] private List<CardData> cardData;
    private int cardIterator = 0;
    public GameObject arrowPrefabU;
    public GameObject arrowPrefabD;
    public GameObject arrowPrefabL;
    public GameObject arrowPrefabR;

    public Transform placeholderU;
    public Transform placeholderD;
    public Transform placeholderL;
    public Transform placeholderR;

    [Header("Spawn Settings")]
    public float bpm = 120f;
    Vector3 spawnPos = Vector3.zero;   // where arrows spawn
    public float arrowSpeed = 5f;   // same as ArrowMover speed
    public float distanceToTarget = 5f; // distance from spawn → placeholder

    private float beatInterval;
    private float nextSpawnTime;
    private int sequenceIndex = 0;

    Vector3 dance;

    

    private float timer = 0f;
   

    void Start()
    {
        beatInterval = 60f / bpm;

        // Offset so arrows arrive on beat
        float travelTime = distanceToTarget / arrowSpeed;
        nextSpawnTime = Time.time + travelTime;
    }

    void Update()
    {
        if (cardIterator < cardData.Count)
        
            if (sequenceIndex >= cardData[cardIterator].Sequence.Length){
                cardIterator++; // next card
                sequenceIndex = 0; 
            } 

            timer += Time.deltaTime;

            if (timer >= beatInterval)
                {
                    SpawnArrow(cardData[cardIterator].Sequence[sequenceIndex]);
                    sequenceIndex++;
                    timer -= beatInterval; // subtract instead of reset to avoid drift
                }
    }

    void SpawnArrow(CardStep step)
    {

        GameObject arrowToSpawn = null;
        

        switch (step)
        {
            case CardStep.U:
                spawnPos = placeholderU.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabU;
                break;
            case CardStep.D:
                spawnPos = placeholderD.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabD;
                
                break;
            case CardStep.L:
                spawnPos = placeholderL.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabL;
               
                break;
            case CardStep.R:
                spawnPos = placeholderR.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabR;
                
                break;
            /*case CardStep.UL:
                spawnPos = placeholderU.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabU;
                spawnPos = placeholderL.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabL;
                break;
            case CardStep.LR:
                spawnPos = placeholderL.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabL;
                spawnPos = placeholderR.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabR;
                break;
            case CardStep.UD:
                spawnPos = placeholderU.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabU;
                spawnPos = placeholderD.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabD;
                break;
            case CardStep.UR:
                spawnPos = placeholderU.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabU;
                spawnPos = placeholderR.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabR;
                break;
            case CardStep.LD:
                spawnPos = placeholderL.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabL;
                spawnPos = placeholderD.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabD;
                break;
            case CardStep.RD:
                spawnPos = placeholderR.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabR;
                spawnPos = placeholderD.position + Vector3.up * distanceToTarget;
                arrowToSpawn = arrowPrefabD;
                break;*/
            case CardStep.P:
                // Pause → no spawn
                break;
        }
        if (arrowToSpawn != null)
        {
            GameObject arrow = Instantiate(arrowToSpawn, spawnPos, Quaternion.identity);


            ArrowMover mover = arrow.GetComponent<ArrowMover>();
        }
    }
    
}
