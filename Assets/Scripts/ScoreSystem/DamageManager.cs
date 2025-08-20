using unityengine;
using UnityEngine.Events;

public class DamageManager : MonoBehaviour
{
    private list<Card> cards = new List<Card>();

    public onCardSelected onCardSelected(Card card){
        cards.Add(card);
    };

    public void calculateTotalDamage()
    {
        int totalDamage = 0;
        int multiplier = 1;

        foreach (var card in cards)
        {
            if card.Type == CardType.Damage {
                totalDamage += card.ScoreValue * multiplier;
                Debug.Log("Applying damage: " + card.ScoreValue + " with multiplier: " + multiplier); 
                multiplier = 1; // Reset multiplier after applying damage
            }
            else if card.Type == CardType.Multiplier { 
                multiplier *= card.ScoreValue; 
                }
        }

        // Deal the total damage to the target

        Debug.Log("Total Damage from round: " + totalDamage);

        // Reset the damage manager
        cards.Clear();
        multiplier = 1; // Reset multiplier for the next round
    }
}