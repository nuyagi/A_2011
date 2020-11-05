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
    public ToggleGroup toggleGroup;

    public Dictionary<string,int> wishlist = new Dictionary<string,int>();
    public Dictionary<string,int> deleteCandidates = new Dictionary<string,int>();

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
            newWishedItem.name = itemName;
            newWishedItem.transform.Find("Toggle").GetComponent<Toggle>().name = itemName;
        }else{
            Debug.Log("Input Field is Empty or Null");
        }
        
    }

    public void DeleteButtonPressed(){
        Debug.Log("Delete");
        foreach(KeyValuePair<string,int> item in deleteCandidates){
            Debug.Log(item.Key);
            wishlist.Remove(item.Key);
            deleteCandidates.Remove(item.Key);
            Destroy(GameObject.Find(item.Key));
        }
    }

    public void ItemSelected(/*Text itemNameText*/){
        //string selectedItemName = itemNameText.text;
        string selectedItemName = GameObject.GetComponentInChildren<Toggle>().name;
        Debug.Log(selectedItemName);
        deleteCandidates.Add(selectedItemName, 0);
    }

    public void ItemDeselected(/*Text itemNameText*/){
        //string deselectedItemName = itemNameText.text;
        string deselectedItemName = GameObject.GetComponentInChildren<Toggle>().name;
        Debug.Log(deselectedItemName);
        deleteCandidates.Remove(deselectedItemName);
    }

    // Update is called once per frame
    void Update()
    {
        itemName = itemNameInputField.text;
        itemPrice = itemPriceInputField.text;
        
    }
}
