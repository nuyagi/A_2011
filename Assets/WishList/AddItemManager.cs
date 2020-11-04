using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItemManager : MonoBehaviour
{
    InputField itemNameInputField, itemPriceInputField;
    WishListScript wish;
    public string itemName, itemPrice;
    private GameObject content;
    private GameObject newWishedItemPrefab;

    public Dictionary<string,int> wishlist = new Dictionary<string,int>();

    // Start is called before the first frame update
    void Start()
    {
        itemNameInputField = GameObject.Find("ItemNameInputField").GetComponent<InputField>();
        itemPriceInputField = GameObject.Find("ItemPriceInputField").GetComponent<InputField>();
        content = GameObject.Find("WishedItemContent");
        newWishedItemPrefab = (GameObject)Resources.Load("WishedItemPrefab");
    }

    public void AddItemButtonPressed(){
        if(!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemPrice)){
            //wish.wishlist.Add(itemName, int.Parse(itemPrice));
            wishlist.Add(itemName, int.Parse(itemPrice));
            GameObject newWishedItem = Instantiate(newWishedItemPrefab, content.transform);
            newWishedItem.transform.Find( "ItemNameText" ).GetComponent<Text>().text = itemName;
            newWishedItem.transform.Find( "ItemPriceText" ).GetComponent<Text>().text = itemPrice;
        }else{
            Debug.Log("Input Field is Empty or Null");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        itemName = itemNameInputField.text;
        itemPrice = itemPriceInputField.text;
        
    }
}
