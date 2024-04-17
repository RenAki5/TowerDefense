using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{

    [Header("References")]
    [SerializeField] TextMeshProUGUI currecnyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    private void OnGUI()
    {
        currecnyUI.text = LevelManager.main.currency.ToString();
        rallyCurrencyUI.text = LevelManager.main.GetRallyCurrency().ToString();

    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    public void SetSelected()
    {

    }
}
