using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
public class PlayerCtrl : MonoBehaviourPun
{
    private Rigidbody rb = null;

    [SerializeField] private GameObject bulletPrefab = null;

    [SerializeField] private Color[] colors = null;
    [SerializeField] private float speed = 3.0f;

    private int hp = 3;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        isDead = false;
    }

    void Update()
    {
        if (!photonView.IsMine) return;  //내 컴퓨터에서 원본만 움직이기 위한 조건 _ 실제 원본이냐 다른 클라이언트에서 만든 원본이냐의 차이
        if (isDead) return;

        float X = Input.GetAxisRaw("Horizontal");
        float Z = Input.GetAxisRaw("Vertical");
        Vector3 Move = new Vector3(X, 0, Z).normalized;
        rb.AddForce(Move * speed);

        if (Input.GetMouseButtonDown(0)) ShootBullet();
        LookAtMouseCusor();
    }

    public void SetMaterial(int _playerNum)
    {
        Debug.LogError(_playerNum + " : " + colors.Length);
        if (_playerNum > colors.Length) return;
        this.GetComponent<MeshRenderer>().material.color = colors[_playerNum-1];
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

    [PunRPC]
    public void SetPlayerColor()
    {
        photonView.RPC("SetMaterial", RpcTarget.Others);
    }

}
