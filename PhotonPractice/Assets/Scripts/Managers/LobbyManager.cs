using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject RoomButtonPrefab;

    [Header("Openning")]
    [SerializeField] private GameObject openningPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TextMeshProUGUI informationText;

    [Header("RoomSelect")]
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private ScrollRect roomListScrollRect;

    [Header("RoomCreate")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private GameObject roomCreatePanel;
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private Slider maxPlayerSlider;

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
        openningPanel.SetActive(true);
        roomPanel.SetActive(false);
        popupPanel.SetActive(false);
        roomCreatePanel.SetActive(false);

        setDeactiveStartButton("���� �������Դϴ�!");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        setActiveStartButton("���� ���� �Ϸ�!");
    }

    private string nickname;
    public void OnClickJoinButton()
    {
        nickname = nameInput.text;
        if(nickname.Length == 0)
        {
            informationText.text = "�̸� �Է� �ٶ��ϴ�!";
            return ;
        }


        nicknameText.text = $"�г���: {nickname}";
        showRoomPanel();
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

    public override void OnCreatedRoom()
    {
        setDeactiveStartButton("�� ���� �Ϸ�!");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        informationText.text = "�� ���� ����! ��õ� ���Դϴ�.";
        PhotonNetwork.CreateRoom(null, roomOption, TypedLobby.Default, null);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        setRoomList(roomList);
    }

    private void setRoomList(List<RoomInfo> roomList)
    {
        Debug.Log(roomList);
        foreach(var roomInfo in roomList)
        {
            GameObject newRoomButton = Instantiate(RoomButtonPrefab, roomListScrollRect.content);
            Button button = newRoomButton.GetComponent<Button>();
            button.interactable = (roomInfo.PlayerCount < roomInfo.MaxPlayers);
            string roomName = roomInfo.Name;
            button.onClick.AddListener(() => JoinSelectedRoom(roomName));

            TextMeshProUGUI[] text = newRoomButton.GetComponentsInChildren<TextMeshProUGUI>();
            text[0].text = $"�� �̸�: {roomInfo.Name}";
            text[1].text = $"�÷��̾� ��: {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        }
    }
    private void JoinSelectedRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName, null);
    }
    private void showRoomPanel()
    {
        setDeactiveStartButton("�κ� ���� ���Դϴ�!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        informationText.text = "�κ� ���� �Ϸ�!";
        roomPanel.SetActive(true);
    }


    public void ShowCreatRoomPanel()
    {
        popupPanel.SetActive(true);
        roomCreatePanel.SetActive(true);
    }

    public void CloseCreatRoomPanel()
    {
        popupPanel.SetActive(false);
        roomCreatePanel.SetActive(false);
    }

    public void CreateRoom()
    {
        if (roomNameInputField.text.Length == 0)
        {
            return;
        }

        RoomOptions newRoomOption = new RoomOptions
        {
            MaxPlayers = (byte)(maxPlayerSlider.value + 1)
        };

        PhotonNetwork.CreateRoom(roomNameInputField.text, newRoomOption, null, null);
    }

    public void ReturnToOpenning()
    {
        roomPanel.SetActive(false);
        openningPanel.SetActive(true);
    }
}
