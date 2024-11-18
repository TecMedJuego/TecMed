using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;
using System.Data.SqlTypes;
//UI Compra de Implementos
public class PurchaseMenu : MonoBehaviour
{
    public Button closeButton;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    public Image itemImage;
    public TMP_Text totalPrice;
    public TMP_Text previousQuantity;
    public TMP_Text buyQuantity;
    private int addedAmount;
    private int addedPrice;
    public Button buyButton;
    public CanvasGroup canvasGroup;
    private ItemStoreSlot[] itemStoreSlots;
    private Inventory inventory;
    private Item selectedItem;


    // Start is called before the first frame update
    void Start()
    {
        itemStoreSlots = FindObjectsOfType<ItemStoreSlot>();
        addedAmount = 1;
        inventory = FindObjectOfType<Inventory>();
        addedPrice = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }
    // Selección de implemento a comprar
    public void Open(Item item)
    {
        AudioManager.Instance.PlaySFX("Soft");
        foreach (ItemStoreSlot storeSlot in itemStoreSlots)
        {
            storeSlot.GetComponent<Button>().interactable = false;
        }
        addedAmount = 1;
        buyQuantity.text = "1";
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        selectedItem = item;
        itemImage.rectTransform.sizeDelta = new Vector2(item.icon.bounds.size.x * 100, item.icon.bounds.size.y * 100);
        itemImage.sprite = item.icon;
        itemName.text = item.itemName;
        itemDesc.text = item.description;
        addedPrice = item.price;
        totalPrice.text = addedPrice.ToString();

        previousQuantity.text = inventory.GetAmount(item).ToString() + " unidad(es)";
        if (GameManager.Instance.money < addedPrice)
            buyButton.interactable = false;
        else
            buyButton.interactable = true;

    }

    public void Close()
    {
        AudioManager.Instance.PlaySFX("Close");
        foreach (ItemStoreSlot storeSlot in itemStoreSlots)
        {
            storeSlot.GetComponent<Button>().interactable = true;
        }
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    // Aumenta la cantiad de implementos a comprar
    public void Add()
    {
        addedAmount++;
        addedPrice += selectedItem.price;
        totalPrice.text = addedPrice.ToString();
        buyQuantity.text = addedAmount.ToString();
        if (GameManager.Instance.money < addedPrice)
            buyButton.interactable = false;
        else
            buyButton.interactable = true;
    }
    // Disminuye la cantiad de implementos a comprar
    public void Substract()
    {
        if (addedAmount > 0)
        {
            addedAmount--;
            buyQuantity.text = addedAmount.ToString();
            addedPrice -= selectedItem.price;
            totalPrice.text = addedPrice.ToString();
            if (GameManager.Instance.money < addedPrice)
                buyButton.interactable = false;
            else
                buyButton.interactable = true;
        }
    }
    // Confirma compra, agrega implementos comprados a inventario y remueve dinero gastado
    public void Confirm()
    {
        AudioManager.Instance.PlaySFX("Confirm");
        inventory.AddItem(selectedItem, addedAmount);

        GameManager.Instance.money -= addedPrice;
        GameManager.Instance.UpdateValues();
        Close();
    }
}
