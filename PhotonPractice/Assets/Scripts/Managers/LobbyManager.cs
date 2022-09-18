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
        setDeactiveStartButton("서버 접속중입니다!");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        setActiveStartButton("서버 접속 완료!");
    }

    public void OnClickJoinButton()
    {
        if(nameInput.text.Length == 0)
        {
            informationText.text = "이름 입력 바랍니다!";
            return ;
        }

        setDeactiveStartButton("방 접속중입니다!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        informationText.text = "서버 접속 실패! 재접속 시도중입니다.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        setDeactiveStartButton("방 접속 완료!");
        PhotonNetwork.LoadLevel(1);
    }

    private RoomOptions roomOption = new RoomOptions()
    {
        MaxPlayers = 3
    };

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        informationText.text = "방 접속 실패! 새로운 방을 만든 중입니다.";
        PhotonNetwork.CreateRoom(null, roomOption, TypedLobby.Default, null);
    }

    public override void OnCreatedRoom()
    {
        setDeactiveStartButton("방 생성 완료!");
        PhotonNetwork.LoadLevel("Main");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        informationText.text = "방 생성 실패! 재시동 중입니다.";
        PhotonNetwork.CreateRoom(null, roomOption, TypedLobby.Default, null);
    }
}
