using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// Elemento UI para compra de examenes
public class ExamSlot : MonoBehaviour
{

    public PacientExam exam;
    public TMP_Text examName;
    public TMP_Text value;
    public bool unlocked;
    public GameObject greenLock;
    public GameObject price;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInfo()
    {
        CheckStatus(exam);
        if (unlocked)
        {
            greenLock.SetActive(true);
            price.SetActive(false);
        }
        else
        {
            greenLock.SetActive(false);
            price.SetActive(true);
        }
        
        examName.text = exam.examName;
        value.text = exam.cost.ToString();
    }
// revisa si el examen ya se encuentra desbloqueado
    public void CheckStatus(PacientExam exam)
    {
        if (GameManager.Instance.unlockedExams.Find(x => x == exam) != null)
            unlocked = true;
        else
            unlocked = false;
    }
}
