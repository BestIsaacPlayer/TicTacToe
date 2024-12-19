using UnityEngine;

namespace Board.Cell
{
    public class Controller : MonoBehaviour
    {
        [Header("Sprite Setup")]
        [SerializeField] public SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite xSprite;
        [SerializeField] private Sprite oSprite;
        public Content Content { get; private set; } = Content.Empty;

        private void Awake()
        {
            if (!spriteRenderer) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(spriteRenderer))} field in the {gameObject.name} object is unset!");
            if (!xSprite) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(xSprite))} field in the {gameObject.name} object is unset!");
            if (!oSprite) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(oSprite))} field in the {gameObject.name} object is unset!");
        }

        public void MarkCell(Content content, bool isTemporary = false)
        {
            Content = content;
            
            if (!isTemporary)
            {
                spriteRenderer.sprite = content switch
                {
                    Content.X => xSprite,
                    Content.O => oSprite,
                    _ => null
                };
            }
        }
    }
}