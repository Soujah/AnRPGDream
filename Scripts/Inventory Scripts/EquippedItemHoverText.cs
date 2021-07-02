using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class EquippedItemHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI hoverTextBox;
    public Item itemToDisplay;
    public EquippedItemButton equippedItemButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("hover");
        if (itemToDisplay != null && equippedItemButton.empty == false)
        {
            hoverTextBox.text = LoadItemText(itemToDisplay);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("hover exit");
        hoverTextBox.text = "";
    }

    public string LoadItemText(Item item)
    {
        string newString = "Lv. " + item.itemLevel + " " + item.itemQuality + "\n"
            + item.itemType + "\n"
            + item.name + "\n"
            + "<sprite=0> " + item.damageValue + " <sprite=2> " + item.armorValue + " <sprite=1> " + item.healingValue;

        return newString;
    }

    public void SetItem(Item newItem)
    {
        itemToDisplay = newItem;
        equippedItemButton.empty = true;
    }

}
