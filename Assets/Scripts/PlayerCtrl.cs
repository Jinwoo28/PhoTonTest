using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
public class PlayerCtrl : MonoBehaviourPun
{
    private Rigidbody rb = null;

    [SerializeField] private GameObject bulletPrefab = null;

    [SerializeField] private Color[] colors = null;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private Text Nickname_ = null;
    [SerializeField] private Slider HP = null;
    [SerializeField] private Canvas CV = null;

    private Camera cam = null;

    private int PlayerNum = 0;

    int hpMax = 3;


    private int hp = 3;
    private bool isDead = false;



    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        Nickname_.text = photonView.Owner.NickName;
    }
    void Start()
    {
        isDead = false;
        Debug.Log("hi");
    }



    void Update()
    {
        if (!photonView.IsMine) return;  //�� ��ǻ�Ϳ��� ������ �����̱� ���� ���� _ ���� �����̳� �ٸ� Ŭ���̾�Ʈ���� ���� �����̳��� ����
        if (isDead) return;

        float X = Input.GetAxisRaw("Horizontal");
        float Z = Input.GetAxisRaw("Vertical");
        Vector3 Move = new Vector3(X, 0, Z).normalized;
        rb.velocity = Move * speed;

        if (Input.GetMouseButtonDown(0)) ShootBullet();
        LookAtMouseCusor();

        HP.value = hp/hpMax;

        CV.transform.LookAt(CV.transform.position+cam.transform.rotation * Vector3.forward,cam.transform.rotation*Vector3.up);

        if (photonView.IsMine)
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                photonView.RPC("SetPlayerColor", RpcTarget.Others, PlayerNum);
            }
        }

    }



    public void SetMaterial(int _playerNum)
    {
            Debug.LogError(_playerNum + " : " + colors.Length);
            if (_playerNum > colors.Length) return;
            this.GetComponent<MeshRenderer>().material.color = colors[_playerNum - 1];
        PlayerNum = _playerNum - 1;
     }

    [PunRPC]
    public void SetPlayerColor(int colorNum)
    {
        this.GetComponent<MeshRenderer>().material.color = colors[colorNum];
        Nickname_.text = photonView.Owner.NickName;
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
        if (photonView.IsMine)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 PlayerPos = Camera.main.WorldToScreenPoint(this.transform.position);
            Vector3 dir = mousePos - PlayerPos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(-angle + 90.0f, Vector3.up);

        }
    }
    [PunRPC] //Remote Processisor Call 
    //���� ȣ�� 
    public void ApplyHp(int _hp)
    {
        hp = _hp;
        Debug.LogErrorFormat("{0} Hp : {1}", PhotonNetwork.NickName,hp);
        if (hp <= 0)
        {
            Debug.LogErrorFormat("Destroy: {0}", PhotonNetwork.NickName);
            isDead = true;
            PhotonNetwork.Destroy(this.gameObject);
            //���纻�� ���ÿ� �ı�
        }
    }
    
    [PunRPC]
    public  void OnDamage(int _dmg)
    {
        hp -= _dmg;
        photonView.RPC("ApplyHp", RpcTarget.Others, hp);
        //ü���� ���� ���� ����ȭ
    }



}
