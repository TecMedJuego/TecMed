using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object resultados con informacion de partida y usuario para base de datos

[SerializeField]
public class Resultados
{
    
    public string nombreUsuario; //nombre de usuario para reconocimiento de sesion
    [SerializeField] public ResultadosGenerales resultadosGenerales; // informacion final mostrada en la pantalla de resultados
    [SerializeField] public ResultadosDiarios resultadosDiarios; // informacion diara detallada en la sesion de juego

    public Resultados()
    {

        int totalUnsatisfied = 0;

        foreach (DaySession session in GameManager.Instance.daySessions)
        {
            totalUnsatisfied += session.unsatisfiedPacients.Count;
        }

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

        nombreUsuario = PlayerData.Instance.GetName();

        resultadosGenerales = new ResultadosGenerales(GameManager.Instance.reputation,
        GameManager.Instance.unlockedExams.Count, totalUnsatisfied, totalAcceptedCorrect, totalAcceptedIncorrect,
        totalRejectedCorrect, totalRejectedIncorrect);

        resultadosDiarios = new ResultadosDiarios();


    }

}
