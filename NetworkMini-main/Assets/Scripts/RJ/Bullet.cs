using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject expFactory; //���� ����
    public float speed = 10;
    public GameObject bullet;
    public AudioClip explosionSound;

    public MeshRenderer missile;
    public Material[] missileMats;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        transform.Rotate(Vector3.up * 70 * Time.deltaTime);

        Destroy(bullet, 6);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(bullet);
            GameObject exp = Instantiate(expFactory);
            exp.transform.position = transform.position;

            SFXPlayer.Instance.PlaySpatialSound(transform.position, explosionSound);


            //3�� �Ŀ� ������ �ı��ϰ�ʹ�
            Destroy(exp, 3);
        }
    }


}
