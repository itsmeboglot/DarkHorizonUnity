using Entities.Card;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Views.Cards;

[RequireComponent(typeof(Image))]
public class ChangCardColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color selectionColor;
    [SerializeField] private CardView card;
    
    private Color _normalColor;
    private Image _image;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _normalColor = _image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(card.IsSelected || card.IsUsed)
            return;
        
        _image.color = selectionColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(card.IsSelected || card.IsUsed)
            return;
        
        _image.color = _normalColor;
    }
}
