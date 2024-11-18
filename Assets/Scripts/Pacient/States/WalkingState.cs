using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class WalkingState : StateMachineBehaviour
{
    private Pacient pacient;
    private ReceptionManager receptionManager;
    private OfficeManager officeManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pacient = animator.gameObject.GetComponent<Pacient>();
        receptionManager = FindAnyObjectByType<ReceptionManager>();
        officeManager = FindAnyObjectByType<OfficeManager>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (pacient.state)
        {
            case Pacient.PacientState.ToReception:


                if (pacient.chairUsed != -1)
                {
                    receptionManager.receptionChairs[pacient.chairUsed].clock.GoToReception();
                    receptionManager.receptionChairs[pacient.chairUsed].filled = false;
                    pacient.chairUsed = -1;

                }
                var deskPath = receptionManager.receptionPoint.position - pacient.pivot.position;
                if (deskPath.magnitude >= 0.05)
                    pacient.MoveTo(receptionManager.receptionPoint);
                else
                {
                    GameManager.Instance.interactionButton.interactable = true;
                    if (GameManager.Instance.cameraState == GameState.Reception) { GameManager.Instance.InteractionStatus(true); }
                    else { GameManager.Instance.InteractionStatus(false); }
                    pacient.PacientStateChange(Pacient.PacientState.Wait);
                }
                break;

            case Pacient.PacientState.ToChair:

                int index = 0;
                foreach (ReceptionManager.ReceptionChair chair in receptionManager.receptionChairs)
                {
                    if (!chair.filled)
                    {
                        pacient.chairUsed = index;
                        var chairPath = chair.charPoint.position - pacient.pivot.position;
                        if (chairPath.magnitude >= 0.05)
                            pacient.MoveTo(chair.charPoint);
                        else
                        {
                            chair.filled = true;
                            chair.clock.StartTimer(pacient);
                            pacient.PacientStateChange(Pacient.PacientState.WaitInChair);
                        }
                        break;
                    }
                    index++;
                }
                break;

            case Pacient.PacientState.ToOffice:

                var officePath = receptionManager.officePoint.position - pacient.pivot.position;

                if (officePath.magnitude >= 0.05)
                    pacient.MoveTo(receptionManager.officePoint);
                else
                {
                    officeManager.currentPacient = pacient;
                    pacient.transform.position = officeManager.officePoint.transform.position;
                    pacient.PacientStateChange(Pacient.PacientState.ToConsult);
                    if (GameManager.Instance.cameraState != GameState.Office) GameManager.Instance.HighlightButton("Office");


                }
                break;

            case Pacient.PacientState.ToConsult:
                var consultPath = officeManager.consultPoint.transform.position - pacient.pivot.position;

                if (consultPath.magnitude >= 0.05)
                    pacient.MoveTo(officeManager.consultPoint);
                else
                {
                    GameManager.Instance.interactionButton.interactable = true;
                    if (GameManager.Instance.cameraState == GameState.Office) { GameManager.Instance.InteractionStatus(true); }
                    else { GameManager.Instance.InteractionStatus(false); }
                    pacient.PacientStateChange(Pacient.PacientState.Wait);

                }
                break;

            case Pacient.PacientState.Leave:
                if (pacient.chairUsed != -1)
                {
                    receptionManager.receptionChairs[pacient.chairUsed].filled = false;
                    pacient.chairUsed = -1;
                }
                var exitPath = receptionManager.leavePoint.position - pacient.pivot.position;
                if (exitPath.magnitude >= 0.05)
                    pacient.MoveTo(receptionManager.leavePoint);
                else
                    Destroy(animator.gameObject);
                break;

            case Pacient.PacientState.LeaveOffice:
                var leaveofficepath = officeManager.officePoint.position - pacient.pivot.position;
                if (leaveofficepath.magnitude >= 0.05)
                    pacient.MoveTo(officeManager.officePoint);
                else
                {
                    pacient.transform.position = receptionManager.officePoint.position;
                    if (GameManager.Instance.cameraState != GameState.Reception && GameManager.Instance.dailyPacients != GameManager.Instance.maxDailyPacients) GameManager.Instance.HighlightButton("Reception");
                    receptionManager.NextPacient();
                    pacient.PacientStateChange(Pacient.PacientState.Leave);
                }
                break;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
