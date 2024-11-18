using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Estructura de examenes para toma de muestra
[CreateAssetMenu(fileName = "New Exam", menuName = "Exam")]
public class PacientExam : ScriptableObject
{
    // Start is called before the first frame update
    public string examName;
    [TextArea(100, 1000)]
    public string examDescription;
    public Conditions conditions;
    public int reward;
    public int cost;
    public int reputation;
    public List<Item> requiredItems;

}
