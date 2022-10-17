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

    //�����Ͱ� �������� ���������� ������ ��
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //�� ��ü�� ���̳� �ൿ�� �̷�������� photonview-mine
        if (stream.IsWriting)
        {
            //������ ���� ����ü(Remote) �� ���� �־�� ���� 
            //������ �Լ� - stream. sendNext
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        if (stream.IsReading)//������ ���� ����ü(Remote) �϶� photonview-mine false �϶�  
        {
            setPos = (Vector3)stream.ReceiveNext();//��ó�� postion ���� �־�����. position��
            setRot = (Quaternion)stream.ReceiveNext();//rotatation �־����� rotation �� 
        }
    }
}
