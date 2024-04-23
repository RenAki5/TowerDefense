using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{

    [Header("References")]
    [SerializeField] TextMeshProUGUI currecnyUI;
    [SerializeField] TextMeshProUGUI rallyCurrencyUI;
    [SerializeField] TextMeshProUGUI livesUI;
    

    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    private void OnGUI()
    {
        currecnyUI.text = "Mana: " + LevelManager.main.currency.ToString();
        rallyCurrencyUI.text = "Rally: " + LevelManager.main.GetRallyCurrency().ToString();
        livesUI.text = "Lives: " + LevelManager.main.lives.ToString();
        

    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    public void SetSelected()
    {

    }

    public void TogglePause()
{
    isMenuOpen = !isMenuOpen;
    anim.SetBool("MenuOpen", isMenuOpen);

    if (isMenuOpen)
    {
        Time.timeScale = 0f; // Pause the game
    }
    else
    {
        Time.timeScale = 1f; // Resume the game
    }
}

}
