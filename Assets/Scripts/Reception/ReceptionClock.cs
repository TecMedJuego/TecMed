using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
// Reloj que contiene tiempo para que el paciente se vaya sin ser atendido
public class ReceptionClock : MonoBehaviour
{
    // Start is called before the first frame update
    private float waitTimer;
    public float currentTimer;
    public Image image;
    public CanvasGroup canvasGroup;
    public float hideTimer = 3;
    public bool on; 
    private Pacient assignedPacient;
    public bool active; 
    private ReceptionManager receptionManager;

    void Start()
    {
        waitTimer = 300;//120
        image = transform.GetChild(0).GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        receptionManager = FindObjectOfType<ReceptionManager>();
    }

    void Update()
    {
        if (GameManager.Instance.day < 6)
            waitTimer = 300;//120
        else if (GameManager.Instance.day >= 6 && GameManager.Instance.day < 10)
            waitTimer = 240;//120
        else
            waitTimer = 200;//120

        if (active)
        {
            if (currentTimer <= 0 && on)
                EndTimer();


            if (currentTimer > 0)
                currentTimer -= Time.deltaTime;

            if (currentTimer <= 0 && !on)
            {
                currentTimer = 0;
                canvasGroup.alpha = 0;
                active = false;
            }

            UpdateVisuals();
        }

    }
    // Comienza el contador cuando el paciente se sienta
    public void StartTimer(Pacient pacient)
    {
        assignedPacient = pacient;
        on = true;
        if (GameManager.Instance.cameraState == GameState.Reception) canvasGroup.alpha = 1;
        image.fillAmount = 1;
        currentTimer = waitTimer;
        active = true;
    }
    // Paciente es llamado a recepcion
    public void GoToReception()
    {
        on = false;
        active = false;
        canvasGroup.alpha = 0;
    }
    // Finaliza el contador y el paciente se va
    public void EndTimer()
    {
        on = false;
        currentTimer = hideTimer;
        GameManager.Instance.unsatisfiedPacients.Add(assignedPacient.data);
        receptionManager.RemovePacient(assignedPacient);
        assignedPacient.PacientStateChange(Pacient.PacientState.Leave);
        GameManager.Instance.CheckDailyPacients();
        assignedPacient = null;

    }
    // Actualiza el apartado visual del reloj 
    public void UpdateVisuals()
    {
        if (on)
        {
            if (currentTimer != 0)
                image.fillAmount = currentTimer / waitTimer;
            else
                image.fillAmount = 0;
        }



    }
}
