using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TextMeshProUGUI informationText;

    private void setActiveStartButton(string information)
    {
        informationText.text = information;
        startButton.interactable = true;
    }

    private void setDeactiveStartButton(string information)
    {
        informationText.text = information;
        startButton.interactable = false;
    }

    private void Awake()
    {
        setDeactiveStartButton("���� �������Դϴ�!");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        setActiveStartButton("���� ���� �Ϸ�!");
    }

    public void OnClickJoinButton()
    {
        if(nameInput.text.Length == 0)
        {
            informationText.text = "�̸� �Է� �ٶ��ϴ�!";
            return ;
        }

        setDeactiveStartButton("�� �������Դϴ�!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        informationText.text = "���� ���� ����! ������ �õ����Դϴ�.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        setDeactiveStartButton("�� ���� �Ϸ�!");
        PhotonNetwork.LoadLevel(1);
    }

    private RoomOptions roomOption = new RoomOptions()
    {
        MaxPlayers = 3
    };

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        informationText.text = "�� ���� ����! ���ο� ���� ���� ���Դϴ�.";
        PhotonNetwork.CreateRoom(null, roomOption, TypedLobby.Default, null);
    }

    public override void OnCreatedRoom()
    {
        setDeactiveStartButton("�� ���� �Ϸ�!");
        PhotonNetwork.LoadLevel("Main");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        informationText.text = "�� ���� ����! ��õ� ���Դϴ�.";
        PhotonNetwork.CreateRoom(null, roomOption, TypedLobby.Default, null);
    }
}
