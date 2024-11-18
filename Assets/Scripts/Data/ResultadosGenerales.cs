using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Estructura de resultados al finalizar la sesion de juego (informacion mostrada en la pantalla final)
[SerializeField]
public class ResultadosGenerales
{
    public int reputacionFinal;
    public int CantidadExamenesDesbloqueados;
    public int pacientesInsatisfechosTotal;
    public int pacientesAtendidosCorrecto;
    public int pacientesAtendidosIncorrecto;
    public int pacientesRechazadosCorrecto;
    public int pacientesRechazadosIncorrecto;

    [SerializeField]
    public ResultadosGenerales(int reputacionFinal, int examenesDesbloqueados, int pacientesInsatisfechosTotal,
    int pacientesAtendidosCorrecto, int pacientesAtendidosIncorrecto, int pacientesRechazadosCorrecto, int pacientesRechazadosIncorrecto)
    {
        this.reputacionFinal = reputacionFinal;
        this.CantidadExamenesDesbloqueados = examenesDesbloqueados;
        this.pacientesInsatisfechosTotal = pacientesInsatisfechosTotal;
        this.pacientesAtendidosCorrecto = pacientesAtendidosCorrecto;
        this.pacientesAtendidosIncorrecto = pacientesAtendidosIncorrecto;
        this.pacientesRechazadosCorrecto = pacientesRechazadosCorrecto;
        this.pacientesRechazadosIncorrecto = pacientesRechazadosIncorrecto;
    }
}
