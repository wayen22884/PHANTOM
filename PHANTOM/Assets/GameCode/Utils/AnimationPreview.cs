using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;

public class AnimationPreview : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject triggerAnimationButton;
    [SerializeField]
    private Transform scrollViewContent;

    void Start()
    {
        Debug.Assert(this.animator != null);
        Debug.Assert(this.triggerAnimationButton != null);
        Debug.Assert(this.scrollViewContent != null);
        Debug.Assert(this.triggerAnimationButton.GetComponent<Button>() != null);
        Debug.Assert(this.triggerAnimationButton.GetComponentInChildren<Text>() != null);

        foreach (var state in this.getAnimatorStateInfo())
        {
            this.createAnimateButton(state);
        }
    }

    private void createAnimateButton(AnimatorState state)
    {
        var btn = Instantiate(this.triggerAnimationButton, scrollViewContent).GetComponent<Button>();
        btn.onClick.AddListener(() => this.animator.Play(state.name));
        var txt = btn.GetComponentInChildren<Text>();
        txt.text = state.name;
        btn.gameObject.SetActive(true);
    }

    // ref: https://www.reddit.com/r/Unity3D/comments/gzmmer/is_it_possible_to_get_list_of_states_from_animator/
    private List<AnimatorState> getAnimatorStateInfo()
    {
        AnimatorController ac = this.animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] acLayers = ac.layers;
        List<AnimatorState> allStates = new List<AnimatorState>();
        foreach (AnimatorControllerLayer layer in acLayers)
        {
            ChildAnimatorState[] animStates = layer.stateMachine.states;
            foreach (ChildAnimatorState chState in animStates)
            {
                allStates.Add(chState.state);
            }
        }
        return allStates;
    }
}
