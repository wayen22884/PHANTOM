using UnityEngine;

public class AttachCombo : MonoBehaviour, ICharacterAnimationSubscriber<EnemyCharacter>
{
    [SerializeField]
    private Combo combo;

    public void Subscribe(EnemyCharacter publisher)
    {
        publisher.combo = this.combo;
        Debug.Log("Player combo set");
    }
}
