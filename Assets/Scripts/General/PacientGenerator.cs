using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
// Enums contienen Nombres masculinos, femeninos y apellidos comunes para chileno/as
public enum CommonMaleNamesChile
{
    Santiago,
    Matías,
    Sebastián,
    Mateo,
    Benjamín,
    Lucas,
    Tomás,
    Martín,
    Nicolás,
    Daniel,
    Joaquín,
    Felipe,
    Diego,
    Emiliano,
    Agustín,
    Juan,
    Vicente,
    Alexander,
    Samuel,
    Thiago,
    Julián,
    Andrés,
    Antonio,
    Rodrigo,
    Maximiliano,
    Pedro,
    Gabriel,
    Francisco,
    Óscar,
    José,
    Ignacio,
    Pablo,
    Ariel,
    Gustavo,
    Raúl,
    Fernando,
    Rafael,
    Enrique,
    Jorge,
    Javier,
    Alexis,
    Alan,
    Jonathan,
    Bruno,
    Iván,
    Marcos,
    Manuel,
    Alfredo,
    Fabián
}
public enum CommonFemaleNamesChile
{
    Sofía,
    Martina,
    Emilia,
    Isabella,
    Catalina,
    Florencia,
    Antonella,
    Amparo,
    María,
    Valentina,
    Renata,
    Maite,
    Agustina,
    Josefa,
    Amanda,
    Constanza,
    Francisca,
    Trinidad,
    Fernanda,
    Emma,
    Rafaela,
    Camila,
    Margarita,
    Olivia,
    Victoria,
    Paula,
    Rocío,
    Macarena,
    Julieta,
    Claudia,
    Javiera,
    Nicole,
    Gabriela,
    Daniella,
    Alejandra,
    Belén,
    Viviana,
    Adriana,
    Soledad,
    Karen,
    Tamara,
    Estefanía,
    Teresa,
    Andrea,
    Carla,
    Manuela,
    Marcela,
    Pamela,
    Flor
}
public enum CommonSurnamesChile
{
    González,
    Muñoz,
    Rojas,
    Díaz,
    Pérez,
    Soto,
    Contreras,
    Silva,
    Martínez,
    Sepúlveda,
    Morales,
    Rodríguez,
    López,
    Araya,
    Fuentes,
    Hernández,
    Torres,
    Espinoza,
    Flores,
    Castillo,
    Valenzuela,
    Ramírez,
    Reyes,
    Gutiérrez,
    Castro,
    Vargas,
    Álvarez,
    Vásquez,
    Tapia,
    Fernández,
    Sánchez,
    Cortés,
    Gómez,
    Herrera,
    Carrasco,
    Núñez,
    Miranda,
    Jara,
    Vergara,
    Rivera,
    Figueroa,
    García,
    Bravo,
    Riquelme,
    Vera,
    Vega,
    Molina,
    Campos,
    Sandoval,
    Olivares,
    Orellana,
    Zúñiga,
    Ortiz,
    Gallardo,
    Alarcón,
    Garrido,
    Salazar,
    Pizarro,
    Aguilera,
    Saavedra,
    Romero,
    Guzmán,
    Henríquez,
    Navarro,
    Peña,
    Aravena,
    Godoy,
    Cáceres,
    Parra,
    Leiva,
    Escobar,
    Yáñez,
    Valdés,
    Salinas,
    Vidal,
    Jiménez,
    Lagos,
    Ruiz,
    Cárdenas,
    Bustos,
    Medina,
    Maldonado,
    Pino,
    Moreno,
    Carvajal,
    Palma,
    Sanhueza,
    Poblete,
    Navarrete,
    Sáez,
    Toro,
    Donoso,
    Ortega,
    Venegas,
    Bustamante,
    Alvarado,
    Acevedo,
    Farías,
    Acuña,
    Guerrero,
    Cerda,
    Pinto,
    Paredes,
    Quezada,
    San_Martín,
    Toledo,
    Cornejo,
    Mora,
    Ramos,
    Arriagada,
    Arancibia,
    Velásquez,
    Hidalgo,
    Salas,
    Troncoso,
    Aguilar,
    Ulloa,
    Cabrera,
    Ríos,
    Inostroza,
    Rivas,
    Durán,
    León,
    Arias,
    Villarroel,
    Cuevas,
    Osorio,
    Marín,
    Calderón,
    Lara,
    Méndez,
    Chávez,
    Catalán,
    Ponce,
    Pacheco,
    Villalobos,
    Ojeda,
    Moya,
    Correa
}
[Serializable]
// Clase con las probabilidades de que un paciente lleve 2 o 3 examenes a una consulta, en base al día de juego
public class ExamPercentage
{
    [SerializeField] private int doubleExam;
    [SerializeField] private int tripleExam;

    public ExamPercentage(int doubleExam, int tripleExam)
    {
        this.doubleExam = doubleExam;
        this.tripleExam = tripleExam;

    }

    public int Getdouble()
    {
        return doubleExam;
    }

    public int GetTriple()
    {
        return tripleExam;
    }
}
// Generador de pacientes que elige sus condiciones, examenes y datos personales usando random
public class PacientGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public PacientData baseData;
    public static PacientGenerator instance;
    public SpriteLibraryCollection maleCollection;
    public SpriteLibraryCollection femaleCollection;
    public ExamCollection examCollection;
    string[] fakeRUTs = new string[]
    {
    "14.924.372-0",
    "8.145.637-2",
    "8.085.016-6",
    "16.527.038-K",
    "23.867.452-2",
    "9.263.564-3",
    "5.402.434-7",
    "10.655.438-2",
    "9.740.860-2",
    "22.750.701-2",
    "10.744.739-3",
    "23.477.497-2",
    "22.084.473-4",
    "31.971.069-2",
    "18.418.327-K",
    "23.514.912-5",
    "16.267.709-8",
    "16.935.558-5",
    "11.694.722-3",
    "22.407.869-K",
    "22.407.869-2",
    "21.598.854-1",
    "24.632.220-1",
    "17.371.234-0",
    "19.418793-9",
    "7.471.524-9",
    "9.107.570-9",
    "13.887.344-7",
    "21.039.128-2",
    "8.844.036-6",
    "13.977.378-0",
    "7.427.468-4",
    "17.186.392-9",
    "13.575.143-K",
    "8.559.892-9",
    "12.515.273-2",
    "21.781.036-1",
    "11.950.518-4",
    "11.135.195-3",
    "17.626.518-3",
    "20.108.953-0",
    "14.070.322-2",
    "7.807.680-1",
    "11.166.619-2",
    "23.163.625-0",
    "18.100.173-9",
    "10.606.995-6",
    "19.703.031-3",
    "23.750.178-0",
    "10.671.584-K",
    "11.575.708-1",
    "24.972.125-5",
    "16.979.857-5",
    "22.734.692-2",
    "5.599.713-6",
    "10.898.643-3",
    "23.930.979-8",
    "13.415.382-2",
    "8.405.868-8",
    "22.740.742-5",
    "23.997.852-5",
    "18.450.000-0",
    "23.446.416-4",
    "24.464.164-3",
    "19.427.866-7",
    "19.455.943-7",
    "15.365.987-7",
    "23.137.529-5",
    "17.150.366-3",
    "18.532.180-0",
    "13.735.455-1",
    "6.998.126-7",
    "9.230.720-4",
    "5.405.572-2",
    "9.630.889-2",
    "12.917.303-3",
    "16.756.786-K",
    "9.182.180-K",
    "6.645.249-2",
    "13.722.521-2",
    "20.709.798-5",
    "21.131.598-9",
    "22.203.145-1",
    "18.078.255-9",
    "22.513.878-8",
    "23.580.163-9",
    "22.697.477-6",
    "6.038.858-K",
    "15.679.588-7",
    "5.599.607-5",
    "19.101.042-6",
    "7.706.208-4",
    "9.352.805-0",
    "7.829.625-9",
    "24.460.406-4",
    "7.355.355-5",
    "9.293.281-8",
    "20.717.710-5",
    "12.614.020-7",
    "22.704.301-6",
    "24.667.782-4",
    "10.022.393-7",
    "8.463.005-5",
    "17.391.953-0",
    "8.963.722-8",
    "17.800.901-K",
    "8.394.770-5",
    "19.748.852-2",
    "8.776.451-6",
    "7.226.078-3",
    "10.389.758-0",
    "8.400.369-7",
    "5.574.319-3",
    "20.725.387-1",
    "12.147.382-9",
    "18.451.436-6",
    "8.254.256-6",
    "15.014.408-6",
    "10.058.849-8",
    "12.693.402-5",
    "12.322.088-9",
    "21.172.460-8",
    "19.411.360-9",
    "19.051.604.0",
    "8.890.906-2",
    "9.682.341-K",
    "24.032.778-5",
    "11.602.879-4",
    "10.861.087-5",
    "23.417.928-4",
    "16.967.082-K",
    "17.667.586-1",
    "6.248.563-9",
    "17.773.187-0",
    "24.888.665-K",
    "6.183.573-3",
    "11.823.011-6",
    "21.107.906-1",
    "20.483.446-6",
    "9.549.940-6",
    "12.796.317-7",
    "11.831.106-K",
    "10.237.253-0",
    "15.694.691-6",
    "20.862.651-5",
    "19.469.313-3",
    "7.673.264-7",
    "17.918.696-9",
    "17.919.998-K",
    "12.262.378-5",
    "15.058.181-8",
    "9.608.298-3",
    "16.846.009-0",
    "6.720.774-2",
    "14.345.191-7",
    "10.247.283-7"
    };

    [SerializeField] private List<ExamPercentage> ExamPercentages = new();
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // Función principal, entrega datos, condiciones y examenes al paciente generado
    public void FillPacientInfo(Pacient pacient)
    {
        bool gender = Convert.ToBoolean(UnityEngine.Random.Range(0, 2)); // false Male true Female
        PacientData newData = Instantiate(baseData) as PacientData;

        if (gender)
        {
            newData.personalInfo.gender = Gender.Female;
            int rand = UnityEngine.Random.Range(0, femaleCollection.spriteCollection.Count);
            newData.sprites = femaleCollection.spriteCollection[rand];
            newData.personalInfo.name = GetRandomEnum<CommonFemaleNamesChile>().ToString();
        }
        else
        {
            newData.personalInfo.gender = Gender.Male;
            int rand = UnityEngine.Random.Range(0, maleCollection.spriteCollection.Count);
            newData.sprites = maleCollection.spriteCollection[rand];
            newData.personalInfo.name = GetRandomEnum<CommonMaleNamesChile>().ToString();
        }
        newData.personalInfo.lastName = GetRandomEnum<CommonSurnamesChile>().ToString();
        newData.personalInfo.rut = fakeRUTs[UnityEngine.Random.Range(0, fakeRUTs.Length)];
        newData.exams = SetPacientExams();

        newData.conditions = GenerateConditions();
        pacient.data = newData;

    }
    // Generador de condiciones
    public Conditions GenerateConditions()
    {
        int food = UnityEngine.Random.Range(-1, 13);

        int smoke = UnityEngine.Random.Range(-10, 30);


        int drink = UnityEngine.Random.Range(-10, 30);



        bool exercise = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        bool sex = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        bool consent = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        int rand = UnityEngine.Random.Range(0, 100);
        Medications meds;
        if (rand < 25)
        {
            meds = Medications.ninguno;
        }
        else
        {
            meds = GetRandomEnum<Medications>();
        }
        return new Conditions(LessThan(food), exercise, LessThan(smoke), LessThan(drink), sex, meds, consent);
    }
    // Función selecciona los examenes del paciente y la cantidad de ellos
    public List<PacientExam> SetPacientExams()
    {
        int difficulty = GameManager.Instance.day - 1;

        int difDouble = UnityEngine.Random.Range(0, 100);
        int difTriple = UnityEngine.Random.Range(0, 100);
        var newList = new List<PacientExam>();
        var examList = new List<int>();
        for (int i = 0; i < examCollection.examCollection.Count; i++)
            examList.Add(i);

        var rand = examList[UnityEngine.Random.Range(0, examList.Count)];
        newList.Add(examCollection.examCollection[rand]);
        examList.Remove(examList.Find(x => x == rand));
        if (difDouble < ExamPercentages[difficulty].Getdouble())
        {
            rand = UnityEngine.Random.Range(0, examList.Count);
            newList.Add(examCollection.examCollection[rand]);
            examList.Remove(examList.Find(x => x == rand));
        }
        if (difTriple < ExamPercentages[difficulty].GetTriple())
        {
            rand = UnityEngine.Random.Range(0, examList.Count);
            newList.Add(examCollection.examCollection[rand]);
            examList.Remove(examList.Find(x => x == rand));
        }
        return newList;
    }
    // revisa que los randoms no generen un numero invalido
    public int LessThan(int number)
    {
        if (number < 2)
            return -1;
        else
            return number;
    }
    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
}
