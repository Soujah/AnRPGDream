using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemImageSwitch : MonoBehaviour
{
    //public Image image;

    public void DisableImage(GameObject image)
    {
        image.SetActive(false);
    }
}
