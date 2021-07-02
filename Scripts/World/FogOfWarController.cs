using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour
{
    public ParticleSystem PS; 

    /*
    void Awake()
    {        
        
        if (GameManager.instance.currentWorld != null)
        {
            if (PS.particleCount >= PS.main.maxParticles && GameManager.instance.currentWorld.fogRevealed == false)
            {
                PS.Stop();
            }
            else if (GameManager.instance.currentWorld.fogRevealed == true)
            {
                DisableFog();
            }
        }
        
    }*/

    private void Update()
    {             
        if (PS.particleCount >= PS.main.maxParticles)
        {
            PS.Stop();
        }

        /*
        if (GameManager.instance.startingWorld)
        {
            DisableFog();
        } else if (GameManager.instance.currentWorld != null)
        {
            if (GameManager.instance.currentWorld.fogRevealed == false)
            {
                Debug.Log("enabling fog");
                EnableFog();
            } else
            {
                Debug.Log("disabling fog");
                DisableFog();
            }
        }*/
    }

    public void EnableFog()
    {
        gameObject.SetActive(true);
        PS.Play();
    }

    public void DisableFog()
    {
        PS.Stop();
        gameObject.SetActive(false);
    }

    public void ResetFog()
    {
        PS.Play();      
    }

}
