using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Lista de pacientes para fakeinfo
[CreateAssetMenu(fileName = "Pacient Collection", menuName = "Collections/pacient")]
public class PacientCollection : ScriptableObject
{
    // Start is called before the first frame update
    public List<PacientData> pacientCollection;
}
