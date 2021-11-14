using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;
public class PlayerCtrl : MonoBehaviourPun
{
    private Rigidbody rb = null;

    [SerializeField] private GameObject bulletPrefab = null;

    [SerializeField] private Color[] colors = null;
    [SerializeField] private float speed = 3.0f;
    Player[] players = PhotonNetwork.PlayerList;

    private int hp = 3;
    private bool isDead = false;

    private int colorNum = 0;

    PhotonView photonview = null;
    [SerializeField] Text PlayerName;

    Player player;

    public int PlayerNum;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonview = GetComponent<PhotonView>();

        //if (!photonview.IsMine) SetMaterial(PlayerNum);

    }
    void Start()
    {
        isDead = false;

        if (player == players[0]) SetMaterial(1);

    }

    [PunRPC]
    public void PlayerColorChange( Vector3 color_)
    {
        Color color = new Color(color_.x, color_.y, color_.z);
        gameObject.GetComponent<MeshRenderer>().material.color = color;

    }


    void Update()
    {
        if (!photonView.IsMine) return;  //내 컴퓨터에서 원본만 움직이기 위한 조건 _ 실제 원본이냐 다른 클라이언트에서 만든 원본이냐의 차이
        if (isDead) return;

        float X = Input.GetAxisRaw("Horizontal");
        float Z = Input.GetAxisRaw("Vertical");
        Vector3 Move = new Vector3(X, 0, Z).normalized;
        rb.velocity = Move * speed;

        if (Input.GetMouseButtonDown(0)) ShootBullet();
        LookAtMouseCusor();
    }
    [PunRPC]
    public void SetMaterial(int _playerNum)
    {

            Debug.LogError(_playerNum + " : " + colors.Length);
            if (_playerNum > colors.Length) return;
            this.GetComponent<MeshRenderer>().material.color = colors[_playerNum-1];
    }

    public void SetPlayer(int Num, Player player)
    {
            PlayerNum = Num;
        players[Num-1] = player;
    }


    public void SetUp(Player player_)
    {
        player = player_;
        PlayerName.text = player_.NickName;
    }

    private void ShootBullet()
    {
        if (bulletPrefab)
        {
            GameObject go = PhotonNetwork.Instantiate(bulletPrefab.name, this.transform.position, Quaternion.identity);
            go.GetComponent<P_Bullet>().Shoot(this.gameObject, this.transform.forward);
        }
    }

    public void LookAtMouseCusor()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 PlayerPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 dir = mousePos - PlayerPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(-angle + 90.0f, Vector3.up);
    }

    [PunRPC] //Remote Processisor Call 
    //원격 호출 
    public void ApplyHp(int _hp)
    {
        hp = _hp;
        Debug.LogErrorFormat("{0} Hp : {1}", PhotonNetwork.NickName,hp);
        if (hp <= 0)
        {
            Debug.LogErrorFormat("Destroy: {0}", PhotonNetwork.NickName);
            isDead = true;
            PhotonNetwork.Destroy(this.gameObject);
            //복사본이 동시에 파괴
        }
    }
    
    [PunRPC]
    public  void OnDamage(int _dmg)
    {
        hp -= _dmg;
        photonView.RPC("ApplyHp", RpcTarget.Others, hp);
        //체력을 깎은 다음 동기화
    }





}
