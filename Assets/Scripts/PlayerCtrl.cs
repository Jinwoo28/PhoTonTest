using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
<<<<<<< Updated upstream
using Photon.Realtime;

using UnityEngine.UI;
public class PlayerCtrl : MonoBehaviourPun //IPunInstantiateMagicCallback
=======


public class PlayerCtrl : MonoBehaviourPun, IPunInstantiateMagicCallback
>>>>>>> Stashed changes
{
    private Rigidbody rb = null;

    [SerializeField] private GameObject bulletPrefab = null;

    [SerializeField] private Color[] colors = null;
    [SerializeField] private float speed = 3.0f;
<<<<<<< Updated upstream
    Player[] players = PhotonNetwork.PlayerList;
=======



    int PlayerNum = 0;
>>>>>>> Stashed changes

    private int hp = 3;
    private bool isDead = false;

    private int colorNum = 0;

    PhotonView photonview = null;
    [SerializeField] Text PlayerName;

    Player player;

<<<<<<< Updated upstream
    public int PlayerNum;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonview = GetComponent<PhotonView>();

        //if (!photonview.IsMine) SetMaterial(PlayerNum);

=======
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
>>>>>>> Stashed changes
    }

    private void Start()
    {
        isDead = false;
<<<<<<< Updated upstream

        if (player == players[0]) SetMaterial(1);

    }

    [PunRPC]
    public void PlayerColorChange( Vector3 color_)
    {
        Color color = new Color(color_.x, color_.y, color_.z);
        gameObject.GetComponent<MeshRenderer>().material.color = color;

    }


    void Update()
=======
    }

    private void Update()
>>>>>>> Stashed changes
    {
        if (!photonView.IsMine) return;
        if (isDead) return;

        float X = Input.GetAxisRaw("Horizontal");
        float Z = Input.GetAxisRaw("Vertical");
        Vector3 Move = new Vector3(X, 0, Z).normalized;
        rb.velocity = Move * speed*Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) ShootBullet();
<<<<<<< Updated upstream
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
=======

        LookAtMouseCursor();
    }

    public void SetMaterial(int _playerNum)
    {
        Debug.LogError(_playerNum + " : " + colors.Length);
        if (_playerNum > colors.Length) return;

        this.GetComponent<MeshRenderer>().material.color = colors[_playerNum - 1];

        PlayerNum = _playerNum;
>>>>>>> Stashed changes
    }

    private void ShootBullet()
    {
        if (bulletPrefab)
        {
            GameObject go = PhotonNetwork.Instantiate(
                bulletPrefab.name,
                this.transform.position,
                Quaternion.identity);
            go.GetComponent<P_Bullet>().Shoot(this.gameObject, this.transform.forward);
        }
    }

    public void LookAtMouseCursor()
    {
        Vector3 mousePos = Input.mousePosition;
<<<<<<< Updated upstream
        Vector3 PlayerPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 dir = mousePos - PlayerPos;
=======
        Vector3 playerPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 dir = mousePos - playerPos;
>>>>>>> Stashed changes
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(-angle + 90.0f, Vector3.up);
    }

<<<<<<< Updated upstream
    [PunRPC] //Remote Processisor Call 
    //���� ȣ�� 
=======
    [PunRPC]
>>>>>>> Stashed changes
    public void ApplyHp(int _hp)
    {
        hp = _hp;
        Debug.LogErrorFormat("{0} Hp: {1}",
            PhotonNetwork.NickName,
            hp
            );

        if (hp <= 0)
        {
            Debug.LogErrorFormat("Destroy: {0}", PhotonNetwork.NickName);
            isDead = true;
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    public void OnDamage(int _dmg)
    {
        hp -= _dmg;
        photonView.RPC("ApplyHp", RpcTarget.Others, hp);
    }

    // PhotonNetwork.Instantiate�� ��ü�� �����Ǹ� ȣ��Ǵ� �ݹ��Լ�
    // -> IPunInstantiateMagicCallback �ʿ�
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //// ��ü���� �����ϱ� ������ �����͸� ó��
        if (!PhotonNetwork.IsMasterClient) return;

        //// ���ӸŴ����� ���ǵǾ� �ִ� �Լ� ȣ��
        FindObjectOfType<GameManager>().ApplyPlayerList();


    }

    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    // ��ü���� �����ϱ� ������ �����͸� ó��
    //    if (!PhotonNetwork.IsMasterClient) return;

    //    // ���ӸŴ����� ���ǵǾ� �ִ� �Լ� ȣ��
    //    FindObjectOfType<GameManager>().ApplyPlayerList();
    //}



}


