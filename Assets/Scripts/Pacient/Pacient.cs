using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;
using System.Linq;
using UnityEngine.U2D.Animation;
using Unity.VisualScripting;

// Aciones de paciente generico
public class Pacient : MonoBehaviour
{
    // Start is called before the first frame update


    private ReceptionManager receptionManager;
    public PacientData data;

    public Animator animator;

    public Transform pivot;
    public SpriteRenderer img;
    private SpriteLibrary library;
    private float speed = 2.1f;

    public bool moving = false;

    public event Action<PacientState> StateChange;
    public PacientState state;
    public int chairUsed;
    public bool incorrectInfo;
    public enum PacientState
    {
        None,
        ToReception,
        GetNumber,
        ToChair,
        Wait,
        WaitInChair,
        ToOffice,
        LeaveOffice,
        ToConsult,
        Leave
    }

    void Awake()
    {
        library = this.GetComponent<SpriteLibrary>();

    }
    void Start()
    {
        chairUsed = -1;
        animator = GetComponent<Animator>();
        pivot = this.transform;
        img = GetComponent<SpriteRenderer>();

        receptionManager = FindAnyObjectByType<ReceptionManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
// Control de estados paciente animator
    public void PacientStateChange(PacientState newState)
    {
        state = newState;

        switch (newState)
        {
            case PacientState.None:
                break;
            case PacientState.ToReception:
                animator.SetBool("OnChair", false);
                animator.SetBool("Sitted", false);
                animator.SetBool("Moving", true);
                break;
            case PacientState.GetNumber:
                break;
            case PacientState.ToChair:
                animator.SetBool("Moving", true);
                break;
            case PacientState.Wait:
                animator.SetBool("Moving", false);
                break;
            case PacientState.WaitInChair:
                animator.SetBool("Moving", false);
                animator.SetBool("OnChair", true);
                break;
            case PacientState.ToOffice:
                animator.SetBool("Moving", true);
                break;
            case PacientState.LeaveOffice:
                animator.SetBool("Moving", true);
                break;
            case PacientState.ToConsult:
                animator.SetBool("Moving", true);
                break;
            case PacientState.Leave:
                animator.SetBool("OnChair", false);
                animator.SetBool("Sitted", false);
                animator.SetBool("Moving", true);
                break;
            default:
                break;
        }

        StateChange?.Invoke(newState);
    }
    // Mover paciente a uno de los puntos en la escena
    public void MoveTo(Transform target)
    {
        var dir = (target.position - pivot.position).normalized;

        img.flipX = !(dir.x >= 0);
        transform.Translate(speed * Time.deltaTime * dir);

    }


// Asigna libreria de sprites encontrada en playerdata
    public void SetSprites()
    {
        library.spriteLibraryAsset = data.sprites;
    }
}
