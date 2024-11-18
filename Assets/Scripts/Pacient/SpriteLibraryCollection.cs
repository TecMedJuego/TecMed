using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
// colleción de sprites humanos para generar paciente
[CreateAssetMenu(fileName = "Sprite Collection", menuName = "Collections/sprite")]

public class SpriteLibraryCollection : ScriptableObject
{
    // Start is called before the first frame update
    public List<SpriteLibraryAsset> spriteCollection;
}
