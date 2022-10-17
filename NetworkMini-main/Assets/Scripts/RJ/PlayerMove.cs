using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Mathf;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
	public float rotateSpeed = 5f;
	public float speed = 5f;

	public Transform rocket;

	
	[Header("Fire")]
	public GameObject[] bullet = new GameObject[2];
	public Transform firePos;
	public float power;
	private int currWeapon = 0;

	[Header("HP")]
	public Slider hpBar;
	int hp;
	public int maxHP = 4;

	[Header("Init Data")]
	[SerializeField] private TextMeshProUGUI nameText;
	// �ʱ� ���� ������
	public int actorNumber;
	public Color[] playerColors = new Color[4];
	public MeshRenderer[] rocketBodies;
	public Material rocketMaterial;

	// For Photon Network
	private Vector3 setPos;
	private Quaternion setRot;

	private Rigidbody rigid;

	public int HP
	{
		get { return hp; }
		set
		{
			hp = value;
			hpBar.value = hp;
		}
	}

	private void Awake()
    {
		rigid = GetComponent<Rigidbody>();
	}

    void Start()
	{
		hpBar.maxValue = maxHP;
		HP = maxHP;

		ApplyInitData();
	}

	void ApplyInitData()
    {
		actorNumber = (int)photonView.InstantiationData[0];
		nameText.text = (string)photonView.InstantiationData[1];

		var playerRocketMat = new Material(rocketMaterial);
		playerRocketMat.SetColor("_Color", playerColors[actorNumber - 1]);
        foreach (var body in rocketBodies)
        {
			body.material = playerRocketMat;
        }
    }

	void Update()
	{
		PosInterpolation();
        Area();
        Shoot();
	}

    private void FixedUpdate()
    {
		Move();
	}

    void Move()
	{
		if (photonView.IsMine)
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

            if (h != 0)
            {
				rocket.rotation = Quaternion.Euler(0, h > 0 ? 0 : 180, 0);
			}

			Vector3 dir = new Vector3(h, v, 0);
			dir.Normalize();
			rigid.AddForce(dir * power, ForceMode.Acceleration);
		}
		else //�� Ŭ���̾��� ����ü ��� �ƴѰܿ� - >Remote (���� ��ü) 
		{
			rocket.rotation = setRot;
		}
	}

	void PosInterpolation()
    {
        if (!photonView.IsMine)
        {
			transform.position = Vector3.Lerp(transform.position, setPos, Time.deltaTime * 10);
		}
	}


	void Area()
	{
		var curPos = transform.position;

        //x,y������ �����ؼ����������� �����̰� �ʹ�
        curPos.x = Clamp(curPos.x, -27, 27.66f);
		curPos.y = Clamp(curPos.y, -13.59f, 14.78f);

		transform.position = curPos;
	}

	private void ChangeWeapon(int weaponNumber)
    {

    }

	private void Shoot()
	{
		if (Input.GetKeyDown(KeyCode.Space) && photonView.IsMine && AquaTimer.Instance.IsShootable)
		{
			// ���� �߻�ó�� ���� �ʰ� ���忡�� �ñ�
			photonView.RPC(nameof(Fire), RpcTarget.AllBuffered, null);
		}
	}

	[PunRPC]
	void Fire()
    {
		var missile = Instantiate(bullet[currWeapon], firePos.position, rocket.rotation);
		var b = missile.GetComponentInChildren<Bullet>();
		b.missile.material = b.missileMats[actorNumber - 1];
	}

	private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
			if (HP >= 1)
			{
				//�浹�� �������� �и��� 
				float dirc = this.gameObject.transform.position.x - collision.gameObject.transform.position.x > 0 ? 1 : -1;
				rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode.Impulse);
				HP--;
			}
        }

		if (HP == 0)
		{
			//�浹 ���̾ �������� �����
			this.gameObject.layer = 10;
			this.transform.position += new Vector3(0, 0, 2f);
			this.gameObject.GetComponent<BoxCollider>().enabled = false;
			//�÷��� �ٲٰ�ʹ�
			var playerRocketMat = new Material(rocketMaterial);
			Color die = new Color(1, 1, 1, 0.4f);
			playerRocketMat.SetColor("_Color", die);
		}
    }


	//�����Ͱ� �������� ���������� ������ ��
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting == true) //�� ��ü�� ���̳� �ൿ�� �̷�������� photonview-mine
		{//������ ���� ����ü(Remote) �� ���� �־�� ���� 
		 //������ �Լ� - stream. sendNext
			stream.SendNext(this.transform.position);
			stream.SendNext(this.rocket.rotation);
		}
		if (stream.IsReading)//������ ���� ����ü(Remote) �϶� photonview-mine false �϶�  
		{
			setPos = (Vector3)stream.ReceiveNext();//��ó�� postion ���� �־�����. position��
			setRot = (Quaternion)stream.ReceiveNext();//rotatation �־����� rotation �� 
		}
	}
}
