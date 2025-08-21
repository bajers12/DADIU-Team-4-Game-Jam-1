using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private HandController handController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool autoStartOnPlay = true; // Automatically start the game when the scene loads
    public float playerHealth = 150f;
    public float enemyHealth = 150f;
    void Start()
    {


    }

    public void DmgEnemy(float dmg)
    {
        enemyHealth -= dmg;
    }
    public void DmgPlayer(float dmg)
    {
        playerHealth -= dmg;
    }


    public IEnumerator PlayerTurn()
    {
        yield return handController.StartTurnRandom();
        if (enemyHealth <= 0)
        {
            Debug.Log("Enemy is defeated.");
            yield break; // End the turn if the enemy is defeated
        }
        yield return EnemyTurn();

    }

    public IEnumerator EnemyTurn()
    {
        // Implement enemy turn logic here
        if (playerHealth < 0)
        {
            Debug.Log("Player is defeated.");
        }
        yield return PlayerTurn();


    }

    



}
