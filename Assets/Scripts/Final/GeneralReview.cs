using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Proyecto26;
using Newtonsoft.Json;

// Manager de UI para pantalla final que muestra rendimiento de usuario y envío de objetos para la base de datos
public class GeneralReview : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_Text repNumber;
    [SerializeField] private TMP_Text examNumber;
    [SerializeField] private TMP_Text unsatisfiedNumber;
    [SerializeField] private TMP_Text attendedNumber;
    [SerializeField] private TMP_Text rejectedNumber;
    [SerializeField] private TMP_Text congratulations;
    [SerializeField] private TMP_Text sendMessage;
    [SerializeField] private Button sendButton;


    void Start()
    {
        FinalCount();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SendData()
    {
        SendPost();
    }

    private void SendPost()
    {

        //acción POST que envia las estructuras de resultados a la base de datos

        Resultados resultados = new();
        var jsonResults = JsonConvert.SerializeObject(resultados);

        RestClient.Post("https://tecmed-b3a59-default-rtdb.firebaseio.com/.json", jsonResults).Then(res =>
        {
            RestClient.ClearDefaultParams();

            Debug.Log("Success");
            sendMessage.text = "Datos Enviados";
        })
        .Catch(err => Debug.Log(err.Message)); ;

        sendButton.interactable = false;

    }



    // Relleno de información final en UI
    public void FinalCount()
    {

        congratulations.text = "Bien hecho " + PlayerData.Instance.GetName();
        if (GameManager.Instance.reputation > 0)
            repNumber.text = "<color=#00B1FF>" + GameManager.Instance.reputation + "</color>";
        else
            repNumber.text = "<color=#EF5060>" + GameManager.Instance.reputation + "</color>";

        examNumber.text = GameManager.Instance.unlockedExams.Count.ToString();

        int totalUnsatisfied = 0;

        foreach (DaySession session in GameManager.Instance.daySessions)
        {
            totalUnsatisfied += session.unsatisfiedPacients.Count;
        }
        unsatisfiedNumber.text = totalUnsatisfied.ToString();

        int totalAcceptedCorrect = 0;
        int totalAcceptedIncorrect = 0;
        foreach (DaySession session in GameManager.Instance.daySessions)
        {
            foreach (Consult acceptedConsult in session.acceptedConsults)
            {
                foreach (bool usableBool in acceptedConsult.usable)
                {
                    if (usableBool)
                        totalAcceptedCorrect++;
                    else
                        totalAcceptedIncorrect++;
                }
            }

        }

        attendedNumber.text = "<color=#00B1FF>" + totalAcceptedCorrect + "</color><color=#4B4B4B>/</color>" +
        "<color=#EF5060>" + totalAcceptedIncorrect + "</color>";


        int totalRejectedCorrect = 0;
        int totalRejectedIncorrect = 0;
        foreach (DaySession session in GameManager.Instance.daySessions)
        {
            foreach (Consult rejectedConsult in session.rejectedConsults)
            {
                foreach (bool usableBool in rejectedConsult.usable)
                {
                    if (usableBool)
                        totalRejectedCorrect++;
                    else
                        totalRejectedIncorrect++;
                }
            }

        }
        rejectedNumber.text = "<color=#00B1FF>" + totalRejectedCorrect + "</color><color=#4B4B4B>/</color>" +
                "<color=#EF5060>" + totalRejectedIncorrect + "</color>";

    }
}
