// GameController.cs
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private HandController handController;

    [SerializeField] private HealthBar healthBar;
    public bool autoStartOnPlay = true;
    public float playerHealth = 150f;
    public float enemyHealth  = 150f;

    public bool playerTurnFinished;
    
    [Header("Timing")]
    [SerializeField] private float bpm = 124f;
    private float beatDuration;
    public bool playerDancing;
    public bool playerDanceActivated;
    public bool enemyDancing;
    public bool choosingCards;


    private void OnEnable()
    {
        if (handController != null)
            handController.OnTurnEnded.AddListener(OnPlayerTurnEnded);
    }

    private void OnDisable()
    {
        if (handController != null)
            handController.OnTurnEnded.RemoveListener(OnPlayerTurnEnded);
    }

    private void Start()
    {
        beatDuration = 60f / bpm;
        
        if (autoStartOnPlay)
            StartCoroutine(BattleLoop());
    }

    private void OnPlayerTurnEnded()
    {
        playerTurnFinished = true; // HandController already applied damage/heal
    }

    private IEnumerator BattleLoop()
    {
        while (playerHealth > 0f && enemyHealth > 0f)
        {
            // --- Player turn ---
            yield return PlayerTurn();
            if (enemyHealth <= 0f) break;
            
            playerDancing = true;
           
            //wait for 2 beats while transitioning
            yield return new WaitForSeconds(beatDuration * 4);
            Debug.Log("dancing");
            playerDanceActivated = true;
            
            
            // wait for 16 beats (player dance animation time)
            yield return new WaitForSeconds(beatDuration * 16);
            Debug.Log("Done Dancing");
            
            //wait for 2 beats while transitioning
            yield return new WaitForSeconds(beatDuration * 4);
            Debug.Log("Enemy turn");
            enemyDancing = true;

            // --- Enemy turn ---
            yield return EnemyTurn();
            if (playerHealth <= 0f) break;

            // wait for 4 beats while the opponet dances
            yield return new WaitForSeconds(beatDuration * 16);
            choosingCards = true;
        }

        if (enemyHealth <= 0f) Debug.Log("Enemy is defeated.");
        else if (playerHealth <= 0f) Debug.Log("Player is defeated.");
        else Debug.Log("Battle ended.");
    }

    public IEnumerator PlayerTurn()
    {
        playerTurnFinished = false;

        // Deal and enable player input
        yield return handController.StartTurnRandom();

        // Wait until HandController finishes the turn (after UseCards has run)
        yield return new WaitUntil(() => playerTurnFinished);

        Debug.Log("Enemy health: " + enemyHealth);
    }

    public IEnumerator EnemyTurn()
    {
        // TODO: your real enemy AI/logic here
        //yield return new WaitForSeconds(0.25f); // small pacing delay

        float enemyDamage = 30f;
        DmgPlayer(enemyDamage);
        Debug.Log($"Enemy attacks for {enemyDamage}. Player health: {playerHealth}");
        healthBar.SetHealth(playerHealth);

        yield return null;
    }

    public void DmgEnemy(float dmg)  => enemyHealth  -= dmg;
    public void DmgPlayer(float dmg) => playerHealth -= dmg;
}
