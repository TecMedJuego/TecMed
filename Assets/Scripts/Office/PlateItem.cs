using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// funciones y acciones para objetos seleccionadoes en la toma de muestras.
public class PlateItem : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite usedSprite;
    public Sprite unusedSprite;

    public bool Used { get; private set; }
    public Image image;
    public Item item;
    void Start()
    {
        Used = false;
    }

    // Update is called once per frame

    // Usar implemento 
    public void UseItem(List<Item> addItem)
    {
        if (usedSprite != null)
        {
            image.sprite = usedSprite;
        }
        Used = true;
        addItem.Add(item);
    }
    // Reiniciar datos del implemento
    public void ClearItem()
    {
        image.sprite = unusedSprite;
        Used = false;

    }
}
