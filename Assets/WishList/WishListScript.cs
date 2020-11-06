using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishListScript : MonoBehaviour
{
    //InputField itemNameInputField, itemPriceInputField;
    public string itemName, itemPrice;
    private GameObject content;
    private GameObject newWishedItemPrefab;
    public Dictionary<string,int> wishlist = new Dictionary<string,int>();
    
    public void AddItem(string itemName, string itemPrice){
       
        wishlist.Add(itemName, int.Parse(itemPrice));
        GameObject newWishedItem = Instantiate(newWishedItemPrefab, content.transform);
        newWishedItem.transform.Find("Toggle").Find("ItemNameText").GetComponent<Text>().text = itemName;
        newWishedItem.transform.Find("Toggle").Find( "ItemPriceText" ).GetComponent<Text>().text = itemPrice;
        newWishedItem.name = itemName;
    }
    // Start is called before the first frame update
    void Start()
    {
         newWishedItemPrefab = (GameObject)Resources.Load("WishedItemPrefab");
         content = GameObject.Find("WishedItemContent");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
