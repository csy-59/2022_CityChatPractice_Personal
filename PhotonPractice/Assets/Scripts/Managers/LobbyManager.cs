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

    public override void OnDisconnected(DisconnectCause cause)
    {
        setDeactiveStartButton("서버 접속 오류...! 재접속 시도합니다!");
        PhotonNetwork.ConnectUsingSettings();
    }

}
