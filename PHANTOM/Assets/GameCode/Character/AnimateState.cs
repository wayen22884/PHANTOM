using UnityEngine;

public class AnimateState : StateMachineBehaviour
{
    [SerializeField]
    private string animateObjectName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (Transform obj in animator.gameObject.transform)
        {
            obj.gameObject.SetActive(obj.name.Equals(this.animateObjectName));
        }
    }
}
