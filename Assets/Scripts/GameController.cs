// GameController.cs
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private HandController handController;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private HealthBar enemyHealthBar;
    [SerializeField] private PlayerAnimationController playerAnimationController;
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
    public List<Card> chosenCards = new List<Card>();

    private SceneChange sceneChange;


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
        healthBar.SetMaxHealth(playerHealth);
        enemyHealthBar.SetMaxHealth(enemyHealth);
        sceneChange = GetComponent<SceneChange>();

        if (autoStartOnPlay)
            StartCoroutine(BattleLoop());
    }

    private void OnPlayerTurnEnded()
    {   
        chosenCards = new List<Card>(handController.chosenCards);
        handController.chosenCards.Clear();
        Debug.Log("Length of chosenCards: " + chosenCards.Count);
        playerTurnFinished = true;
    }
    public void UseCards()
    {
        float nextMultiplier = 1f;

        foreach (var card in chosenCards)
        {

            float healAmount = card.HealAmount;
            float score = card.ScoreValue;
            DmgEnemy(score * nextMultiplier);
            nextMultiplier = card.Multiplier;
            DmgPlayer(-healAmount);


        }
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
            playerAnimationController.TriggerDanceAnimation(chosenCards[0].Title);

            yield return new WaitForSeconds(beatDuration * 4);
            playerAnimationController.TriggerDanceAnimation(chosenCards[1].Title);

            yield return new WaitForSeconds(beatDuration * 4);
            playerAnimationController.TriggerDanceAnimation(chosenCards[2].Title);

            yield return new WaitForSeconds(beatDuration * 4);
            UseCards();
            Debug.Log("Enemy health: " + enemyHealth);
            playerAnimationController.TriggerDanceAnimation("");
            playerDancing = false;
            Debug.Log("Enemy turn");
            enemyDancing = true;



            // wait for 4 beats while the opponet dances
            yield return new WaitForSeconds(beatDuration * 20);
            // --- Enemy turn ---
            yield return EnemyTurn();

            if (playerHealth <= 0f) break;
            enemyDancing = false;
            choosingCards = true;
        }

        if (enemyHealth <= 0f) Debug.Log("Enemy is defeated.");
        else if (playerHealth <= 0f) Debug.Log("Player is defeated.");
        else Debug.Log("Battle ended.");
        yield return new WaitForSeconds(2f);
        sceneChange.ChangeScene();
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

        yield return null;
    }

    public void DmgEnemy(float dmg) {
        enemyHealth -= dmg;
        enemyHealthBar.SetHealth(enemyHealth);
    }
    public void DmgPlayer(float dmg)
    {
        playerHealth -= dmg;
        healthBar.SetHealth(playerHealth);
    }
}
