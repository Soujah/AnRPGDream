using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitToMenu : MonoBehaviour
{

    #region Singleton

    public static ExitToMenu instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Item Generator Found");
        }
        instance = this;
    }
    #endregion


    public GameObject mainMenu;

    [SerializeField] private GameObject activeUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject returnHomeUI;
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject combatUI;
    [SerializeField] private float timeTillHide = 0;

    public float UILoadDelay = 1f;
    private float timeTillUILoad;
    /*
    public void ExitToMainMenu(GameObject activeUI)
    {
        //enable menu ui and disable active ui
        activeUI.SetActive(false);
        mainMenu.SetActive(true);
    }*/

    private void Start()
    {
        timeTillUILoad = Time.time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && GameManager.instance.player.GetComponent<PlayerController>().GetCombatStatus() == false)
        {
            if (inventoryUI != activeUI)
            {
                InventoryTabSwitch.instance.UpdateInventory();
                LoadUI(inventoryUI);

            } else
            {
                LoadUI(mainMenu);
            }
        }

        if (GameManager.instance.startingWorld)
        {
            //hide return home menu button
            returnHomeUI.SetActive(false);
            saveUI.SetActive(true);
        } else
        {
            //show return home menu button
            returnHomeUI.SetActive(true);
            saveUI.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.player.GetComponent<PlayerController>().GetCombatStatus() == false)
        {
            //show options menu
            if (optionsUI != activeUI)
            {
                LoadUI(optionsUI);
            } else
            {
                LoadUI(mainMenu);
            }
        }

    }

    public void LoadUI(GameObject UI)
    {
        //enable UI and disable activeUI
        if (Time.time > timeTillUILoad || UI == combatUI)
        {
            if (UI != activeUI)
            {
                //disable active UI
                activeUI.GetComponent<Animator>().SetTrigger("Close");
                //StartCoroutine(DisableUI(activeUI));

                //enable new UI
                UI.SetActive(true);
                UI.GetComponent<Animator>().SetTrigger("Open");
                activeUI = UI;

            }
            else if (UI == activeUI)
            {
                UI.GetComponent<Animator>().SetTrigger("Close");
                //StartCoroutine(DisableUI(UI));
            }

            timeTillUILoad = Time.time + UILoadDelay;
        }
        
      
    }

    IEnumerator DisableUI(GameObject UI)
    {      
        yield return new WaitForSeconds(UI.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        UI.SetActive(false);
    }

    public GameObject GetActiveUI()
    {
        return activeUI;
    }

    public void QuitGame()
    {
        SaveGame.instance.Save();
        Application.Quit();
    }




}
