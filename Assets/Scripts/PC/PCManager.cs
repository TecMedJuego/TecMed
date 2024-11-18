using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
// UI general para uso de PC y compras, principalmente valores y canvases
public class PCManager : MonoBehaviour
{

    public TMP_Text money;
    public TMP_Text reputation;
    
    private CanvasGroup canvasGroup;
    public CanvasGroup implementsGroup;
    public CanvasGroup examGroup;
    public List<ExamSlot> examList;

    // Start is called before the first frame update

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
    }

    // Update is called once per frame
    void Update()
    {
        money.text = GameManager.Instance.moneyText.text;
        reputation.text = GameManager.Instance.reputationText.text;
    }

    public void OpenPC(bool implements)
    {
                

        ManageCanvasGroup(canvasGroup, true);
        if (implements)
            ShowImplements();
        else
            ShowExams();

    }
    public void ClosePC()
    {
        AudioManager.Instance.PlaySFX("Close");
        ManageCanvasGroup(implementsGroup, false);
        ManageCanvasGroup(examGroup, false);
        ManageCanvasGroup(canvasGroup, false);
    }


    public void ShowImplements()
    {
        AudioManager.Instance.PlaySFX("Open");
        ManageCanvasGroup(examGroup, false);
        ManageCanvasGroup(implementsGroup, true);


    }

    public void ShowExams()
    {
        AudioManager.Instance.PlaySFX("Open");
        ManageCanvasGroup(implementsGroup, false);
        ManageCanvasGroup(examGroup, true);
        foreach (ExamSlot slot in examList)
        {
            slot.SetInfo();
        }
    }
    public void ManageCanvasGroup(CanvasGroup canvasGroup, bool on)
    {
        canvasGroup.alpha = on ? 1 : 0;
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
    }
}
