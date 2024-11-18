using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.ComponentModel;
using System;
using Unity.VisualScripting;
using System.Linq;
using TMPro;
// Manejo de UI relacionada a la toma de muestras
public class PacientSample : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasGroup canvasGroup;
    public OfficeManager manager;
    public Button addButton;
    public Button backButton;
    public Transform background;
    private bool onImplemnts;
    public List<GameObject> itemSelection = new();
    public List<Item> addedItems = new();
    public List<Item> itemsTosend = new();

    public Inventory inventory;
    public Button bathButton;
    public Button bloodButton;
    public Button torulaButton;
    private bool bloodTaken = false;
    private bool bathroomUsed = false;
    private bool torulaUsed = false;
    public Button sendButton;
    public Image pacientImage;
    public GameObject options;
    public Transform implementsPos;
    public TMP_Text orderText;
    void Start()
    {
        onImplemnts = false;
        inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Transición entre acciones toma de muestra y selección de implementos
    public void SwitchFocus()
    {
        addButton.interactable = false;
        backButton.interactable = false;

        var ratioX = (float)Screen.height / 800f;
        var ratioY = (float)Screen.width / 600f;

        if (onImplemnts)
        {
            options.SetActive(true);
            background.DOLocalMoveX(125, 0.5f, true).OnComplete(() =>
        {
            onImplemnts = false;
            addButton.interactable = true;

        });
        }
        else
        {
            options.SetActive(false);
            background.DOLocalMoveX(-1140, 0.5f, true).OnComplete(() =>
        {
            onImplemnts = true;
            backButton.interactable = true;
        });
        }
    }
    // Activa/desactiva canvasgroup y funciones relacionadas
    public void Activate(bool on)
    {
        pacientImage.sprite = manager.currentPacient.data.GetExamSprite();
        FillExams();
        canvasGroup.interactable = on;
        canvasGroup.alpha = on ? 1 : 0;
        canvasGroup.blocksRaycasts = on;
    }
    // Escribe los examenes del paciente en la interfaz
    private void FillExams()
    {
        orderText.text = "";
        for (int i = 0; i < manager.currentPacient.data.exams.Count; i++)
        {
            if (manager.currentPacient.data.exams.Count == i + 1)
            {
                orderText.text += "-" + manager.currentPacient.data.exams[i].examName;
                break;
            }
            else
            {
                orderText.text += "-" + manager.currentPacient.data.exams[i].examName + "\n";
            }
        }
    }
    // Retorna los implementos usados en la toma de muestras
    public PlateItem GetPlateItem(Item item)

    {
        PlateItem plateItem;

        foreach (GameObject gameObject in itemSelection)
        {
            plateItem = gameObject.GetComponent<PlateItem>();
            if (plateItem.item == item) return plateItem;

        }
        Debug.Log("NO ITEM FOUND ");
        return null;
    }
    // Busca si el implemento ha sido seleccionado para ser utilizado
    public bool FindItemInPlate(Item item)
    {
        foreach (Item plateitem in addedItems)
        {
            if (plateitem == item)
            {
                return true;
            }
        }

        return false;
    }
    // Agrega el implemento seleccionado a la lista de implementos para la toma de muestra
    public void AddToPlate(Item item)
    {
        if (inventory.CheckAmount(item))
        {
            if (!FindItemInPlate(item))
            {

                addedItems.Add(item);
                itemSelection[inventory.FindPositionInList(item)].SetActive(true);

            }
            else
            {

                if (!itemSelection[inventory.FindPositionInList(item)].GetComponent<PlateItem>().Used)
                {
                    addedItems.Remove(item);
                    itemSelection[inventory.FindPositionInList(item)].SetActive(false);
                }

            }

            CheckButtonActions();
        }



    }
// Revisa que acción ha sido seleccionada para la toma de muestra (sangre/baño/torula)
    public void CheckButtonActions()
    {
        if (!bloodTaken)
        {
            if (itemSelection[0].activeSelf || itemSelection[1].activeSelf || itemSelection[2].activeSelf || itemSelection[3].activeSelf || itemSelection[4].activeSelf)
            {
                if (itemSelection[5].activeSelf && itemSelection[6].activeSelf)
                    bloodButton.interactable = true;
                else
                    bloodButton.interactable = false;
            }
            else
                bloodButton.interactable = false;
        }

        if (!bathroomUsed)
        {
            if (itemSelection[7].activeSelf || itemSelection[8].activeSelf)
                bathButton.interactable = true;
            else
                bathButton.interactable = false;

        }
        if (!torulaUsed)
        {
            if (itemSelection[9].activeSelf)
                torulaButton.interactable = true;
            else
                torulaButton.interactable = false;
        }

    }


// Función especifica para diferenciar que tubos fueron utilizados al tomar sangre
    public void TakeBlood()
    {
        for (int i = 0; i <= 6; i++)
        {
            if (itemSelection[i].activeSelf)
            {
                itemSelection[i].GetComponent<PlateItem>().UseItem(itemsTosend);
            }
        }

        bloodTaken = true;
        bloodButton.interactable = false;
        sendButton.interactable = true;
        AudioManager.Instance.PlaySFX("Soft");

    }
// Acciones al usar los implementos relacionados al baño
    public void UseBathroom()
    {

        if (itemSelection[7].activeSelf) itemSelection[7].GetComponent<PlateItem>().UseItem(itemsTosend);
        if (itemSelection[8].activeSelf) itemSelection[8].GetComponent<PlateItem>().UseItem(itemsTosend);

        bathroomUsed = true;
        bathButton.interactable = false;
        sendButton.interactable = true;
        AudioManager.Instance.PlaySFX("Soft");

    }
// Acciones al usar la torula
    public void UseTorula()
    {
        if (itemSelection[9].activeSelf) itemSelection[9].GetComponent<PlateItem>().UseItem(itemsTosend);

        torulaUsed = true;
        torulaButton.interactable = false;
        sendButton.interactable = true;
        AudioManager.Instance.PlaySFX("Soft");

    }
    // Enviar las muestras extraidas y completa la toma de muestras
    public void SendSample()
    {
        AudioManager.Instance.PlaySFX("Confirm");

        GameManager.Instance.interactionButton.interactable = false;
        GameManager.Instance.state = GameState.Reception;

        GameManager.Instance.acceptedConsults.Last().usedItems = new(itemsTosend);
        foreach (Item item in addedItems)
        {
            inventory.GetInventoryItem(item).amount--;
        }
        addedItems.Clear();
        itemsTosend.Clear();
        foreach (GameObject gameObject in itemSelection)
        {
            gameObject.GetComponent<PlateItem>().ClearItem();
            gameObject.SetActive(false);
        }
        bloodTaken = false;
        bathroomUsed = false;
        torulaUsed = false;
        foreach (ItemAction ia in FindObjectsByType<ItemAction>(FindObjectsSortMode.None))
            ia.Reset();

        sendButton.interactable = false;
        FindObjectOfType<ReceptionManager>().currentPacient.PacientStateChange(Pacient.PacientState.LeaveOffice);
        GameManager.Instance.UnlockEvaluation();
        Activate(false);

        GameManager.Instance.CheckDailyPacients();
    }
}
