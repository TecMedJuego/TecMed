using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Rendering;
// Función que guarda el nombre del usuario al inicio del juego
public class PlayerData : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerData Instance;
    [SerializeField] private string userName;

    void Awake()
    {
        InitSingleton();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetName(string inputName)
    {
        userName = inputName;
    }
    public string GetName()
    {
        return userName;
    }
}
