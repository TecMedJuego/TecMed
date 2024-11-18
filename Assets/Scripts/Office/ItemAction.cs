using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Selección de implemento durante uso de implementos toma de muestra
public class ItemAction : MonoBehaviour
{
    // Start is called before the first frame update

    public bool blood;
    public GameObject blooditem;
    public Item item;
    public Image image;
    public Image rectangle;
    public Inventory inventory;
    private TextMeshProUGUI number;
    public bool selected;
    public Color selectedColor;
    public Color unselectedColor;
    private PacientSample pacientSample;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite red;



    void Start()
    {
        pacientSample = FindObjectOfType<PacientSample>();
        inventory = FindAnyObjectByType<Inventory>();
        selected = false;
        number = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        number.text = inventory.GetAmount(item).ToString();
        if (inventory.GetAmount(item) > 0)
            rectangle.sprite = green;
        else
            rectangle.sprite = red;
    }


    // Reinicia las variables al completar la toma de muestras
    public void Reset()
    {
        selected = false;

        if (blood)
            blooditem.SetActive(false);
        else
            image.color = unselectedColor;
    }
    // Define las acciónes al seleccionar un implemento
    public void CheckClick()
    {
        if (!pacientSample.GetPlateItem(item).Used)
        {

            if (inventory.CheckAmount(item))
            {
                AudioManager.Instance.PlaySFX("Implement");
                selected = !selected;
                if (selected)
                {
                    if (blood)
                    {
                        blooditem.SetActive(true);
                    }
                    else
                        image.color = selectedColor;
                }
                else
                {
                    if (blood)
                    {
                        blooditem.SetActive(false);
                    }
                    else
                        image.color = unselectedColor;
                }
            }
        }

    }
}
