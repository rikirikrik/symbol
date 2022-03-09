using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerControl : MonoBehaviour
{
    public float inputSpeed;
    public float jumpingPower;
    public LayerMask CollisionLayer;

    [SerializeField] private Animator animator;

    private Rigidbody2D rb2d;
    private float x_val;
    private float speed;
    private bool jumpFlg = false;
    private bool isMoving = false;
    private bool isGraund = false;
    private bool isDead = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        Sound.LoadSe("dead","Dead");
    }
    void Update()
    {
        x_val = Input.GetAxis("Horizontal");

        jumpFlg = IsCollision();
        isMoving = x_val != 0;

        //Spaceが押された場合
        if (Input.GetKeyDown("space") && jumpFlg)
        {
            jump();
        }

        animator.SetBool("isJump", jumpFlg);
        animator.SetBool("isMove", isMoving);
    }
    void FixedUpdate()
    {
        //待機
        if (x_val == 0)
        {
            speed = 0;
        }
        //右に移動
        else if (x_val > 0)
        {
            speed = inputSpeed;
            //右方向を向く
            transform.localScale = new Vector3(1, 1, 1);
        }
        //左に移動
        else if (x_val < 0)
        {
            speed = inputSpeed * -1;
            //左方向を向く
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // キャラクターを移動 Vextor2(x軸スピード、y軸スピード(元のまま)) 死んでなければね
        if (!isDead)
        {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }

    //接地判定
    bool IsCollision()
    {
        Vector3 left_SP = transform.position - Vector3.right * 0.2f;
        Vector3 right_SP = transform.position + Vector3.right * 0.2f;
        Vector3 EP = transform.position - Vector3.up * 0.1f;
        return Physics2D.Linecast(left_SP, EP, CollisionLayer)
               || Physics2D.Linecast(right_SP, EP, CollisionLayer);
    }

    void jump()
    {
        rb2d.AddForce(Vector2.up * jumpingPower);
        jumpFlg = false;
    }

    IEnumerator Dead()
    {
        animator.SetBool("isDamage", true);
        yield return new WaitForSeconds(0.5f);
        GetComponent<CircleCollider2D>().enabled = false;
        Sound.PlaySe("dead",0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Damage")
        {
            isDead = true;
            StartCoroutine("Dead");

        }

    }
}