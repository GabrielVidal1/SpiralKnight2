using System.Collections;
using System.Collections.Generic;
using Multiplayer.Client;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode W;
    [SerializeField] private KeyCode A;
    [SerializeField] private KeyCode S;
    [SerializeField] private KeyCode D;


    private Vector3 direction;
    
    private void Update()
    {
        direction = (2*new Vector3(Input.mousePosition.x / Screen.width, 0, Input.mousePosition.y / Screen.height) - new Vector3(1, 0, 1)).normalized;
        GameManager.players[Client.instance.myId].SetDirection(direction);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerAttack(direction);
        }
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(W),
            Input.GetKey(S),
            Input.GetKey(A),
            Input.GetKey(D)
        };

        ClientSend.PlayerMovement(_inputs, direction);
    }
}