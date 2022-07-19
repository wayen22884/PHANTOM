using UnityEngine;

public class HoverAndChangeSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite toChange;

    private Sprite original;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.original = this.spriteRenderer.sprite;
    }

    void OnMouseOver()
    {
        this.spriteRenderer.sprite = this.toChange;
    }

    void OnMouseExit()
    {
        this.spriteRenderer.sprite = this.original;
    }
}
