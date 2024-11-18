using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using JetBrains.Annotations;
using UnityEngine;
using Unity.VisualScripting;
using System;
using Unity.IO.LowLevel.Unsafe;
// Inventario conteniendo los implementos disponibles
public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public List<PacientExam> unlockedExams;

    public List<InventoryItem> items;

    [Serializable]
    public class InventoryItem
    {
        public Item item;
        public int amount;

        public InventoryItem(Item item, int amount)
        {
            item = this.item;
            amount = this.amount;
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    // función entrega la cantidad del implemento solicitado
    public int GetAmount(Item item)
    {

        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.item == item)
            {
                return inventoryItem.amount;
            }
        }

        Debug.Log("NOHAY");
        return 0;
    }

    // Revisa si el usuario tiene el implemento solicitado 
    public bool CheckAmount(Item item)
    {
        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.item == item)
            {
                return inventoryItem.amount > 0;
            }
        }
        Debug.Log("NOHAY");
        return false;
    }
    // Agrega una cantidad del implemento solicitado al inventario
    public void AddItem(Item item, int amount)
    {

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == item)
            {
                items[i].amount += amount;
                break;
            }

        }
    }

    // Entrega la posición del implemento en la lista
    public int FindPositionInList(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == item)
                return i;
        }
        Debug.Log("notfound");
        return 0;

    }
    // Entrega la clase de inventoryItem (implemento + cantidad) basado en el implemento
    public InventoryItem GetInventoryItem(Item item)
    {
        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.item == item)
            {
                return inventoryItem;
            }
        }


        Debug.Log("Inventory item not found");

        return null;
    }
    // remueve o reduce la cantidad del implemento en InventoryItem
    public void RemoveItem(Item item)
    {
        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.item == item)
            {
                if (inventoryItem.amount <= 0)
                {
                    Debug.Log("Cant remove, there are no items left");
                }
                else
                {
                    inventoryItem.amount -= 1;
                }
                break;
            }
        }

    }
}
