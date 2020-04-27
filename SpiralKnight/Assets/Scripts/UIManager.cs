using System.Collections;
using System.Collections.Generic;
using Multiplayer.Client;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    [SerializeField] private Slider HealthBar;
    [SerializeField] private Text HealthText;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        GameManager.instance.isServer = false;
        Client.instance.ConnectToServer();
    }

    public void HostAndPlay()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Multiplayer.Server.Server.Start(50, 26950);
        GameManager.instance.isServer = true;
        ConnectToServer();
    }

    public void UpdateHealth()
    {
        HealthText.text = GameManager.LocalPlayer.health.ToString("0.00");
        HealthBar.value = GameManager.LocalPlayer.health / GameManager.LocalPlayer.maxHealth;
    }
}