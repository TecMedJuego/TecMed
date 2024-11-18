using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Elemento UI generado en evaluacion cuando un paciente no respeta un requerimiento del examen
public class Condition : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text examName;
    public TMP_Text description;
    public Image image;


    public void SetInfo(string exam, string desc)
    {
        examName.text = exam;
        description.text = desc;
    }
}
