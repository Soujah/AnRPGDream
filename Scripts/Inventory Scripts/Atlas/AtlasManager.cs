using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtlasManager : MonoBehaviour
{


    

    public TextMeshProUGUI worldName;
    public TextMeshProUGUI worldType;
    public TextMeshProUGUI worldBoss;

    public int atlasIndex = 0;

    private Map mapOnPage;

    void OnEnable()
    {
        //LoadFirstPage();

        
    }

    void Update()
    {
        
    }

    public void LoadFirstPage()
    {
        if (GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaps().Count != 0 && GameManager.instance.mapGenerationFinished)
        {
            //Debug.Log("loading first map");
            LoadAtlasPage(GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaps()[0]);
            atlasIndex = 0;
        }
    }

    public void LoadAtlasPage(Map mapToLoad)
    {
        //render image for map
        //Debug.Log("Loading " + mapToLoad.mapName);
        worldType.text = "World Type \n" + mapToLoad.type.ToString();
        worldBoss.text = "World Boss";
        //Debug.Log(mapToLoad.mapBosses[0].GetEnemyName());
        if (mapToLoad.mapBosses != null)
        {
            foreach (Enemy item in mapToLoad.mapBosses)
            {
                if (item.IsBoss())
                {
                    worldBoss.text = "World Boss \n" + item.GetEnemyName();
                    break;
                }
            }
        }
        
        worldName.text = mapToLoad.mapName;
        LoadRenderOfMap(mapToLoad, 13);
        mapOnPage = mapToLoad;
    }

    public void NextAtlasPage(int indexAppend)
    {
        /*
        if (GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaps().Count != 0 && GameManager.instance.mapGenerationFinished)
        {
            LoadAtlasPage(GameManager.instance.player.GetComponent<PlayerManager>().ownedMaps[indexAppend]);
        }*/

        
        if (atlasIndex + indexAppend >= 0 && atlasIndex + indexAppend <= GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaps().Count -1 && GameManager.instance.mapGenerationFinished)
        {
            LoadAtlasPage(GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaps()[atlasIndex + indexAppend]);
            //Debug.Log("Indexing To Load Index: " + atlasIndex + indexAppend);
            atlasIndex += indexAppend;
        } 
    }

    public void LoadRenderOfMap(Map mapToRender, int layer)
    {
        GameManager.instance.LoadExistingWorldFromMap(mapToRender, 13);
    }

    public void LoadMapAtIndex(int index)
    {
        if (GameManager.instance.mapGenerationFinished)
        {
            LoadAtlasPage(GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaps()[index]);
            Debug.Log("Loading map index: " + index);
        }
        
    }

    public void VisitPageWorld()
    {
        if (mapOnPage != null)
        {
            GameManager.instance.LoadExistingWorldFromMap(mapOnPage, 12);
            ExitToMenu.instance.LoadUI(ExitToMenu.instance.mainMenu);
            //VisitWorld();

            /*
            if (GameManager.instance.playerTeleporting == false)
            {
                Debug.Log("starting teleport");
                StartCoroutine(VisitWorld());
            }*/
        }
    }

    IEnumerator VisitWorld()
    {
        Debug.Log("starting method");
        GameManager.instance.playerTeleporting = true;
        GameManager.instance.playerAnimator.SetTrigger("PlayerDissolve");
        yield return new WaitForSeconds(GameManager.instance.playerDissolveAnimation.length);
        Debug.Log("ending animation");
        GameManager.instance.LoadExistingWorldFromMap(mapOnPage, 12);
        GameManager.instance.playerAnimator.SetTrigger("PlayerAppear");
        GameManager.instance.playerTeleporting = false;
        Debug.Log("ending teleport");

    }

}
