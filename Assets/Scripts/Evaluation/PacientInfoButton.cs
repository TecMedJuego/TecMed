using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Elemento UI boton para acceder a información especifica de un paciente
public class PacientInfoButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    public Image background;
    private Consult pacientConsult;
    public bool rejected;
    public Sprite att;
    public Sprite reje;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetData(Consult pacient, bool rej)
    {
        pacientConsult = pacient;
        image.sprite = pacientConsult.pData.GetExamSprite();
        rejected = rej;
        if(rejected) background.sprite = reje;
        else background.sprite = att;
    }

    public void ShowInfo()
    {
        EvaluationManager.Instance.ShowSelectedInfo(pacientConsult,rejected);
    }
}
