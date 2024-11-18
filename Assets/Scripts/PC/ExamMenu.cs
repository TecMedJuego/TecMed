using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// UI para selección, revisión y compra de examenes 
public class ExamMenu : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public PacientExam exam;
    public Button closeButton;
    public TMP_Text examName;
    public TMP_Text examDesc;
    public TMP_Text priceText;
    public Button buyButton;
    public Transform implementsTransform;
    public Sprite lockedImage;
    public GameObject examItem;
    public GameObject unlocked;
    public GameObject locked;
    public List<GameObject> itemsList;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Activa la información del examen seleccionado si ha sido desbloqueado
    public void FillUnlocked()
    {
        locked.SetActive(false);
        unlocked.SetActive(true);
        examName.text = exam.examName;
        examDesc.text = exam.examDescription;

        foreach (Item implement in exam.requiredItems)
        {
            var newItem = Instantiate(examItem, implementsTransform);
            newItem.GetComponent<Image>().sprite = implement.icon;
            newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(implement.icon.bounds.size.x * 15,
            implement.icon.bounds.size.y * 15);
            itemsList.Add(newItem);
        }
    }
    // Activa la información del examen seleccionado si no ha sido desbloqueado

    public void FillLocked()
    {
        locked.SetActive(true);
        unlocked.SetActive(false);
        examName.text = exam.examName;
        examDesc.text = "Porfavor compre el examen para desbloquear su descripción...";

        priceText.text = exam.cost.ToString();
        if (exam.cost <= GameManager.Instance.money)
            buyButton.interactable = true;
        else
            buyButton.interactable = false;

        foreach (Item implement in exam.requiredItems)
        {
            var newItem = Instantiate(examItem, implementsTransform);
            newItem.GetComponent<Image>().sprite = lockedImage;
            newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 106);
            itemsList.Add(newItem);
        }

    }
    // Abre la información del examen seleccionado
    public void Open(PacientExam provExam)
    {
        AudioManager.Instance.PlaySFX("Soft");

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        exam = provExam;
        if (itemsList.Count != 0)
        {
            for (int i = 0; i < itemsList.Count; i++)
            {
                Destroy(itemsList[i]);
            }
        }
        itemsList.Clear();

        if (GameManager.Instance.unlockedExams.Find(x => x == provExam) != null)
            FillUnlocked();
        else
            FillLocked();
    }
    // Cerrar menu de implementos
    public void Close()
    {
        AudioManager.Instance.PlaySFX("Close");

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    // Compra el examen, cobra el dinero y se agrega a la lista en gamemanager
    public void Buy()
    {
        AudioManager.Instance.PlaySFX("Confirm");
        GameManager.Instance.unlockedExams.Add(exam);
        GameManager.Instance.newExams.Add(exam);

        GameManager.Instance.money -= exam.cost;
        GameManager.Instance.UpdateValues();
        Open(exam);
    }
}
