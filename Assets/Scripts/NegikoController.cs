using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegikoController : MonoBehaviour
{
    // ���[�����s�p
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1f;
    int targetLane;

    // �L�����N�^�[�R���g���[���[
    CharacterController controller;

    // �A�j���[�^�[   
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
        // �f�o�b�O�p�L�[����
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

        // �d�͕��̗͂𖈃t���[���ǉ�
        moveDirection.y -= gravity * Time.deltaTime;

        //�ړ����s
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        // �ړ����x��0�ȏ�Ȃ瑖���Ă���t���O��true��
        animator.SetBool("run", moveDirection.z > 0.0f);
        
    }
    /// <summary>
    /// �����[���Ɉړ�
    /// </summary>
    public void MoveLeftLane()
    {
        if(controller.isGrounded&&targetLane>MinLane)
        {
            targetLane--;
        }
    }

    /// <summary>
    /// �����[���Ɉړ�
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
