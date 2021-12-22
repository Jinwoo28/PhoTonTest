using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab = null;

    // 각 클라이언트 마다 생성된 플레이어 게임 오브젝트를 리스트로 관리
    private List<GameObject> playerGoList = new List<GameObject>();

<<<<<<< Updated upstream

=======
    public static GameManager Gm;
>>>>>>> Stashed changes

    private void Start()
    {
        if (playerPrefab != null)
        {
            GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                new Vector3(
                    Random.Range(-10.0f, 10.0f),
                    0.0f,
                    Random.Range(-10.0f, 10.0f)),
                Quaternion.identity,
                0);
            go.GetComponent<PlayerCtrl>().SetMaterial(PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    // PhotonNetwork.LeaveRooom 함수가 호출되면 호출
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        SceneManager.LoadScene("Launcher");
    }

    // 플레이어가 입장할 때 호출되는 함수
    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player Entered Room: {0}",
                        otherPlayer.NickName);
    }

    public void ApplyPlayerList()
    {
        // 전체 클라이언트에서 함수 호출
        photonView.RPC("RPCApplyPlayerList", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPCApplyPlayerList()
    {
<<<<<<< Updated upstream
        int playerCnt = PhotonNetwork.CurrentRoom.PlayerCount;      // playerCnt에 현재 방의 플레이어 수를 넣음
        // 플레이어 리스트가 최신이라면 건너뜀
        if (playerCnt == playerGoList.Count) return;
        //playerCnt와 플레이어GoList에 들어있는 플레이어 수가 같다면 PlayerCnt는 현재 방에 있는 플레이어 오브젝트가 모두 카운트 되었음.
=======
        int playerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        // 플레이어 리스트가 최신이라면 건너뜀
        if (playerCnt == playerGoList.Count) return;
>>>>>>> Stashed changes

        // 현재 방에 접속해 있는 플레이어의 수
        Debug.LogError("CurrentRoom PlayerCount : " + playerCnt);

        // 현재 생성되어 있는 모든 포톤뷰 가져오기
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        // 매번 재정렬을 하는게 좋으므로 플레이어 게임오브젝트 리스트를 초기화
        playerGoList.Clear();
<<<<<<< Updated upstream
        //playerCnt가 현재 방에 있는 플레이어 게임오브젝트보다 적을 경우 초기화를 통해 다시 순서대로 넣어줘야 하기 때문에 List를 초기화 시켜준다.
=======
>>>>>>> Stashed changes

        // 현재 생성되어 있는 포톤뷰 전체와
        // 접속중인 플레이어들의 액터넘버를 비교해,
        // 플레이어 게임오브젝트 리스트에 추가
        for (int i = 0; i < playerCnt; ++i)
        {
            // 키는 0이 아닌 1부터 시작
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // 만약 PhotonNetwork.Instantiate를 통해서 생성된 포톤뷰가 아니라면 넘김
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // 만약 현재 키 값이 딕셔너리 내에 존재하지 않는다면 넘김
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // 포톤뷰의 액터넘버
                int viewNum = photonViews[j].Owner.ActorNumber;
                // 접속중인 플레이어의 액터넘버
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                // 액터넘버가 같은 오브젝트가 있다면,
                if (viewNum == playerNum)
                {
                    // 게임오브젝트 이름도 알아보기 쉽게 변경
                    photonViews[j].gameObject.name = "Player_" + photonViews[j].Owner.NickName;
                    // 실제 게임오브젝트를 리스트에 추가
                    playerGoList.Add(photonViews[j].gameObject);
                }
            }
        }

        // 디버그용
        PrintPlayerList();
    }

    private void PrintPlayerList()
    {
        foreach (GameObject go in playerGoList)
        {
            if (go != null)
            {
                Debug.LogError(go.name);
            }
        }
    }

    // 플레이어가 나갈 때 호출되는 함수
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player Left Room: {0}",
                        otherPlayer.NickName);
    }

    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }
}


