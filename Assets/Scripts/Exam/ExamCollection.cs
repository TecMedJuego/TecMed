using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Lista con todos los examenes disponibles
[CreateAssetMenu(fileName = "Exam Collection", menuName = "Collections/exam")]

public class ExamCollection : ScriptableObject
{
    public List<PacientExam> examCollection;

}
