using System;
using FMODUnity;
using UnityEngine;

public class AudioWinLose : MonoBehaviour
{
    public StudioEventEmitter winningEmitter;
    public StudioEventEmitter losingEmitter;
    private GameController gameController;
    private bool hasPlayedWin = false;
    private bool hasPlayedLose = false;


    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (gameController.enemyHealth <= 0 && !hasPlayedWin)
            {
                winningEmitter.Play();
                hasPlayedWin = true;
            }

            if (gameController.playerHealth <= 0 && !hasPlayedLose)
            {
                losingEmitter.Play();
                hasPlayedLose = true;
            }
        }
    }
}
