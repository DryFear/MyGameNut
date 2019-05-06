using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float wallDelay = 0.1f;
    public float checkDistanceForFloor = 0.1f;

    private bool wallJump = true;

    public GameObject sprite;

    private Rigidbody2D body;
    private Collider2D collider;

    public float dashTime = 0.3f;
    private float _dashTimer;
    private bool isDashNow = false;

    public float dashDelay = 0.5f;
    public float _dashDelayTimer;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        _dashTimer = 0;
        _dashDelayTimer = 0;
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.S) && CharacterStats.CharacterMove)
        {
            transform.position += Vector3.right * CharacterStats.CharacterSpeed * Time.deltaTime;
            CheckJump();
        }
        CheckDash();
        if (isDashNow)
        {
            _dashTimer -= Time.deltaTime;
            if(_dashTimer <= 0)
            {
                StopDash();
            }
        }
        if (!wallJump && Input.GetKeyDown(KeyCode.Space))
        {
            wallJump = true;
        }
        if(_dashDelayTimer > 0)
        {
            _dashDelayTimer -= Time.deltaTime;
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && collider.IsTouchingLayers())
        {
            body.AddForce(Vector2.up * CharacterStats.CharacterJumpForce);
        }
    }

    private void CheckDash() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashDelayTimer <= 0 && CharacterStats.CharacterMove)
        {
            StartDash();
            _dashDelayTimer = dashDelay;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.tag == "Wall")
        {
            CharacterStats.CharacterSpeed = -CharacterStats.CharacterSpeed;
            CharacterStats.CharacterDashForce = -CharacterStats.CharacterDashForce;
            sprite.GetComponent<SpriteRenderer>().flipX = !sprite.GetComponent<SpriteRenderer>().flipX;
            if (!Physics2D.Raycast(transform.position, Vector3.down, checkDistanceForFloor))
            {
                body.velocity = Vector3.zero;
                StartCoroutine(WallDelay());
            }

        }
    }

    private void StartDash()
    {
        isDashNow = true;
        body.velocity = CharacterStats.CharacterDashForce * Vector3.right;
        _dashTimer = dashTime;
    }

    private void StopDash()
    {
        isDashNow = false;
        body.velocity = Vector3.zero;
    }

    IEnumerator WallDelay()
    {
        body.gravityScale = 0;
        CharacterStats.CharacterMove = false;
        wallJump = false;
        yield return new WaitForSeconds(wallDelay);
        CharacterStats.CharacterMove = true;
        body.gravityScale = 1;
        if (wallJump)
        {

            body.AddForce(new Vector2(CharacterStats.CharacterJumpForce * Mathf.Sign(CharacterStats.CharacterSpeed), CharacterStats.CharacterJumpForce));
        }
        wallJump = true;
    }
}
