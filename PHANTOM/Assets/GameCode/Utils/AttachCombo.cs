using UnityEngine;

public class AttachCombo : MonoBehaviour, ICharacterAnimationSubscriber<EnemyCharacter>, ICharacterAnimationSubscriber<PlayerCharacter>
{
    [SerializeField]
    private Combo combo;

    public void Subscribe(EnemyCharacter publisher)
    {
        publisher.combo = this.combo;
        Debug.Log("Enemy combo set");
    }

    public void Subscribe(PlayerCharacter publisher)
    {
        publisher.combo = this.combo;
        Debug.Log("Player combo set");
    }
}
