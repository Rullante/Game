using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Unity.Core;
using System;
using UnityEngine;
using UnityEngine.UI;



public class SplashScreenControlle : MonoBehaviour
{
	[SerializeField]
	private Button ConnectButton;
    [SerializeField]
    private InputField nameText;
    private string name;

    public string GetName()
    {
        return name;
    }

    public void AttemptSpatialOsConnection()
	{
		DisableConnectionButton();
		AttemptConnection();
	}

	private void DisableConnectionButton()
	{
		ConnectButton.interactable = false;
	}

	private void AttemptConnection()
	{
        name = nameText.text;
        if (name.Length >= 1 && name.Length < 15)
        {

            // Disable connect button
            ConnectButton.interactable = false;

            
            Debug.LogWarning("Nickname: "  + name);
            FindObjectOfType<Bootstrap>().ConnectToClient();
            StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.ClientConnectionTimeoutSecs, ConnectionTimeout));
        }
        
    }

	private void ConnectionTimeout()
	{
		if (SpatialOS.IsConnected)
		{
			SpatialOS.Disconnect();
		}

        
        ConnectButton.interactable = true;
	}
}
