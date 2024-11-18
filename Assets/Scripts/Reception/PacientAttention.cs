using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using DG.Tweening;
// Manejo de recepción, revisión condiciones paciente, rechazar o aceptar paciente
public class PacientAttention : MonoBehaviour
{
   
    private ReceptionManager manager;
    private CanvasGroup canvasGroup;
    public CanvasGroup elementsGroup;
    public CanvasGroup documentsGroup;


    public TMP_Text identityName;
    public TMP_Text identityRut;
    public TMP_Text examName;
    public TMP_Text examRut;

    public TMP_Text examDesc;

    public Image pacient;
    public Image photo;
    public Image bubble;
    public TMP_Text bubbleText;
    public TMP_Text acceptAmount;
    public TMP_Text rejectAmount;


    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Mostrar/esconder documentos paciente
    public void LookUpInfo()
    {
        if (!documentsGroup.interactable)
        {
            elementsGroup.interactable = false;
            elementsGroup.alpha = 0;
            elementsGroup.blocksRaycasts = false;
            documentsGroup.alpha = 1;
            documentsGroup.interactable = true;
            documentsGroup.blocksRaycasts = true;

            return;
        }
        else
        {
            documentsGroup.interactable = false;
            documentsGroup.alpha = 0;
            documentsGroup.blocksRaycasts = false;
            elementsGroup.alpha = 1;
            elementsGroup.interactable = true;
            elementsGroup.blocksRaycasts = true;

            return;
        }
    }
    public void ViewFood()
    {
        AudioManager.Instance.PlaySFX("Info");
        if (manager.currentPacient.data.conditions.food == -1)
        {
            bubbleText.text = "Comí antes de venir aquí";
        }
        else
        {
            bubbleText.text = "No he ingerido alimentos por " + manager.currentPacient.data.conditions.food + " horas";
        }
        bubble.transform.DOScaleX(0, 0);
        bubble.transform.DOScaleX(1, 0.3f);

    }
    public void ViewExercise()
    {
        AudioManager.Instance.PlaySFX("Info");
        if (manager.currentPacient.data.conditions.exercise)
        {
            bubbleText.text = "No he realizado ejercicio en 12 horas";
        }
        else
        {
            bubbleText.text = "Vengo del gimnasio";
        }
        bubble.transform.DOScaleX(0, 0);
        bubble.transform.DOScaleX(1, 0.3f);

    }

    public void ViewSmoke()
    {
        AudioManager.Instance.PlaySFX("Info");

        if (manager.currentPacient.data.conditions.smoke == -1)
        {
            bubbleText.text = "No fumo cigarros";
        }
        else
        {
            bubbleText.text = "El último cigarrillo que fumé fue hace " + manager.currentPacient.data.conditions.smoke + " horas";
        }
        bubble.transform.DOScaleX(0, 0);
        bubble.transform.DOScaleX(1, 0.3f);

    }

    public void ViewDrink()
    {
        AudioManager.Instance.PlaySFX("Info");

        if (manager.currentPacient.data.conditions.drink == -1)
        {
            bubbleText.text = "No bebo alcohol";
        }
        else
        {
            bubbleText.text = "Tomé alcohol hace " + manager.currentPacient.data.conditions.drink + " horas aproximadamente";

        }
        bubble.transform.DOScaleX(0, 0);
        bubble.transform.DOScaleX(1, 0.3f);
    }

    public void ViewSex()
    {
        AudioManager.Instance.PlaySFX("Info");

        if (manager.currentPacient.data.conditions.sex)
        {
            bubbleText.text = "No he tenido relaciones sexuales en las últimas 24 horas";
        }
        else
        {
            bubbleText.text = "Tuve relaciones sexuales anoche";
        }
        bubble.transform.DOScaleX(0, 0);
        bubble.transform.DOScaleX(1, 0.3f);
    }
    public void ViewConsent()
    {
        AudioManager.Instance.PlaySFX("Info");

        if (manager.currentPacient.data.conditions.consent)
        {
            bubbleText.text = "Aquí está mi consentimiento firmado";
        }
        else
        {
            bubbleText.text = "No tengo ningún documento de consentimiento";
        }
        bubble.transform.DOScaleX(0, 0);
        bubble.transform.DOScaleX(1, 0.3f);
    }

    public void ViewMedication()
    {
        AudioManager.Instance.PlaySFX("Info");

        switch (manager.currentPacient.data.conditions.medication)
        {
            case Medications.ninguno:
                bubbleText.text = "no me encuentro tomando medicamentos";
                break;
            case Medications.antibioticos:
                bubbleText.text = "Estoy tomando antibioticos para sanar una gripe";
                break;
            case Medications.antiinflamatorios:
                bubbleText.text = "Tomé antiinflamatorio ya que desperté con dolores";
                break;

            case Medications.antiparasitarios:
                bubbleText.text = "Estoy tomando el antiparasitario que me recetó el doctor";

                break;

            case Medications.valproicos:
                bubbleText.text = "Tomo ácido valproico, ingerí una dosis antes de venir";
                break;
        }
        bubble.transform.DOScaleX(0, 0);
        bubble.transform.DOScaleX(1, 0.3f);
    }
    // Aceptar/rechazar paciente, reiniciando datos y llamando a nuevo paciente
    public void AcceptPacient(bool accepted)
    {


        if (accepted)
        {
            AudioManager.Instance.PlaySFX("Confirm");
            GameManager.Instance.UpdateValues();
            manager.currentPacient.PacientStateChange(Pacient.PacientState.ToOffice);

            Activate(false);

            GameManager.Instance.AddAcceptedConsult(manager.currentPacient.data, manager.currentPacient.incorrectInfo);
            GameManager.Instance.state = GameState.Office;


        }
        else
        {
            Activate(false);
            GameManager.Instance.AddRejectedConsult(manager.currentPacient.data, manager.currentPacient.incorrectInfo);
            manager.currentPacient.PacientStateChange(Pacient.PacientState.Leave);
            GameManager.Instance.CheckDailyPacients();
            manager.NextPacient();

        }

        GameManager.Instance.interactionButton.interactable = false;


    }
    // Activar canvas
    public void Activate(bool on)
    {
        if (on) manager = FindAnyObjectByType<ReceptionManager>();
        canvasGroup.interactable = on;
        canvasGroup.alpha = on ? 1 : 0;
        canvasGroup.blocksRaycasts = on;
        bubble.transform.DOScaleX(0, 0);

        documentsGroup.interactable = false;
        documentsGroup.alpha = 0;
        documentsGroup.blocksRaycasts = false;
        elementsGroup.alpha = 1;
        elementsGroup.interactable = true;
        elementsGroup.blocksRaycasts = true;


    }
    // Llenar datos de paciente actual
    public void FillPersonalInfo()
    {
        pacient.sprite = manager.currentPacient.img.sprite;
        photo.sprite = manager.currentPacient.img.sprite;
        int acceptedAccumulated = 0;
        int rejectedAccumulated = 0;

        foreach (PacientExam exam in manager.currentPacient.data.exams)
        {
            acceptedAccumulated += exam.reward;
            rejectedAccumulated += exam.reward / 10;
        }


        acceptAmount.text = "+" + acceptedAccumulated.ToString();
        rejectAmount.text = "+" + rejectedAccumulated.ToString();


        identityName.text = manager.currentPacient.data.personalInfo.name + " " +
        manager.currentPacient.data.personalInfo.lastName;

        identityRut.text = "Rut: " + manager.currentPacient.data.personalInfo.rut;

        if (manager.currentPacient.incorrectInfo)
        {
            PacientData fakedata = manager.fakeInfos.pacientCollection[Random.Range(0, manager.fakeInfos.pacientCollection.Count)];
            examName.text = fakedata.personalInfo.name + " " + fakedata.personalInfo.lastName;
            examRut.text = fakedata.personalInfo.rut;
        }
        else
        {
            examName.text = manager.currentPacient.data.personalInfo.name + " " +
            manager.currentPacient.data.personalInfo.lastName;

            examRut.text = manager.currentPacient.data.personalInfo.rut;
        }

        examDesc.text = "";
        for (int i = 0; i < manager.currentPacient.data.exams.Count; i++)
        {
            if (manager.currentPacient.data.exams.Count == i + 1)
            {
                examDesc.text += "-" + manager.currentPacient.data.exams[i].examName;
                break;
            }
            else
            {
                examDesc.text += "-" + manager.currentPacient.data.exams[i].examName + "\n";
            }

        }
        //examDesc.text = manager.currentPacient.data.exam.examName;

    }


}
