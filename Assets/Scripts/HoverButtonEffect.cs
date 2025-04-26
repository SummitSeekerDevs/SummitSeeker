using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButtonEffect
    : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
{
    [SerializeField]
    private Image backgroundImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        backgroundImage.enabled = false;
    }
}
