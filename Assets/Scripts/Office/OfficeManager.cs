using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Maneja los elementos de la esena relacionados a la oficina
public class OfficeManager : MonoBehaviour
{
    // Start is called before the first frame update

    public PacientSample pacientSample;
    public GameObject prefab;

    public Transform officePoint;

    public Transform consultPoint;

    public Pacient currentPacient;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartConsult()
    {
        pacientSample.Activate(true);
    }
}
