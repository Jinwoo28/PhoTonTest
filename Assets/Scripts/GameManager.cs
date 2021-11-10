using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject playerPrefab = null;
    void Start()
    {
        if(playerPrefab != null)
        {
                GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f)),
                Quaternion.identity, 0);
            //동시에 동기화를 통해 생성하는 함수
            // 서버에 들어가 있는 내용
            //파일명이 들어가야 만들어짐

            go.GetComponent<PlayerCtrl>().SetMaterial(PhotonNetwork.CountOfPlayers);
            
        }

    }

    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player Entered Room: {0}", otherPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player Left Room: {0}", otherPlayer.NickName);
    }
    public void LeaveRoom()
    {
        Debug.Log("Leave Room");
        PhotonNetwork.LeaveRoom();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
