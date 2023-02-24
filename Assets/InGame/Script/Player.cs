using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Player : MonoBehaviour
{
    public PhotonView PV;
    private Rigidbody rigid;

    private Animator anim;
    private float h = 0.0f;
    private float v = 0.0f;
    private float moveSpeed;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        moveSpeed = 5.0f;

        if (PV.IsMine)
        {
            var CM = GameObject.Find("CM_VCam").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
        }
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(PV.IsMine)
            PlayerMove();
    }

    void PlayerMove()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 moveDir = (transform.forward * v) + (transform.right * h);
        rigid.velocity = moveDir.normalized * moveSpeed;

        PlayerAnim(h, v);
        PlayerRot(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        if (h != 0.0f || v != 0.0f)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
    }

    void PlayerRot(float h, float v)
    {
        if (h > 0)
            gameObject.transform.GetChild(0).rotation = Quaternion.Slerp(gameObject.transform.GetChild(0).rotation, Quaternion.Euler(0, 90f, 0), Time.deltaTime * 8f);
        else if (h < 0)
            gameObject.transform.GetChild(0).rotation = Quaternion.Slerp(gameObject.transform.GetChild(0).rotation, Quaternion.Euler(0, -90f, 0), Time.deltaTime * 8f);
        if (v > 0)
            gameObject.transform.GetChild(0).rotation = Quaternion.Slerp(gameObject.transform.GetChild(0).rotation, Quaternion.Euler(0, 0f, 0), Time.deltaTime * 8f);
        else if (v < 0)
            gameObject.transform.GetChild(0).rotation = Quaternion.Slerp(gameObject.transform.GetChild(0).rotation, Quaternion.Euler(0, 180f, 0), Time.deltaTime * 8f);
    }
}
