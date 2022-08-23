using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource ac;
    public AudioClip[] clips;
    public int scene = 1;


    private void Awake()
    {

    }
    void Start()
    {

        switch (scene)
        {
            case 1: //menu
                if (!ac.isPlaying)
                {
                    ac.clip = clips[0];
                    ac.Play();
                }
                break;
            case 2: //instrucoes
                break;
            case 3: //credits
                break;
            case 4: // battle scene
                ac.clip = clips[1];
                ac.Play();
                break;
            case 5: // endgame scene
                ac.clip = clips[2];
                ac.Play();
                break;
            default:
                ac.clip = clips[1];
                ac.Play();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
