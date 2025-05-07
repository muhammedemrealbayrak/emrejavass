using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{

    public CharacterController2D controller;

    public Animator animator;

    public float runSpeed = 40f;

    float horizantalMove = 0f;
    bool jump = false;

    // Update is called once per frame
    void Update()
    {   

        horizantalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizantalMove));
       /* 
        if (Input.GetButtonUp("Roll"))
        {
            jump = true;
            animator.SetBool("Roll", true);

            horizantalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            controller.Move(horizantalMove * Time.fixedDeltaTime, false, roll);
            roll = false;
       }
        */
        if (Input.GetButtonDown("Jump"))
        {

            jump = true;
            animator.SetBool("IsJumping", true);
            runSpeed = 0f;
            animator.SetBool("Speed", false);
        }
        else if (jump == false) {
            runSpeed = 40f;
        }


    }
    void Start()
    {
        // Yere iniþ event'ine abone ol
        controller.OnLandEvent.AddListener(OnLanding);
    }

    public void OnLanding()
    {

        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {


        controller.Move(horizantalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

    }

}
