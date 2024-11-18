using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Estructura de Item para implementos
[CreateAssetMenu(fileName = "New Item", menuName = "Medical Implement")]
public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite icon;
    public string description;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
