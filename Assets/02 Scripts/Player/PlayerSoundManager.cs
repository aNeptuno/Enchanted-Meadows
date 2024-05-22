using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public PlayerController player;
    public AudioSource footsteps;
    public AudioSource runsteps;

    public AudioSource tilling;

    public AudioSource watering;

    void Update()
    {
        if (player!= null)
        switch (player.GetCurrentState)
        {
            case PlayerController.PlayerStates.WALK:
                if (!footsteps.isPlaying)
                {
                    footsteps.Play();
                    runsteps.Stop();
                    tilling.Stop();
                    watering.Stop();
                }
                break;
            case PlayerController.PlayerStates.RUN:
                if (!runsteps.isPlaying)
                {
                    runsteps.Play();
                    footsteps.Stop();
                    tilling.Stop();
                    watering.Stop();
                }
                break;
            case PlayerController.PlayerStates.TILING:
                if (!tilling.isPlaying)
                {
                    tilling.Play();
                    footsteps.Stop();
                    runsteps.Stop();
                    watering.Stop();
                }
                break;
            case PlayerController.PlayerStates.WATERING:
                if (!watering.isPlaying)
                {
                    watering.Play();
                    tilling.Stop();
                    footsteps.Stop();
                    runsteps.Stop();
                }
                break;
            default:
                if (footsteps.isPlaying)
                {
                    footsteps.Stop();
                    runsteps.Stop();
                    tilling.Stop();
                    watering.Stop();
                }
                break;
        }

    }

}
