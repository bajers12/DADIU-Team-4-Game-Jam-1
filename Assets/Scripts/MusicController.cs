using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MusicController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private EventReference musicEvent;
    private string musicStateParam = "MusicState"; 
    private string musicComboParam = "ComboCounter"; // navnet på din FMOD parameter
    private EventInstance musicInstance;

    void Start()
    {
       gameController = FindFirstObjectByType<GameController>();

        // start FMOD music
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
        musicInstance.release(); // frigør C#-håndtaget, men instansen spiller stadig
    }

    void Update()
    {
        // Eksempel på at sætte forskellige states
        if (gameController.playerTurnFinished == true)
        {
            RuntimeManager.StudioSystem.setParameterByNameWithLabel(musicStateParam, "Ready"); // f.eks. "Ready" state = 1
        }
        else if (gameController.playerTurnFinished == false)
        {
            RuntimeManager.StudioSystem.setParameterByNameWithLabel(musicStateParam, "PlayerRoundOver");
        }

        if (gameController.enemyHealth <= 100)
        {
            RuntimeManager.StudioSystem.setParameterByName(musicComboParam, 1f);
        }
        else if (gameController.enemyHealth <= 50)
        {
            RuntimeManager.StudioSystem.setParameterByName(musicComboParam, 2f);
        }
        else if (gameController.enemyHealth <= 0 )
        {
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
        
        if (gameController.playerHealth <= 0)
        {
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
        
        
        
        
        
    }
}