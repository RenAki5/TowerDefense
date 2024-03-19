using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private bool mouse_over = false;        //is the mouse over the upgrade UI


    //when the pointer enters the UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        //set mouse_over to true
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }

    //when the pointer leaves the UI
    public void OnPointerExit(PointerEventData eventData)
    {
        //set mouse_over to false, and close the UI
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }

}
