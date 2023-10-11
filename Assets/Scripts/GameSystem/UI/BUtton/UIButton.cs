using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData) {
        AudioManager.Instance.PlaySFX("buttonHighlight");
    }
    public void OnPointerDown(PointerEventData eventData) {
        AudioManager.Instance.PlaySFX("buttonPress");
    }
}
