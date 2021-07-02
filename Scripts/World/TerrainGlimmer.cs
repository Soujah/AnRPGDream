using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGlimmer : MonoBehaviour
{

    public float glimmerTime = 0.5f;
    public bool glimmerFinished = false;
    void Start()
    {
        
    }

    void Update()
    {
        
        if (GameManager.instance.mapGenerationFinished && GameManager.instance.GetCurrentWorld() != null)
        {
            if (glimmerFinished)
            {
                StartCoroutine(GlimmerTiles());
            }
        }
    }

    public void TriggerGlimmer()
    {
        if (glimmerFinished)
        {
            StartCoroutine(GlimmerTiles());
        }
    }

   
    public IEnumerator GlimmerTiles()
    {
        if (GameManager.instance.GetCurrentWorld() != null)
        {
            glimmerFinished = false;
            WaitForSeconds wait = new WaitForSeconds(glimmerTime);

            GameObject currentWorld = GameManager.instance.GetCurrentWorld();

            List<GameObject> worldTiles = new List<GameObject>();

            foreach (Transform item in currentWorld.transform)
            {
                if (item.name == "WorldTile(Clone)")
                {
                    worldTiles.Add(item.gameObject);
                }
            }


            worldTiles = worldTiles.SortByDistance(new Vector3(-10,0,Random.Range(-20, 10)));

            for (int i = 0; i < worldTiles.Count; i++)
            {
                if (worldTiles[i] != null)
                {
                    worldTiles[i].GetComponent<Animator>().SetTrigger("Glimmer");
                }
                yield return wait;
            }

            

            glimmerFinished = true;
        }
        
    }
}
