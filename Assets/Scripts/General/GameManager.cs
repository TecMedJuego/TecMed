using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;
using System;

//Estados de juego
public enum GameState
{
    Reception,
    Office,
    Evaluation
}
[Serializable]
// Estructura de consulta, que contiene info de paciente, examenes y implementos utilizados y si la muestra tomada es usable
public class Consult
{
    public PacientData pData;
    public bool incorrectInfo;
    public List<PacientExam> exams;
    public List<Item> usedItems;
    public List<bool> usable;

    public Consult(PacientData pData, bool incorrectInfo, List<PacientExam> exams, List<Item> usedItems, List<bool> usable)
    {
        this.pData = pData;
        this.incorrectInfo = incorrectInfo;
        this.exams = exams;
        this.usedItems = usedItems;
        this.usable = usable;
    }

}
[Serializable]
// Estructura de un dia de trabajo, conteniendo pacientes aceptados/rechazados y que se fueron de recepción Ademas de puntaje, dinero y examenes obtenidos
public class DaySession
{
    [SerializeField] public List<Consult> acceptedConsults;
    [SerializeField] public List<Consult> rejectedConsults;
    [SerializeField] public List<PacientData> unsatisfiedPacients;
    [SerializeField] public int moneyGained;
    [SerializeField] public int repGained;
    [SerializeField] public List<PacientExam> examsUnlocked;

    public DaySession(List<Consult> acceptedConsults, List<Consult> rejectedConsults, List<PacientData> unsatisfiedPacients,
    int moneyGained, int repGained, List<PacientExam> examsUnlocked)
    {
        this.acceptedConsults = acceptedConsults;
        this.rejectedConsults = rejectedConsults;
        this.unsatisfiedPacients = unsatisfiedPacients;
        this.moneyGained = moneyGained;
        this.repGained = repGained;
        this.examsUnlocked = examsUnlocked;
    }
}


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;

    // Listas que contienen las estructuras principales de juego
    // Estructuras permanentes para base de datos
    [SerializeField] public List<DaySession> daySessions;
    [SerializeField] public List<PacientData> unsatisfiedPacients;
    // Estructuras temporales por dia 
    [SerializeField] public List<Consult> acceptedConsults;
    [SerializeField] public List<Consult> rejectedConsults;
    // Control de examenes debloqueados
    [SerializeField] public List<PacientExam> unlockedExams;
    [SerializeField] public List<PacientExam> newExams;

    // Estados y valores de dinero y reputación 
    public GameState state;
    public GameState cameraState;
    public int money;
    public int reputation;
    public TMP_Text moneyText;
    public TMP_Text reputationText;
    public TMP_Text dayText;

    // Camaras
    public CinemachineVirtualCamera receptionCamera;
    public CinemachineVirtualCamera officeCamera;

    public CinemachineVirtualCamera evaluationCamera;
    // Elementos de UI encontrados en GeneralCanvas
    public Button receptionButton;
    public Button officeButton;
    public Button evaluationButton;
    public Button interactionButton;
    public Button completedButton;
    public CanvasGroup completedMessage;
    private int btnstate;
    private ReceptionManager receptionManager;
    public int maxDailyPacients;
    public int dailyPacients;
    public int day;

    public TMP_Text dayCounter;
    public CanvasGroup panelCanvas;

    void Start()
    {
        FindObjectOfType<SceneLoader>().Fade(false);
        receptionManager = FindObjectOfType<ReceptionManager>();
        btnstate = 1;
        state = GameState.Reception;
        cameraState = GameState.Reception;
        money = 50000;
        reputation = 10;
        dailyPacients = 0;
        maxDailyPacients = 10; //10
        UpdateValues();
        day = 0; //0 
        dayCounter.SetText("{00}", dailyPacients);
        UpdateDayCounter();
    }


    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        InitSingleton();
    }

    private void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Actualiza UI cuando la reputación o el dinero cambia
    public void UpdateValues()
    {
        moneyText.text = money.ToString();
        reputationText.text = reputation.ToString();
    }

    // Controlador de camaras para las 3 pantallas/estados principales 
    public void SwitchCameras(string name)
    {

        AudioManager.Instance.PlaySFX("Cam");
        switch (name)
        {
            case "Reception":
                cameraState = GameState.Reception;
                receptionCamera.Priority = 2;
                officeCamera.Priority = 1;
                evaluationCamera.Priority = 1;
                receptionButton.interactable = false;
                officeButton.interactable = true;
                evaluationButton.interactable = true;
                if (btnstate == 1) { EndHighlight(); }
                if (state == GameState.Reception)
                { InteractionStatus(true); }
                else
                { InteractionStatus(false); }

                foreach (ReceptionManager.ReceptionChair chair in receptionManager.receptionChairs)
                {
                    if (chair.clock.active)
                        chair.clock.canvasGroup.alpha = 1;
                }
                completedButton.gameObject.SetActive(false);
                panelCanvas.alpha = 1;
                break;
            case "Office":
                cameraState = GameState.Office;
                receptionCamera.Priority = 1;
                officeCamera.Priority = 2;
                evaluationCamera.Priority = 1;
                receptionButton.interactable = true;
                officeButton.interactable = false;
                evaluationButton.interactable = true;
                if (btnstate == 2) { EndHighlight(); }
                if (state == GameState.Office)
                { InteractionStatus(true); }
                else
                { InteractionStatus(false); }

                foreach (ReceptionManager.ReceptionChair chair in receptionManager.receptionChairs)
                {
                    chair.clock.canvasGroup.alpha = 0;
                }
                completedButton.gameObject.SetActive(false);
                panelCanvas.alpha = 0;

                break;
            case "Evaluation":
                cameraState = GameState.Evaluation;
                receptionCamera.Priority = 1;
                officeCamera.Priority = 1;
                evaluationCamera.Priority = 2;
                receptionButton.interactable = true;
                officeButton.interactable = true;
                evaluationButton.interactable = false;
                if (btnstate == 3) { EndHighlight(); }

                InteractionStatus(false);

                foreach (ReceptionManager.ReceptionChair chair in receptionManager.receptionChairs)
                {
                    chair.clock.canvasGroup.alpha = 0;
                }

                completedButton.gameObject.SetActive(true);
                panelCanvas.alpha = 0;

                break;
        }
    }
    // Animación para destacar botones
    public void HighlightButton(string name)
    {

        switch (name)
        {
            case "Reception":
                Sequence highlightrec = DOTween.Sequence();
                highlightrec.Append(receptionButton.transform.DOScaleY(1.3f, .25f));
                highlightrec.Append(receptionButton.transform.DOScaleY(1f, .25f));
                highlightrec.SetLoops(-1);
                btnstate = 1;

                break;
            case "Office":
                Sequence highlightof = DOTween.Sequence();
                highlightof.Append(officeButton.transform.DOScaleY(1.3f, .25f));
                highlightof.Append(officeButton.transform.DOScaleY(1f, .25f));
                highlightof.SetLoops(-1);
                btnstate = 2;

                break;
            case "Evaluation":
                Sequence highlightev = DOTween.Sequence();
                highlightev.Append(evaluationButton.transform.DOScaleY(1.3f, .25f));
                highlightev.Append(evaluationButton.transform.DOScaleY(1f, .25f));
                highlightev.SetLoops(-1);
                btnstate = 3;
                break;
        }



    }
    // Función que finaliza todas las animaciones de botones
    public void EndHighlight()
    {
        DOTween.KillAll();
        officeButton.transform.DOScaleY(1f, 0f);
        evaluationButton.transform.DOScaleY(1f, 0f);
        receptionButton.transform.DOScaleY(1f, 0f);
    }

    public void UnlockOffice()
    {
        officeButton.interactable = true;
    }
    //Activa el boton de evaluación luego de atender a 1 paciente
    public void UnlockEvaluation()
    {
        completedButton.interactable = true;
    }
    // Crea una consulta cuando un paciente es aceptado en recepción
    public void AddAcceptedConsult(PacientData data, bool incorrect)
    {

        var usableCount = new List<bool>();
        foreach (PacientExam exam in data.exams)
            usableCount.Add(false);
        acceptedConsults.Add(new Consult(data, incorrect, data.exams, new List<Item>(), usableCount));
    }
    // Crea una consulta cuando un paciente es rechazado en recepción
    public void AddRejectedConsult(PacientData data, bool incorrect)
    {
        var usableCount = new List<bool>();
        foreach (PacientExam exam in data.exams)
            usableCount.Add(false);
        rejectedConsults.Add(new Consult(data, incorrect, data.exams, null, usableCount));
    }
    // Genera una sesión de dia al ser completada
    public void AddSession(int money, int rep)
    {
        var accepted = new List<Consult>(acceptedConsults);
        var rejected = new List<Consult>(rejectedConsults);
        var unsatisfied = new List<PacientData>(unsatisfiedPacients);
        var unlocked = new List<PacientExam>(newExams);

        daySessions.Add(new DaySession(accepted, rejected, unsatisfied, money, rep, unlocked));
        newExams.Clear();
    }



    // Define que acción lleva a cabo el boton de interacción
    public void Interact()
    {
        AudioManager.Instance.PlaySFX("Open");
        switch (state)
        {

            case GameState.Reception:

                FindAnyObjectByType<ReceptionManager>().SeeAPacient();
                break;
            case GameState.Office:
                FindAnyObjectByType<OfficeManager>().StartConsult();
                break;
            case GameState.Evaluation:
                break;
        }
    }
    // Revisa si ningún paciente adicional debe aparecer para completar el día
    public void CheckDailyPacients()
    {
        dailyPacients++;
        dayCounter.SetText("{00}", dailyPacients);

        if (dailyPacients == maxDailyPacients)
        {
            HighlightButton("Evaluation");
            completedMessage.alpha = 1;
            FindObjectOfType<EvaluationManager>().CloseConsult();
            completedButton.interactable = true;

        }
    }
    // Abrir menu PC
    public void OpenPC()
    {
        FindObjectOfType<PCManager>().OpenPC(true);
    }
    // Finaliza una sesión de dia, reinicia valores temporales y revisa si se llegó al final del juego
    public void EndDay()
    {
        completedMessage.alpha = 0;
        completedButton.interactable = false;
        acceptedConsults.Clear();
        rejectedConsults.Clear();
        unsatisfiedPacients.Clear();
        if (day >= 15)
            FindObjectOfType<SceneLoader>().GoToResults();

        else
        {
            //Move camera to reception
            SwitchCameras("Reception");
            //Reset pacient counts 
            dailyPacients = 0;
            dayCounter.SetText("{00}", dailyPacients);

            receptionManager.pacientsPerDay = 0;
            maxDailyPacients++;
            //Move State to reception
            state = GameState.Reception;
            UpdateDayCounter();
        }


    }
    // Activa/desactiva canvasgroup del boton interacción
    public void InteractionStatus(bool show)
    {
        CanvasGroup interactionCanvas = interactionButton.GetComponent<CanvasGroup>();
        interactionCanvas.alpha = show ? 1 : 0;
        interactionCanvas.interactable = show;
        interactionCanvas.blocksRaycasts = show;
    }
    // LLeva directamente a la infomración del primer examen del paciente 
    public void ShowCurrentExam()
    {
        FindObjectOfType<PCManager>().OpenPC(false);
        FindObjectOfType<ExamMenu>().Open(receptionManager.currentPacient.data.exams[0]);
    }
    
    public void UpdateDayCounter()
    {
        day++;
        dayText.text = "Día: " + day;
    }

    public void CheckEndgame()
    {

    }
}

