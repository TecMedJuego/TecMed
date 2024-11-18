using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Funcionalidades Input field para nombre de usuario
public class InputFieldGrabber : MonoBehaviour
{

    public string inputText;
    public GameObject reactionGroup;
    public TMP_Text reactionTextBox;
    public TMP_InputField inputField;
    public Button sendButton;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void CheckEmptyInput()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
            sendButton.interactable = false;
        else
            sendButton.interactable = true;

    }
    private void DisplayReactionToInput()
    {
        reactionTextBox.text = "Bienvenido/a " + inputText;
        reactionGroup.SetActive(true);
    }

    public void SendInputFieldText()
    {
        Debug.Log(inputField.text);
        PlayerData.Instance.SetName(inputField.text);
        AudioManager.Instance.PlaySFX("Confirm");
    }
}
