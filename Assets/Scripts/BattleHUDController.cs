using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using static StatsManager;

public class BattleHUDController : MonoBehaviour
{
    private bool updatingHud = false;

    // UI elements for stats
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;
    public Slider healthSlider;
    public Slider shieldSlider;
    public Image gunImage;

    // Reference to the StatsManager.cs
    private StatsManager statsManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statsManager = StatsManager.Instance;
        UpdateHUD(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.updateHud && !updatingHud) //checking !updatingHud only to avoid performing loop on every update.
        {
            //Start Coroutine for updating Hud
            updatingHud = true;
            StartCoroutine(UpdateHUDCoroutine());
        }
        else if (!GameManager.Instance.updateHud)
        {
            //Stop Coroutine for updating Hud
            updatingHud = false;
        }
    }

    // Coroutine to update the HUD periodically
    private IEnumerator UpdateHUDCoroutine()
    {
        while (updatingHud)
        {
            UpdateHUD(0);  // Update HUD for Player 0 (or switch based on the active player)

            // You can adjust the wait time to update the HUD more or less frequently
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Method to update the UI based on the player's stats
    private void UpdateHUD(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= statsManager.player.Length)
            return;

        PlayerData player = statsManager.player[playerIndex];

        // Update Text fields for Score, Health, Shield
        scoreText.text = "Score: " + player.Score;
        healthText.text = "Health: " + player.Health.ToString("F1"); // Rounded to 1 decimal place
        shieldText.text = "Shield: " + player.Shield.ToString("F1");

        // Update Slider values for Health and Shield (0-100 range)
        healthSlider.value = player.Health;
        shieldSlider.value = player.Shield;

        // Update the gun image (check that the gun's sprite is part of the GunData)
        if (player.CurrentGun != null)
        {
            gunImage.sprite = player.CurrentGun.GunIcon;  // Set the gun's icon image
        }
    }

    // You can add methods for handling events like changing the gun, taking damage, etc.
}
