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

    private bool playerTurnFinished;

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

            // --- Enemy turn ---
            yield return EnemyTurn();
            if (playerHealth <= 0f) break;
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
        yield return new WaitForSeconds(0.25f); // small pacing delay

        float enemyDamage = 30f;
        DmgPlayer(enemyDamage);
        Debug.Log($"Enemy attacks for {enemyDamage}. Player health: {playerHealth}");
        healthBar.SetHealth(playerHealth);

        yield return null;
    }

    public void DmgEnemy(float dmg)  => enemyHealth  -= dmg;
    public void DmgPlayer(float dmg) => playerHealth -= dmg;
}
