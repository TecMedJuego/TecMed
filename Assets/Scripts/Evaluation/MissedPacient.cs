using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//Elemento UI generado para mostrar pacientes que se retiraron por tiempo
public class MissedPacient : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text pacientName;

    public Image image;



    public void SetInfo(string name, string surname, Sprite sprite)
    {
        pacientName.text = name + " " + surname;
        image.sprite = sprite;
    }
}
