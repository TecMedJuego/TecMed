using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// Elemento UI para compra de implementos
public class ItemStoreSlot : MonoBehaviour
{
    public Item item;
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    

    // Start is called before the first frame update
    void Awake()
    {
        itemImage.rectTransform.sizeDelta = new Vector2(item.icon.bounds.size.x*100, item.icon.bounds.size.y*100);
        itemImage.sprite = item.icon;
        itemName.text = item.itemName;
        itemPrice.text = item.price.ToString();
    }
    void Start()
    {
    }

    // Update is called once per frame




}
