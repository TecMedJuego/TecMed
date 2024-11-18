using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

// Manager de evaluación que controla interfaz, datos de pacientes, toma de muestra y rendimiento del dia completado 
public class EvaluationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static EvaluationManager Instance;
    // Canvasgroup de pantallas utilizadas
    public CanvasGroup evaluationGroup;
    public CanvasGroup generalGroup;
    public CanvasGroup pacientSelectionGroup;
    public CanvasGroup pacientInfoGroup;
    public CanvasGroup resultsGroup;
    public CanvasGroup requirementsGroup;
    public CanvasGroup implementsGroup;
    public CanvasGroup attendedGroup;
    public CanvasGroup rejectedGroup;
    //Elementos de UI instanciados por codigo
    public GameObject pacientSlot;
    public GameObject conditionSlot;
    public GameObject evaluationItem;
    public GameObject pacientButton;
    //Posiciones donde objetos instanciados se generan en UI
    public Transform missedPos;
    public Transform conditionLayout;
    public Transform itemLayout;
    public Transform attendedContainer;
    public Transform rejectedContainer;
    // Contenedores de elementos generados para destruirlos al finalizar su uso
    private List<GameObject> conditionList = new(); //< reset
    private List<GameObject> itemList = new(); //< reset
    private List<GameObject> missedList = new(); //< reset
    private List<GameObject> rejectedList = new(); //< reset
    private List<GameObject> attendedList = new(); //< reset 

    //UI general para control de pantallas y elementos visuales
    public Button previous;
    public Button next;
    private int index;
    public GameObject generalGood;
    public GameObject requirementGood;
    public GameObject implementGood;
    public GameObject officeNone;
    public TMP_Text pacientName;
    public TMP_Text examText;
    public Image pacientImage;
    public Button resultsButton;
    public Button closeButton;
    public Button requirementsButton;
    public Button implementsButton;

    // Valores para calcular dinero y reputacion obtenida, son reseteados al finalizar el dia
    private int accumulatedMoney; //< reset
    private int generalRep; //< reset
    private int attentionRep; //< reset
    private int consultRep; //< reset
                            //UI general para control de pantallas y elementos visuales
    public TMP_Text general;
    public TMP_Text attention;
    public TMP_Text consult;
    public TMP_Text finalMoney;
    public TMP_Text finalRep;
    public Image generalFace;
    public Image attentionFace;
    public Image consultFace;
    public Sprite happyFace;
    public Sprite neutralFace;

    void Start()
    {
    }

    void Awake()
    {
        Instance = this;
    }

    public void FillConsult(int index)
    {

        // Limpieza de elementos anteriores
        if (attendedList.Count != 0)
        {
            for (int i = 0; i < attendedList.Count; i++)
            {
                Destroy(attendedList[i]);
            }
        }

        if (rejectedList.Count != 0)
        {
            for (int i = 0; i < rejectedList.Count; i++)
            {
                Destroy(rejectedList[i]);
            }
        }




        attendedList.Clear();
        rejectedList.Clear();

        // Generador de pacientes atendidos
        if (GameManager.Instance.acceptedConsults.Count > 0)
        {
            foreach (Consult consult in GameManager.Instance.acceptedConsults)
            {
                if (consult.usedItems.Count > 0)
                {
                    var acceptedbutton = Instantiate(pacientButton, attendedContainer);
                    acceptedbutton.GetComponent<PacientInfoButton>().SetData(consult, false);
                    attendedList.Add(acceptedbutton);
                }

            }
        }
        // Generador de pacientes rechazados
        if (GameManager.Instance.rejectedConsults.Count > 0)
        {
            foreach (Consult consult in GameManager.Instance.rejectedConsults)
            {
                var rejectedButton = Instantiate(pacientButton, rejectedContainer);
                rejectedButton.GetComponent<PacientInfoButton>().SetData(consult, true);
                rejectedList.Add(rejectedButton);
            }
        }


    }
    // Mostrar informacion del paciente seleccionado ya sea rechazado o aceptado
    public void ShowSelectedInfo(Consult pconsult, bool rejected)
    {
        pacientImage.color = Color.white;
        pacientName.text = pconsult.pData.personalInfo.name + " " + pconsult.pData.personalInfo.lastName;
        pacientImage.sprite = pconsult.pData.sprites.GetSprite("Standing", "caminando 1");
        //mostrar examenes
        examText.text = "Examen(es):";
        for (int i = 0; i < pconsult.pData.exams.Count; i++)
        {
            if (pconsult.pData.exams.Count == i + 1)
            {
                examText.text += " " + pconsult.pData.exams[i].examName;
                break;
            }
            else
            {
                examText.text += " " + pconsult.pData.exams[i].examName + ",";
            }

        }
        officeNone.SetActive(false);
        ManageCanvasGroup(pacientSelectionGroup, false);
        ManageCanvasGroup(pacientInfoGroup, true);
        requirementGood.SetActive(false);
        implementGood.SetActive(false);
        //Limpieza de datos paciente previamente seleccionado
        if (conditionList.Count != 0)
        {
            for (int i = 0; i < conditionList.Count; i++)
            {
                Destroy(conditionList[i]);
            }
        }
        if (itemList.Count != 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i]);
            }
        }
        conditionList.Clear();
        itemList.Clear();
        SwitchShownInfo(true);
        // Si el paciente fue rechazado su muestra no fue extraida, no se muestran los implementos
        if (rejected)
        {
            //        ManageCanvasGroup(rejectedGroup, false);
            implementGood.SetActive(true);

        }
        else
        {
            //          ManageCanvasGroup(attendedGroup, false);
            var UsedItems = new List<Item>();
            foreach (PacientExam exam in pconsult.pData.exams)
                CompareImplements(pconsult.usedItems, exam, false, UsedItems);

            UsedItems.Clear();
        }
        ComparePacientInfo(pconsult, false, true);
        foreach (PacientExam exam in pconsult.pData.exams)
            CompareConditions(pconsult.pData.conditions, exam, false, true);


        requirementGood.SetActive(conditionList.Count == 0);


    }
    //Revisa que los datos del paciente concuerden con los del examen (si final = true en vez de generar UI calcula la reputación obtenida)
    public bool ComparePacientInfo(Consult consult, bool final, bool rejected)
    {
        bool usable = true;
        if (consult.incorrectInfo)
        {
            if (!final)
            {
                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo("Identificación",
                 "La información del paciente no concuerda con la del examen");
                conditionList.Add(conditionslot);

            }
            else
            {
                usable = false;
                if (!rejected)
                    attentionRep -= 3;

            }
        }

        return usable;
    }

    // Función para botones que controlan la navegación entre pantallas de pacientes atendidos y rechazados
    public void SwitchPacientView(bool rejected)
    {
        if (rejected)
        {
            ManageCanvasGroup(rejectedGroup, true);
            ManageCanvasGroup(attendedGroup, false);
        }
        else
        {
            ManageCanvasGroup(attendedGroup, true);
            ManageCanvasGroup(rejectedGroup, false);
        }
    }
    //Comparación entre condiciones del paciente y sus examenes (si final = true en vez de generar UI calcula la reputación obtenida)
    public bool CompareConditions(Conditions pacient, PacientExam exam, bool final, bool rejected)
    {

        int provRep = 0;
        bool usable = true;



        if (exam.conditions.food > -1)
        {
            if (pacient.food >= exam.conditions.food)
            {
                //good
                //    if (final)
                //        provRep++;
            }
            else
            {

                provRep--;
                usable = false;

                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo(exam.examName, "Duración de ayuno insuficiente");
                conditionList.Add(conditionslot);




                //bad
            }
        }

        if (exam.conditions.drink > -1)
        {
            if (pacient.drink >= exam.conditions.drink || pacient.drink == -1)
            {
                //  good
                //    if (final)
                //        provRep++;
            }
            else
            {

                provRep--;
                usable = false;


                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo(exam.examName, "Periodo abstención ingesta de alcohol insuficiente");
                conditionList.Add(conditionslot);

                //bad
            }
        }
        if (exam.conditions.smoke > -1)
        {
            if (pacient.smoke >= exam.conditions.smoke || pacient.smoke == -1)
            {
                //  good
                //    if (final)
                //       provRep++;
            }
            else
            {

                provRep--;
                usable = false;
                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo(exam.examName, "Tabaco afecta negativamente a la muestra requerida");
                conditionList.Add(conditionslot);

                //bad
            }
        }

        if (exam.conditions.consent)
        {
            if (pacient.consent)
            {
                //good
                //    if (final)
                //        provRep++;
            }
            else
            {

                provRep--;
                usable = false;

                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo(exam.examName, "Paciente llegó sin el consentimiento requerido");
                conditionList.Add(conditionslot);


                //bad
            }

        }

        if (exam.conditions.sex)
        {
            if (pacient.sex)
            {
                //good
                //     if (final)
                //        provRep++;
            }
            else
            {

                provRep--;
                usable = false;


                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo(exam.examName, "Abstención de actividad sexual no respetada");
                conditionList.Add(conditionslot);

                //bad
            }
        }

        if (exam.conditions.exercise)
        {
            if (pacient.exercise)
            {
                //good
                //    if (final)
                //        provRep++;

            }
            else
            {

                provRep--;
                usable = false;
                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo(exam.examName, "Abstención de ejercicio fisico no respetada");
                conditionList.Add(conditionslot);

                //bad
            }
        }

        if (exam.conditions.medication != Medications.ninguno)
        {
            if (pacient.medication == exam.conditions.medication)
            {
                //bad

                provRep--;
                usable = false;

                var conditionslot = Instantiate(conditionSlot, conditionLayout);
                conditionslot.GetComponent<Condition>().SetInfo(exam.examName, "ingesta de " + pacient.medication.ToString() + " prohibida");
                conditionList.Add(conditionslot);

            }
            else
            {
                //good
                //    if (final)
                //        provRep++;
            }
        }

        if (final)
        {
            if (rejected)
            {
                if (provRep < 0)
                    attentionRep += exam.reputation;

                else
                    attentionRep -= exam.reputation;
            }
            else
            {
                if (provRep < 0)
                    attentionRep += exam.reputation;
                else
                    attentionRep += provRep;
            }
        }

        return usable;



    }
    // Comparación entre implementos utilizados e implementos requeridos por examenes (si final = true en vez de generar UI calcula la reputación obtenida)
    public bool CompareImplements(List<Item> items, PacientExam exam, bool final, List<Item> UsedItems)
    {
        bool usable = true;
        if (!final)
        {
            foreach (Item item in exam.requiredItems)
            {

                if (UsedItems.Find(x => x == item) == null)
                {
                    var newItem = Instantiate(evaluationItem, itemLayout);
                    itemList.Add(newItem);

                    if (items.Find(x => x == item) != null)
                    {
                        newItem.GetComponent<Image>().sprite = item.icon;
                        newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(item.icon.bounds.size.x * 20, item.icon.bounds.size.y * 20);
                    }
                    UsedItems.Add(item);
                }

            }
        }
        else
        {
            int provRep = 0;
            int incorrect = 0;
            foreach (Item item in exam.requiredItems)
            {

                if (items.Find(x => x == item) != null)
                {
                    provRep++;
                }
                else
                {
                    incorrect++;
                    usable = false;
                }

            }

            if (usable)
                consultRep += provRep;
            else
                consultRep -= incorrect;


        }

        return usable;
    }

    // Generador de elementos UI representando los pacientes que se fueron de recepción sin ser atendidos
    public void MissedPacients(bool final)
    {
        if (GameManager.Instance.unsatisfiedPacients.Count != 0)
        {
            foreach (PacientData data in GameManager.Instance.unsatisfiedPacients)
            {
                if (!final)
                {
                    var slot = Instantiate(pacientSlot, missedPos);
                    slot.GetComponent<MissedPacient>().SetInfo(data.personalInfo.name, data.personalInfo.lastName,
                    data.sprites.GetSprite("Standing", "caminando 1"));
                    missedList.Add(slot);
                }
                else
                    generalRep--;

            }
            generalGood.SetActive(false);
        }
        else
        {
            generalGood.SetActive(true);
            /// mensaje de ningún paciente perdido
            if (final)
                generalRep = GameManager.Instance.day + 1;
        }
    }
    //Controlador de las 3 pantallas principales en evaluación
    public void SwitchScreens(string name)
    {
        switch (name)
        {
            case "General":
                ManageCanvasGroup(generalGroup, true);
                // ManageCanvasGroup(pacientInfoGroup, false);
                ManageCanvasGroup(pacientSelectionGroup, false);
                ManageCanvasGroup(resultsGroup, false);
                ManageCanvasGroup(pacientSelectionGroup, false);
                ManageCanvasGroup(attendedGroup, false);
                ManageCanvasGroup(rejectedGroup, false);
                ManageCanvasGroup(pacientInfoGroup, false);

                break;
            case "Pacients":
                /*
                    ManageCanvasGroup(generalGroup, false);
                    ManageCanvasGroup(pacientInfoGroup, true);
                    ManageCanvasGroup(resultsGroup, false);
                    */
                ManageCanvasGroup(generalGroup, false);
                ManageCanvasGroup(resultsGroup, false);
                ManageCanvasGroup(pacientSelectionGroup, true);
                ManageCanvasGroup(attendedGroup, true);
                ManageCanvasGroup(rejectedGroup, false);
                ManageCanvasGroup(pacientInfoGroup, false);


                break;
            case "Results":
                ManageCanvasGroup(generalGroup, false);
                //    ManageCanvasGroup(pacientInfoGroup, false);
                ManageCanvasGroup(pacientSelectionGroup, false);
                ManageCanvasGroup(pacientSelectionGroup, false);
                ManageCanvasGroup(attendedGroup, false);
                ManageCanvasGroup(rejectedGroup, false);

                ManageCanvasGroup(resultsGroup, true);
                break;
        }
    }
    // Acción volver desde información de paciente a selección de paciente
    public void BackToSelection()
    {
        ManageCanvasGroup(pacientInfoGroup, false);
        ManageCanvasGroup(pacientSelectionGroup, true);
    }
    // Función para botones que controlan la navegación entre pantallas de requerimientos e implementos
    public void SwitchShownInfo(bool requirements)
    {
        if (requirements)
        {
            ManageCanvasGroup(requirementsGroup, true);
            ManageCanvasGroup(implementsGroup, false);

        }
        else
        {
            ManageCanvasGroup(requirementsGroup, false);
            ManageCanvasGroup(implementsGroup, true);
        }
    }
    //Abrir menu de evaluación
    public void OpenEvaluation()
    {
        AudioManager.Instance.PlaySFX("Open");
        ManageCanvasGroup(evaluationGroup, true);
        SwitchScreens("General");
        MissedPacients(false);
        index = 0;
        FillConsult(index);
        SwitchPacientView(true);
    }
    //Función para mostrar y esconder canvasgroups
    public void ManageCanvasGroup(CanvasGroup canvasGroup, bool on)
    {
        canvasGroup.alpha = on ? 1 : 0;
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
    }
    //Borrar información y elementos del dia luego de ser utilizados
    public void Clear()
    {
        if (conditionList.Count != 0)
        {
            for (int i = 0; i < conditionList.Count; i++)
            {
                Destroy(conditionList[i]);
            }
        }
        if (itemList.Count != 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i]);
            }
        }
        if (missedList.Count != 0)
        {
            for (int i = 0; i < missedList.Count; i++)
            {
                Destroy(missedList[i]);
            }
        }
        conditionList.Clear();
        itemList.Clear();
        missedList.Clear();
        generalRep = 0;
        attentionRep = 0;
        consultRep = 0;

        accumulatedMoney = 0;
    }
    // Cerrar menu de evaluación
    public void Close()
    {
        ManageCanvasGroup(evaluationGroup, false);
    }
    //Habilitar/desactivar pantalla de resultados finales al completar un día de trabajo
    public void FlipResults(bool results)
    {
        resultsButton.interactable = results;
        resultsButton.GetComponent<Image>().raycastTarget = results;
        closeButton.interactable = !results;
        closeButton.GetComponent<Image>().raycastTarget = !results;

    }
    //Evaluador de reputación y dinero obtenido al finalizar un día de trabajo
    public void AssignReputationAndMoney()
    {
        MissedPacients(true);
        //MONEY loop info, consult & implements True
        if (GameManager.Instance.acceptedConsults.Count > 0)
        {
            foreach (Consult consult in GameManager.Instance.acceptedConsults)
            {
                //  accumulatedMoney += consult.exam.reward;
                var UsedItems = new List<Item>();
                for (int i = 0; i < consult.exams.Count; i++)
                {

                    bool pacient = ComparePacientInfo(consult, true, consult.usable[i]);
                    bool conditions = CompareConditions(consult.pData.conditions, consult.exams[i], true, false);
                    bool implements = CompareImplements(consult.usedItems, consult.exams[i], true, UsedItems);
                    consult.usable[i] = pacient && conditions && implements; //ERROR
                    if (consult.usable[i])
                        accumulatedMoney += consult.exams[i].reward;
                }
                UsedItems.Clear();

            }
        }

        if (GameManager.Instance.rejectedConsults.Count > 0)
        {
            foreach (Consult consult in GameManager.Instance.rejectedConsults)
            {
                for (int i = 0; i < consult.exams.Count; i++)
                {

                    bool pacient = ComparePacientInfo(consult, true, consult.usable[i]);
                    bool conditions = CompareConditions(consult.pData.conditions, consult.exams[i], true, true);
                    consult.usable[i] = pacient && conditions;
                    if (!consult.usable[i])
                        accumulatedMoney += consult.exams[i].reward / 10;
                }

            }
        }
        //UPDATE UI
        RepValue(generalFace, general, generalRep); //general
        RepValue(attentionFace, attention, attentionRep); //attention
        RepValue(consultFace, consult, consultRep); //consult
        finalMoney.text = "+" + accumulatedMoney;
        if (generalRep + attentionRep + consultRep > 0)
            finalRep.text = "Total +" + (generalRep + attentionRep + consultRep).ToString();
        else
            finalRep.text = "Total " + (generalRep + attentionRep + consultRep).ToString();

        //UPDATE MONEY AND REP GENERAL VALUES
        GameManager.Instance.money += accumulatedMoney;
        GameManager.Instance.reputation += generalRep + attentionRep + consultRep;
        GameManager.Instance.UpdateValues();
        //Add values to session list
        GameManager.Instance.AddSession(accumulatedMoney, generalRep + attentionRep + consultRep);

    }
    // Funciones que calculan puntaje y habilitan información al finalizar un día
    public void CloseConsult()
    {
        FlipResults(true);
        Close();
        AssignReputationAndMoney();
    }
    // Calculador de reputación obtenida
    public void RepValue(Image icon, TMP_Text text, int value)
    {
        if (value > 0)
        {
            icon.sprite = happyFace;
            text.text = "+" + value.ToString();
        }
        else
        {
            icon.sprite = neutralFace;
            text.text = value.ToString();
        }
    }
// Función que reinicia la interfaz y valores temporales al completar el día. Avisa a gamemanager para comenzar el siguiente dia de trabajo
    public void FinishDay()
    {
        Clear();
        FlipResults(false);
        FindObjectOfType<SceneLoader>().Fade(true);
        Close();
        GameManager.Instance.EndDay();
    }

}
