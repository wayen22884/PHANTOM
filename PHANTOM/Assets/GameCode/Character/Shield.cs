using UnityEngine;

public class Shield : MonoBehaviour, ICharacterAnimationSubscriber<ICharacter>
{
    private ICharacter publisher;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(this.spriteRenderer != null);
        this.spriteRenderer.enabled = false;
    }

    public void Subscribe(ICharacter publisher)
    {
        Debug.Log("Register shield");
        this.publisher = publisher;
        publisher.ShieldAnimationCallBack += this.onShieldLayerChange;
    }

    private void OnDestroy()
    {
        if (this.publisher != null)
        {
            this.publisher.ShieldAnimationCallBack -= this.onShieldLayerChange;
        }
    }

    private void onShieldLayerChange(int layer)
    {
        // TODO: Add transition
        this.spriteRenderer.enabled = layer > 0;
    }
}
