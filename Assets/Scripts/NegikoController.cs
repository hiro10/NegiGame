using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegikoController : MonoBehaviour
{
    // レーン走行用
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1f;
    int targetLane;

    // キャラクターコントローラー
    CharacterController controller;

    // アニメーター   
    Animator animator;

    Vector3 moveDirection;

    public float gravity;
    public float speed_Z;
    public float speed_X;
    public float speed_Jump;
    public float accelerationZ;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用キー入力
        if(Input.GetKeyDown("left"))
        {
            MoveLeftLane();
        }
        if (Input.GetKeyDown("right"))
        {
            MoveRigntLane();
        }
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }

        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speed_Z);

        float rationX = (targetLane * LaneWidth - transform.position.x)/LaneWidth;
        moveDirection.x = rationX * speed_X;

        // 重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        // 移動速度が0以上なら走っているフラグをtrueに
        animator.SetBool("run", moveDirection.z > 0.0f);
        
    }
    /// <summary>
    /// 左レーンに移動
    /// </summary>
    public void MoveLeftLane()
    {
        if(controller.isGrounded&&targetLane>MinLane)
        {
            targetLane--;
        }
    }

    /// <summary>
    /// 左レーンに移動
    /// </summary>
    public void MoveRigntLane()
    {
        if (controller.isGrounded && targetLane < MaxLane)
        {
            targetLane++;
        }
    }

    public void Jump()
    {
        if(controller.isGrounded)
        {
            moveDirection.y = speed_Jump;

            animator.SetTrigger("Jump");
        }
    }
}
