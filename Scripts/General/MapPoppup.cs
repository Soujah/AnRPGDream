using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MapPoppup : MonoBehaviour
{

    public GameObject bigMap;
    public GameObject postProcessingVolume;
    public AnimationClip mapTransitionAnimation;
    public Animator mapTransitionAnimator;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(ToggleMap());
        }
    }


    public void RevealMap()
    {
        StartCoroutine(ToggleMap());
    }

    IEnumerator ToggleMap()
    {
        mapTransitionAnimator.SetTrigger("TransitionMap");
        yield return new WaitForSeconds(mapTransitionAnimation.length / 2);
        ToggleBigMap();

    }


    private void ToggleBigMap()
    {
        bigMap.SetActive(!bigMap.activeSelf);
        if (bigMap.activeSelf == true)
        {
            //blurr background
            //postProcessingVolume.GetComponent<DepthOfField>().active = true;
            //postProcessingVolume.SetActive(true);
        } else
        {
            //remove blurr
            //postProcessingVolume.GetComponent<DepthOfField>().active = false;
            //postProcessingVolume.SetActive(false);
        }
    }
}
