using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class P_Bullet : MonoBehaviourPun
{
    private bool isShoot = false;
    private Vector3 direction = Vector3.zero;
    private float speed = 10.0f;
    private float duration = 5.0f;
    private GameObject owner = null;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShoot)
        {
            this.transform.Translate(direction * speed * Time.deltaTime);
        }
    }
    public void Shoot(GameObject _owner, Vector3 _dir)
    {
        owner = _owner;
        direction = _dir;
        isShoot = true;

        if (photonView.IsMine) Invoke("SelfDestroy", duration);
    }

    private void SelfDestroy()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        if(owner != other.gameObject && other.CompareTag("Player")) //각자가 충돌처리를 하게되면 복사본까지 포함해서 데미지를 받기 때문에 원본만 처리를 해야한다
                                                                    // 최적화_ 원본만 충돌처리를 하면 되기 때문에 원본이 아닌 것은 콜라이더가 없어도 된다.
        {
            other.GetComponent<PlayerCtrl>().OnDamage(1);
            SelfDestroy();
        }
    }

}
