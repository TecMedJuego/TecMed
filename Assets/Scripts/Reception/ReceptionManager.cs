using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
// Manager de recepción, controla los pacientes que llegan y asegura que no se sobrepongan en acciones
public class ReceptionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PacientAttention pacientAttention;

    //Informacion de creacion paciente
    public GameObject prefab;
    public Transform spawnPoint;
    public Transform receptionPoint;
    public Transform leavePoint;
    public Transform officePoint;
    private float _spawnTime = 5f;
    private float _spawnDeltaTime = 5f;
    public PacientCollection collection;
    public bool canSpawn;
    [SerializeField]
    public List<ReceptionChair> receptionChairs = new();

    [Serializable]
    public class ReceptionChair
    {
        public Transform charPoint;
        public bool filled;
        public ReceptionClock clock;
    }

    //Informacion de paciente
    [SerializeField]
    private List<Pacient> pacients = new();
    public Pacient currentPacient;

    public PacientCollection fakeInfos;

    public int pacientsPerDay;

    void Start()
    {
        pacientsPerDay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnDeltaTime > _spawnTime)
        {
            _spawnDeltaTime = 0f;
            canSpawn = pacients.Count < 6 && pacientsPerDay < GameManager.Instance.maxDailyPacients;
            _spawnTime = UnityEngine.Random.Range(5f, 30f);
            SpawnPacient();
        }
        else
        {
            _spawnDeltaTime += Time.deltaTime;
        }
    }
    // Creacion instancia de paciente en recepción
    public void SpawnPacient()
    {
        if (canSpawn)
        {
            var newPacient = Instantiate(prefab, spawnPoint.position, quaternion.identity, null);
            pacients.Add(newPacient.GetComponent<Pacient>());
            SetPacientInfo(newPacient.GetComponent<Pacient>());
            if (newPacient.GetComponent<Pacient>() == pacients[0])
            {
                currentPacient = newPacient.GetComponent<Pacient>();
                newPacient.GetComponent<Pacient>().PacientStateChange(Pacient.PacientState.ToReception);

            }
            else
            {
                newPacient.GetComponent<Pacient>().PacientStateChange(Pacient.PacientState.ToChair);
            }
            pacientsPerDay++;

        }
    }
    // Llama al siguiente paciente al completar la consulta del actual
    public void NextPacient()
    {
        pacients.Remove(pacients.Find(p => p = currentPacient));
        pacients.Where(item => item != null).ToList();
        if (pacients.Count > 0)
        {
            currentPacient = pacients[0];
            currentPacient.PacientStateChange(Pacient.PacientState.ToReception);
        }

    }
    // Completa los datos del paciente usando pacientgenerator
    private void SetPacientInfo(Pacient pacient)
    {

        var rand = UnityEngine.Random.Range(0, collection.pacientCollection.Count);
        PacientGenerator.instance.FillPacientInfo(pacient);
        pacient.SetSprites();
        rand = UnityEngine.Random.Range(0, 100);
        if (rand >= 85) pacient.incorrectInfo = true;
        else pacient.incorrectInfo = false;

    }
    // Activación de recepción
    public void SeeAPacient()
    {
        pacientAttention.Activate(true);
        pacientAttention.FillPersonalInfo();
    }
    // Remueve de la lista de paciente al paciente ya atendido 
    public void RemovePacient(Pacient pacient)
    {
        pacients.Remove(pacients.Find(p => p = pacient));
        pacients.Where(item => item != null).ToList();
    }
}
