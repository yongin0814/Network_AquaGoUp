using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerControlSync : MonoBehaviourPun
{
    private PhotonView view;

    private Vector3 setPos;
    private Quaternion setRot;

    float moveSpeed = 3.0f;
    float rotSpeed = 180.0f;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * (v * Time.deltaTime * moveSpeed));
            transform.eulerAngles += new Vector3(0, h, 0) * (rotSpeed * Time.deltaTime);
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.position, set)
            //this.transform.Translate(Vector3.forward * dir_pos2 * Time.deltaTime * mov_speed);
            //this.transform.eulerAngles += new Vector3(0, dir_pos1, 0) * rot_speed * Time.deltaTime;
        }
    }

    //데이터가 차곡차곡 순차적으로 오고가고 함
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //내 객체의 값이나 행동이 이루어졌을대 photonview-mine
        if (stream.IsWriting)
        {
            //상대방이 보는 내객체(Remote) 에 값을 주어야 겠지 
            //보내는 함수 - stream. sendNext
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        if (stream.IsReading)//상대방이 보는 내객체(Remote) 일때 photonview-mine false 일때  
        {
            setPos = (Vector3)stream.ReceiveNext();//맨처음 postion 으로 넣었으니. position값
            setRot = (Quaternion)stream.ReceiveNext();//rotatation 넣었으니 rotation 값 
        }
    }
}
