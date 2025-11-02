using UnityEngine;

public class PlayerComponentManager : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public PlayerMovement playerMovement;
    public GunScript gunScript;
    public SkinScript skinScript;
    public PlayerHealthHandler playerHealthHandler;
    public PlayerCarryHandler playerCarryHandler;

    public PlayerGameManager playerGameManager;


    public Animator getAnimator()
    {
        return animator;
    }

    public AudioSource getAudioSource()
    {
        return audioSource;
    }

    public PlayerMovement getPlayerMovement()
    {
        return playerMovement;
    }

    public GunScript getGunScript()
    {
        return gunScript;
    }

    public SkinScript getSkinScript()
    {
        return skinScript;
    }

    public PlayerHealthHandler getPlayerHealthHandler()
    {
        return playerHealthHandler;
    }

    public PlayerCarryHandler getPlayerCarryHandler()
    {
        return playerCarryHandler;
    }
    
    public PlayerGameManager getPlayerGameManager()
    {
        return playerGameManager;
    }

}
