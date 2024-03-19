using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager main;

    private bool isHoveringUI;

    //Called when the script instance is loaded
    private void Awake()
    {
        main = this;    
    }

    //Set the hovering state for the upgrade UI
    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    //Current hoveringUI state
    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }

}
