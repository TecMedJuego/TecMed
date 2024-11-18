using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Estructura de resultados por dia para base de datos
[SerializeField]
public class ConsultaAceptada //Informacion de pacientes atendidos en recepcion
{
    public string NombrePaciente;
    public string rut;
    public bool datosIncorrectos;
    public Condiciones condicionesPaciente;
    public List<string> examenes;
    public List<bool> muestrasUsables;
    public List<string> implementosUtilizados;

    public ConsultaAceptada(Consult consult, Condiciones condiciones)
    {
        NombrePaciente = consult.pData.personalInfo.name + " " + consult.pData.personalInfo.lastName;
        rut = consult.pData.personalInfo.rut;
        datosIncorrectos = consult.incorrectInfo;
        this.condicionesPaciente = condiciones;
        List<string> ex = new();
        foreach (PacientExam pacientExam in consult.pData.exams)
        {
            ex.Add(pacientExam.examName);
        }
        examenes = ex;
        muestrasUsables = consult.usable;
        List<string> imp = new();
        foreach (Item item in consult.usedItems)
        {
            imp.Add(item.itemName);
        }
        implementosUtilizados = imp;
    }

}
[SerializeField]
public class ConsultaRechazada //Informacion de pacientes rechazados en recepcion
{
    public string NombrePaciente;
    public string rut;
    public bool datosIncorrectos;
    public Condiciones condicionesPaciente;
    public List<string> examenes;
    public List<bool> muestrasUsables;

    public ConsultaRechazada(Consult consult, Condiciones condiciones)
    {
        NombrePaciente = consult.pData.personalInfo.name + " " + consult.pData.personalInfo.lastName;
        rut = consult.pData.personalInfo.rut;
        datosIncorrectos = consult.incorrectInfo;
        this.condicionesPaciente = condiciones;
        List<string> ex = new();
        foreach (PacientExam pacientExam in consult.pData.exams)
        {
            ex.Add(pacientExam.examName);
        }
        examenes = ex;
        muestrasUsables = consult.usable;
    }
}
[SerializeField]
public class Dia // Estructura de dia con puntaje obtenido, examenes desbloqueados y desempeño con pacientes
{
    public int ReputacionObtenida;
    public int DineroGanado;
    public List<string> ExamenesDesbloqueados;
    public int PacientesInsatisfechos;
    public List<ConsultaAceptada> PacientesAtendidos;
    public List<ConsultaRechazada> PacientesRechazados;

    public Dia(DaySession daySession, List<ConsultaAceptada> PacientesAtendidos, List<ConsultaRechazada> PacientesRechazados)
    {
        ReputacionObtenida = daySession.repGained;
        DineroGanado = daySession.moneyGained;
        List<string> ed = new();
        foreach (PacientExam exam in daySession.examsUnlocked)
        {
            ed.Add(exam.examName);
        }
        ExamenesDesbloqueados = ed;
        PacientesInsatisfechos = daySession.unsatisfiedPacients.Count;
        this.PacientesAtendidos = PacientesAtendidos;
        this.PacientesRechazados = PacientesRechazados;

    }
}
[SerializeField]
public class Condiciones // Estructura de condiciones en paciente y examenes
{
    public int Ayuna;
    public bool AbstenenciaEjercicio;
    public int AbstenenciaCigarro;
    public int AbstenenciaAlcohol;
    public bool AbstenenciaSexual;
    public string medicamento;
    public bool ConsentimientoFirmado;

    public Condiciones(Conditions conditions)
    {
        Ayuna = conditions.food;
        AbstenenciaEjercicio = conditions.exercise;
        AbstenenciaCigarro = conditions.smoke;
        AbstenenciaAlcohol = conditions.drink;
        AbstenenciaSexual = conditions.sex;
        medicamento = System.Enum.GetName(typeof(Medications), conditions.medication);
        ConsentimientoFirmado = conditions.consent;
    }
}
[SerializeField]
public class ResultadosDiarios
{
    public List<Dia> Dias;

    public ResultadosDiarios()
    {
        Dias = new();
        List<ConsultaAceptada> cas = new();
        List<ConsultaRechazada> crs = new();

        foreach (DaySession session in GameManager.Instance.daySessions)
        {
            foreach (Consult consult in session.acceptedConsults)
            {
                Condiciones condiciones = new Condiciones(consult.pData.conditions);
                ConsultaAceptada ca = new ConsultaAceptada(consult, condiciones);
                cas.Add(ca);
            }
            foreach (Consult consult in session.rejectedConsults)
            {
                Condiciones condiciones = new Condiciones(consult.pData.conditions);
                ConsultaRechazada cr = new ConsultaRechazada(consult, condiciones);
                crs.Add(cr);
            }
            Dias.Add(new Dia(session, cas, crs));
        }

    }
}
