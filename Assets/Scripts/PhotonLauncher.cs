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
//      Edit - ProjectSetting - Player 의 버젼을 가져오는 것

        // 마스터가 PhotonNetwork.LoadLevel()을 호출하면, 모든 플레이어가 동일한 레벨을 자동으로 로드
        // 시작은 동기화를 시키지만 끝나는 타이밍은 동기화를 시켜주지 못하기 때문에 따로 조정을 해줘야 한다.

        PhotonNetwork.AutomaticallySyncScene = true;
        //자동으로 씬을 동기화
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
            //포톤 클라우드에 접속을 시작하는 지점
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

        //롤 같은 게임은 접속을 하고 방을 찾는 버튼을 눌러야 한다.
        PhotonNetwork.JoinRandomRoom();
        //현재는 접속하자마자 방을 찾는 코드
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected: {0}", cause);
        
        connectButton.interactable = true;
        //이런 코드들도 함수화를 시켜줘야 한다.
        //캡슐화처럼 만들어줘야 한다.

        Debug.Log("Create Room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom } /*동적할당에서 특정 변수만 초기화*/);

        //이런 코드는 함수를 static으로 만들어서 사용하면 편하다.
        //static으로 함수를 만들경우 안에 변수도 static이어야 하지만 지금처럼 있는 것을 사용하는 경우는 무관
    }

    public override void OnJoinedRoom( )
    {
        Debug.Log("Joined Room");
        // 마스터가 동시에 게임을 시작하게 하는 구조가 아니기 때문에 각자 씬을 부르면 됨
        // PhotonNetwork,LoadLevel("Room") 레디를 하고 게임을 시작하는 경우 이 함수를 사용

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
