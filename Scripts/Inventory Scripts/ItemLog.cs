using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemLog : MonoBehaviour
{
    //item log
    public GameObject ItemListContent;
    public GameObject itemLogDisplayPrefab;

    public void NewItemObtained(Item newItem)
    {
        GameObject newButton = Instantiate(itemLogDisplayPrefab) as GameObject;
        //newButton.SetActive(true);

        //newButton.transform.Find("Text").GetComponent<TextMeshPro>().text = ButtonTextGenerator(newItem);
        newButton.transform.SetParent(ItemListContent.transform, false);
        newButton.GetComponent<ItemLogDisplayer>().SetText(ButtonTextGenerator(newItem));
    }

    private string ButtonTextGenerator(Item item)
    {
        string levelText = "Lv. " + item.itemLevel;
        if (!item.levelRequirement)
        {
            levelText = "";
        }

        string buttonText = levelText + " " + item.itemQuality + " " + item.itemType + "\n"
                + item.name + "" + ItemAmountText(item) + "\n"
                + "<sprite=0> " + item.damageValue + " <sprite=2> " + item.armorValue + " <sprite=1> " + item.healingValue;

        return buttonText;
    }

    private string ItemAmountText(Item item)
    {
        string amount = "";

        if (item.itemAmount == 0 || item.itemAmount == 1)
        {
            amount = "";
        }
        else
        {
            amount = " x" + item.itemAmount;
        }

        return amount;
    }
}
