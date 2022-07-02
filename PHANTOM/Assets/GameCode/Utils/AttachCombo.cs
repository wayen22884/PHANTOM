using UnityEngine;

public class AttachCombo : MonoBehaviour, ICharacterAnimationSubscriber<PlayerCharacter>
{
    [SerializeField]
    private Combo combo;

    public void Subscribe(PlayerCharacter publisher)
    {
        publisher.combo = this.combo;
        Debug.Log("Player combo set");
    }
}
