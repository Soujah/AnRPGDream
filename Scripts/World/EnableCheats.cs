using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCheats : MonoBehaviour
{

    public GameObject cheatsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && Application.isEditor)
        {
            EnableCheatsMenu();
        }
    }


    public void EnableCheatsMenu()
    {
        cheatsMenu.SetActive(!cheatsMenu.activeSelf);
    }

}
