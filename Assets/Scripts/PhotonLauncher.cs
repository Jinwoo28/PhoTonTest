using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.SceneManagement;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "0.0.1";
    [SerializeField] private byte maxPlayerPerRoom = 4;

    [SerializeField] private string nickName = string.Empty;

    [SerializeField] private Button connectButton = null;

    private void Awake()
    {
//        Application.version
//      Edit - ProjectSetting - Player �� ������ �������� ��

        // �����Ͱ� PhotonNetwork.LoadLevel()�� ȣ���ϸ�, ��� �÷��̾ ������ ������ �ڵ����� �ε�
        // ������ ����ȭ�� ��Ű���� ������ Ÿ�̹��� ����ȭ�� �������� ���ϱ� ������ ���� ������ ����� �Ѵ�.

        PhotonNetwork.AutomaticallySyncScene = true;
        //�ڵ����� ���� ����ȭ
    }
    void Start()
    {
        connectButton.interactable = true;
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(nickName))
        {
            Debug.Log("NickName is empty");
            return;
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogFormat("Connect : {0}", gameVersion);

            PhotonNetwork.GameVersion = gameVersion;
            //���� Ŭ���忡 ������ �����ϴ� ����
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnValueChangedNickName(string _nickName)
    {
        nickName = _nickName;
        PhotonNetwork.NickName = nickName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Connected to Master: {0}", nickName);
        connectButton.interactable = false;

        //�� ���� ������ ������ �ϰ� ���� ã�� ��ư�� ������ �Ѵ�.
        PhotonNetwork.JoinRandomRoom();
        //����� �������ڸ��� ���� ã�� �ڵ�
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected: {0}", cause);
        
        connectButton.interactable = true;
        //�̷� �ڵ�鵵 �Լ�ȭ�� ������� �Ѵ�.
        //ĸ��ȭó�� �������� �Ѵ�.

        Debug.Log("Create Room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom } /*�����Ҵ翡�� Ư�� ������ �ʱ�ȭ*/);

        //�̷� �ڵ�� �Լ��� static���� ���� ����ϸ� ���ϴ�.
        //static���� �Լ��� ������ �ȿ� ������ static�̾�� ������ ����ó�� �ִ� ���� ����ϴ� ���� ����
    }

    public override void OnJoinedRoom( )
    {
        Debug.Log("Joined Room");
        // �����Ͱ� ���ÿ� ������ �����ϰ� �ϴ� ������ �ƴϱ� ������ ���� ���� �θ��� ��
        // PhotonNetwork,LoadLevel("Room") ���� �ϰ� ������ �����ϴ� ��� �� �Լ��� ���

        SceneManager.LoadScene("Room");
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);
        connectButton.interactable = true;

        Debug.Log("Create Room");

        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }


}
