using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour {
    Rigidbody2D rigid;
    public int nextMove;
    public float speed;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circlecollider;
    public bool fly;
    Vector3 pos; //현재위치
    float delta = 2.0f; // 좌(우)로 이동가능한 (x)최대값
    public bool immortal;

    void Start()
    {
        if (fly == true)
        {
            pos = transform.position;
        }
    }


    void Update()
    {
        if (fly == true)
        {
            Vector3 v = pos;
            v.y += delta * Mathf.Sin(Time.time * speed);
            transform.position = v;
        }
    }
    void Awake ()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circlecollider = GetComponent<CircleCollider2D>();

        Think();
    }
	
	void FixedUpdate ()
    {
        if (fly == true)
        {
          //  rigid.velocity = new Vector2(rigid.velocity.x, nextMove * speed);
            //rigid.velocity = new Vector2( nextMove * speed, rigid.velocity.x);
        }

        else
        {
            //Move 알아서 왼쪽이동
            rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);

            //Platform check
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null)
            {
                Turn();
            }
        }
    }
     
    //재귀함수
    void Think()
    {
        //set Next Active
        nextMove = Random.Range(-1, 2);

        //flip Sprite
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 2);
    }

    public void OnDamaged()
    {
        if(immortal == true)
        {
            rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            circlecollider.enabled = false;
            Invoke("MonsterCol", 2);
        }
        else
        {
            if(fly == true)
            {
                rigid.AddForce(Vector2.down * 20, ForceMode2D.Impulse);
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                circlecollider.enabled = false;
                Invoke("DeActive", 0.5f);
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                spriteRenderer.flipY = true;
                circlecollider.enabled = false;
                rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                Invoke("DeActive", 5);
            }
           
        }
       
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
    void MonsterCol()
    {
        circlecollider.enabled = true;
        spriteRenderer.color = new Color(255, 255, 255, 1);
    }
}
