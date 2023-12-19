using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
  walk,
  attack
}

public class PlayerController : MonoBehaviour
{

      public PlayerState currentState;
      //player Move
      private Rigidbody2D _rb;
      public float _speed = 5;
      private float _moveInput;

      //animation
      private Animator animator;
     
     //player Jump
     private bool isGrounded;
     public float _jumpForce = 5;
     public Transform feetPos;
     public float checkRadius;
     public LayerMask Ground; 
     private float jumpTimeCounter;
     private bool isJumping;
     public float jumpingTime; 


     void Start() {
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();    
      }
      void FixedUpdate() {
        _moveInput = Input.GetAxis("Horizontal");
        MovingPlayer();
        
      }
      void MovingPlayer(){
        _rb.velocity = new Vector2(_moveInput * _speed, _rb.velocity.y);
         animator.SetFloat("moveX",_moveInput);
        if(Input.GetButtonDown("attack") && currentState != PlayerState.attack){
          StartCoroutine(AttackCo());
        }

       else if (_moveInput < 0)
        {
            // Jika bergerak ke kiri, putar pemain menghadap kiri
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_moveInput > 0)
        {
            // Jika bergerak ke kanan, putar pemain menghadap kanan
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Mengatur animator "moving" sesuai dengan gerakan
        animator.SetBool("moving", _moveInput != 0);
      }

      private IEnumerator AttackCo(){
        animator.SetBool("Attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(.2f);
        currentState = PlayerState.walk;
      }
      
       void Update() {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius,Ground);
        if(isGrounded == true && Input.GetKeyDown(KeyCode.Space)){
            isJumping = true;
            jumpTimeCounter = jumpingTime;
            _rb.velocity = Vector2.up * _jumpForce;
             animator.SetBool("jump", true);
             
        }
        if(Input.GetKey(KeyCode.Space) && isJumping == true){
            if(jumpTimeCounter > 0 ){
                _rb.velocity = Vector2.up * _jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }else{
                isJumping = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            isJumping = false;
             animator.SetBool("jump", false);
        }
      }

} 