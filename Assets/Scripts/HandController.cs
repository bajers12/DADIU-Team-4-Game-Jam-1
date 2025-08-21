using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using DG.Tweening;
using FMODUnity;

public class HandController : MonoBehaviour
{
    [Header("Game Controller")]
    [SerializeField] private GameController gameController;

    [Header("Scene refs")]
    [SerializeField] private HandView handView;
    [SerializeField] private CardViewCreator creator;
    [SerializeField] private CardViewHoverSystem hoverSystem;

    [Header("Turn config")]
    [SerializeField] private int cardsPerTurn = 5;
    [SerializeField] private int picksPerTurn = 3;

    [Header("Input & selection")]
    [SerializeField] private float hoverY = -2f;
    [SerializeField] private bool startAtRightMost = true;
    [SerializeField] private bool wrapAround = true;

    [SerializeField] private ChosenView chosenView;

    [Header("Auto-start for testing")]
    [SerializeField] private bool autoStartOnPlay = true;

    [Header("Random deal pool")]
    [Tooltip("Cards will be picked at random from this list without replacement.")]
    [SerializeField] private List<CardData> cardPool = new List<CardData>();   // <-- add your ~12 CardData here

    [Header("Events")]
    public UnityEvent OnTurnEnded;

    [Header("FMOD")]
    [SerializeField] private StudioEventEmitter leftCycle;
    [SerializeField] private StudioEventEmitter rightCycle;
    [SerializeField] private StudioEventEmitter cardConfirm;
    [SerializeField] private StudioEventEmitter cardShuffle;

    private int selectedIndex = -1;
    private int chosenThisTurn = 0;
    private bool isDealing = false;
    private List<Card> chosenCards = new List<Card>();

    private InputAction left, right, confirm;

    private void OnEnable()
    {
        left    = new InputAction("Left",    binding: "<Keyboard>/leftArrow");
        right   = new InputAction("Right",   binding: "<Keyboard>/rightArrow");
        confirm = new InputAction("Confirm", binding: "<Keyboard>/space");

        left.AddBinding("<Keyboard>/a");
        right.AddBinding("<Keyboard>/d");
        confirm.AddBinding("<Keyboard>/enter");

        left.AddBinding("<Gamepad>/dpad/left");
        right.AddBinding("<Gamepad>/dpad/right");
        confirm.AddBinding("<Gamepad>/buttonSouth");

        left.Enable(); right.Enable(); confirm.Enable();
    }

    private void OnDisable()
    {
        left?.Disable(); right?.Disable(); confirm?.Disable();
        left?.Dispose(); right?.Dispose(); confirm?.Dispose();
    }

    private void Start()
    {
        if (autoStartOnPlay)
            StartCoroutine(StartTurnRandom());
    }

private void Update()
    {
        if (isDealing || handView.CardsCount == 0) return;

               if (left.WasPressedThisFrame())
        {
            leftCycle.Play(); //plays left cycle sound
            Step(-1);
        }


        if (right.WasPressedThisFrame())
        {
            rightCycle.Play(); //plays right cycle sound
            Step(+1);
        }
        if (confirm.WasPressedThisFrame()) ChooseSelected();
    }


    public IEnumerator StartTurnRandom()
{
    chosenThisTurn = 0;

    hoverSystem.Hide();
    yield return TossEntireHand();

    if (cardPool == null || cardPool.Count == 0)
    {
        Debug.LogWarning("[HandController] cardPool is empty; cannot deal.");
        yield break;
    }
    cardShuffle.Play(); //Plays cardshuffle sound


    // Shuffle indices (Fisher–Yates) and take the first N
        var indices = new List<int>(cardPool.Count);
    for (int i = 0; i < cardPool.Count; i++) indices.Add(i);

    for (int i = 0; i < indices.Count - 1; i++)
    {
        int j = Random.Range(i, indices.Count);
        (indices[i], indices[j]) = (indices[j], indices[i]);
    }

    int toDeal = Mathf.Min(cardsPerTurn, indices.Count);
    for (int k = 0; k < toDeal; k++)
    {
        var data = cardPool[indices[k]];
        var card = new Card(data);
        var cv   = creator.CreateCardView(card, handView.transform.position, Quaternion.identity);
        yield return handView.AddCard(cv);
    }

    if (handView.CardsCount > 0)
    {
        int startIndex = startAtRightMost ? handView.CardsCount - 1 : 0;
        SetSelection(startIndex);
    }
    else
    {
        selectedIndex = -1;
    }
}

    private void Step(int dir)
    {
        if (handView.CardsCount == 0) return;

        int next = selectedIndex + dir;
        next = wrapAround
            ? (next % handView.CardsCount + handView.CardsCount) % handView.CardsCount
            : Mathf.Clamp(next, 0, handView.CardsCount - 1);

        SetSelection(next);
    }

    private void SetSelection(int index)
    {
        if (selectedIndex >= 0 && selectedIndex < handView.CardsCount)
            handView.GetCard(selectedIndex).SetHovered(false);

        selectedIndex = index;

        var card = handView.GetCard(selectedIndex);
        card.SetHovered(true);

        Vector3 pos = card.transform.position;
        pos.y = hoverY;
        hoverSystem.Show(card.Card, pos);
    }


    private void ChooseSelected()
    {
        if (selectedIndex < 0 || selectedIndex >= handView.CardsCount) return;

        var cardView = handView.GetCard(selectedIndex);
        var card = cardView.Card;
        chosenThisTurn++;
        StartCoroutine(chosenView.AddCard(card));
        chosenCards.Add(card);
        cardConfirm.Play();

        hoverSystem.Hide();
        cardView.SetHovered(false);

        Sequence seq = DOTween.Sequence();
        seq.Append(cardView.transform.DOScale(1.1f, 0.08f));
        seq.Append(cardView.transform.DOScale(0f, 0.12f));
        seq.OnComplete(() =>
        {
            handView.RemoveCard(cardView);
            Destroy(cardView.gameObject);

            if (chosenThisTurn < picksPerTurn && handView.CardsCount > 0)
            {
                int next = Mathf.Clamp(selectedIndex, 0, handView.CardsCount - 1);
                SetSelection(next);
            }
            else if (chosenThisTurn >= picksPerTurn)
            {
                StartCoroutine(EndTurn());
            }
            else
            {
                selectedIndex = -1;
            }
        });
    }

    private IEnumerator EndTurn()
    {
        left.Disable(); right.Disable(); confirm.Disable();
        hoverSystem.Hide();

        // Toss remaining cards
        yield return TossEntireHand();
        yield return chosenView.ClearAll();

        OnTurnEnded?.Invoke();

        selectedIndex = -1;
        chosenThisTurn = 0;
        UseCards();
        chosenCards.Clear();
        left.Enable(); right.Enable(); confirm.Enable();
    }

    private void UseCards()
    {
        float nextMultiplier = 1f;

        foreach (var card in chosenCards)
        {

            float healAmount = card.HealAmount;
            float score = card.ScoreValue;
            gameController.DmgEnemy(score * nextMultiplier);
            nextMultiplier = card.Multiplier;
            gameController.DmgPlayer(-healAmount);


        }
    }

    private IEnumerator TossEntireHand()
    {
        if (handView.CardsCount == 0) yield break;

        var remaining = new List<CardView>(handView.CardsCount);
        for (int i = 0; i < handView.CardsCount; i++)
            remaining.Add(handView.GetCard(i));

        float delayBetween = 0.03f;
        float t = 0f;

        foreach (var cv in remaining)
        {
            cv.SetHovered(false);

            Sequence s = DOTween.Sequence();
            s.AppendInterval(t);
            s.Append(cv.transform.DOScale(0f, 0.12f));
            s.OnComplete(() =>
            {
                handView.RemoveCard(cv);
                Destroy(cv.gameObject);
            });

            t += delayBetween;
        }

        yield return new WaitForSeconds(t + 0.14f);
    }
}
