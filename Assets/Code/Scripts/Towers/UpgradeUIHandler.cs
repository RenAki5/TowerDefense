using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //when the pointer enters the UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        //set mouse_over to true
        UIManager.main.SetHoveringState(true);
    }

    //when the pointer leaves the UI
    public void OnPointerExit(PointerEventData eventData)
    {
        //set mouse_over to false, and close the UI
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }

}
