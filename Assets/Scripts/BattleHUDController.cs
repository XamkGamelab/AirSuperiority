using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BattleHUDController : MonoBehaviour
{

    // UI elements for stats
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;
    public Slider healthSlider;
    public Slider shieldSlider;
    public Image gunImage;

    // Reference to the StatsManager.cs
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        UpdateHUDCoroutine();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Coroutine to update the HUD periodically
    private IEnumerator UpdateHUDCoroutine()
    {
        while (GameManager.Instance.updateHud && GameManager.Instance.isPlaying)
        {
            //Loop playerData and update HUD
            //StatsManager.Instance.player[player].CurrentGun.AmmoCount


            // You can adjust the wait time to update the HUD more or less frequently
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ChangeGunIcon(int index, Sprite sprite)
    {
        //When called, changes player[index] gun sprite 
    }

    // Method to update the UI based on the player's stats
    private void UpdateHUD()
    {

        // Update Text fields for Score, Health, Shield
        scoreText.text = "Score: " + player.Score;
        healthText.text = "Health: " + player.Health.ToString("F1"); // Rounded to 1 decimal place
        shieldText.text = "Shield: " + player.Shield.ToString("F1");

        // Update Slider values for Health and Shield (0-100 range)
        healthSlider.value = player.Health;
        shieldSlider.value = player.Shield;

    }

    // You can add methods for handling events like changing the gun, taking damage, etc.
}
