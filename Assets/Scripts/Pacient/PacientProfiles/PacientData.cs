using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
// Estructura de paciente con sus datos y examenes
public enum Gender
{
    Male,
    Female
}

public enum Age
{
    teen,
    tweens,
    midlife,
    old,

}

public enum Medications
{
    antibioticos,
    valproicos,
    antiinflamatorios,
    antiparasitarios,
    ninguno
}

[CreateAssetMenu(fileName = "New Pacient Data", menuName = "Pacient Data")]
public class PacientData : ScriptableObject
{
    // Start is called before the first frame update

    public PersonalInfo personalInfo;
    public Conditions conditions;
    public SpriteLibraryAsset sprites;
    public List<PacientExam> exams;
    // retorna sprite para toma de muestras
    public Sprite GetExamSprite()
    {

        return sprites.GetSprite("Exam", "Sitted");

    }
    // retorna sprite del paciente parado
    public Sprite GetStandingSprite()
    {
        return sprites.GetSprite("Standing", "Caminando 1");
    }

}
[Serializable]
public class PersonalInfo
{
    public Gender gender;
    public string name;
    public string lastName;
    public Age age;
    public string rut;
    public PersonalInfo(Gender gender, string name, string lastname, Age age, string rut)
    {
        this.gender = gender;
        this.name = name;
        this.lastName = lastname;
        this.age = age;
        this.rut = rut;
    }
}


[Serializable]
public class Conditions
{
    
    public int food; // Si -1 no requiere ayuna 
    public bool exercise; //Siempre es 12 horas
    public int smoke; // Si -1 fumar no afecta consulta 
    public int drink; // Si -1 alcohol no afecta consulta 
    public bool sex; // Siempre es 24 horas
    public Medications medication; //Si es igual al examen no puede hacer la toma de muestras
    public bool consent;
    public Conditions(int food, bool exercise, int smoke, int drink, bool sex, Medications medication, bool consent)
    {
        this.food = food;
        this.exercise = exercise;
        this.smoke = smoke;
        this.drink = drink;
        this.sex = sex;
        this.medication = medication;
        this.consent = consent;
    }

}


