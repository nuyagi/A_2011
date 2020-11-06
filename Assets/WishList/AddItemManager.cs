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

    //public Dictionary<string,int> wishlist = new Dictionary<string,int>();
    public Dictionary<string,int> deleteCandidates = new Dictionary<string,int>();

    // Start is called before the first frame update
    void Start()
    {
        wish = GameObject.Find("WishListObject").GetComponent<WishListScript>();
        itemNameInputField = GameObject.Find("ItemNameInputField").GetComponent<InputField>();
        itemPriceInputField = GameObject.Find("ItemPriceInputField").GetComponent<InputField>();
        content = GameObject.Find("WishedItemContent");
        newWishedItemPrefab = (GameObject)Resources.Load("WishedItemPrefab");
    }

    public void AddItemButtonPressed(){
        if(!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemPrice)){
            wish.AddItem(itemName,itemPrice);
            /*
            //wish.wishlist.Add(itemName, int.Parse(itemPrice));
            wish.wishlist.Add(itemName, int.Parse(itemPrice));
            GameObject newWishedItem = Instantiate(newWishedItemPrefab, content.transform);
            newWishedItem.transform.Find("Toggle").Find("ItemNameText").GetComponent<Text>().text = itemName;
            newWishedItem.transform.Find("Toggle").Find( "ItemPriceText" ).GetComponent<Text>().text = itemPrice;
            newWishedItem.name = itemName;
            //newWishedItem.transform.Find("Toggle").GetComponent<Toggle>().name = itemName;
            */
        }else{
            Debug.Log("Input Field is Empty or Null");
        }
        
    }

    public void DeleteButtonPressed(){
        Debug.Log("Delete");
        string[] deleteItems = new string[deleteCandidates.Count];
        deleteCandidates.Keys.CopyTo(deleteItems,0);
        foreach(string item in deleteItems){
            Debug.Log(item);
            wish.wishlist.Remove(item);
            deleteCandidates.Remove(item);
            Destroy(GameObject.Find(item));
        }
    }

/*
    public void ItemSelected(ToggleGroup togglea){
        //string selectedItemName = itemNameText.text;
        string selectedItemName = togglea.ActiveToggles().First().Find("ItemNameText").GetComponentsInChildren<Text>().text;
        Debug.Log(selectedItemName);
        deleteCandidates.Add(selectedItemName, 0);
    }

    public void ItemDeselected(ToggleGroup togglea){
        //string deselectedItemName = itemNameText.text;
        string deselectedItemName = togglea.ActiveToggles().First().Find("ItemNameText").GetComponentsInChildren<Text>().text;
        Debug.Log(deselectedItemName);
        deleteCandidates.Remove(deselectedItemName);
    }
*/
    // Update is called once per frame
    void Update()
    {
        itemName = itemNameInputField.text;
        itemPrice = itemPriceInputField.text;
        GameObject[] itemToggle = GameObject.FindGameObjectsWithTag("Item");
        if(itemToggle.Length > 0){
            for(int i=0; i<itemToggle.Length; i++){
                string selectedItemName = itemToggle[i].transform.Find("Toggle").Find("ItemNameText").gameObject.GetComponents<Text>()[0].text;
                //string selectedItemPrice = itemToggle[i].Find("ItemPriceText").GetComponents<Text>().text;
                if(itemToggle[i].transform.Find("Toggle").GetComponent<Toggle>().isOn){
                    if(!deleteCandidates.ContainsKey(selectedItemName)){
                        deleteCandidates.Add(selectedItemName,0);
                    }
                }else{
                    if(deleteCandidates.ContainsKey(selectedItemName)){
                        deleteCandidates.Remove(selectedItemName);
                    }
                }
            }
        }
    }
}
