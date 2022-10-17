using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject expFactory; //폭발 공장
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


            //3초 후에 폭발을 파괴하고싶다
            Destroy(exp, 3);
        }
    }


}
