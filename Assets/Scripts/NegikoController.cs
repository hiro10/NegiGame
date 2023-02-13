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

    // ��{Hp
    const int DefaultLife = 3;

    const float StunDirection = 0.5f;

    // �Q�[�������C�t
    int life = DefaultLife;

    float recoverTime = 0.0f;

    /// <summary>
    /// ���C�t�擾�p�֐�
    /// </summary>
    /// <returns>�Q�[�������C�t</returns>
    public int Life()
    {
        return life;
    }

    /// <summary>
    /// �C�┻��
    /// </summary>
    /// <returns></returns>
    private bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }

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
        if (Input.GetKeyDown("left"))
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

        // �X�^������
        if (IsStun())
        {
            // �������~�ߋC���Ԃ���̕��A�J�E���g��i�߂�
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speed_Z);
            //Clamp``���ݍ���(SpeedZ�ō����ɂȂ�����)

            float rationX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = rationX * speed_X;
        }
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
        if(IsStun())
        {
            return;
        }
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
        if (IsStun())
        {
            return;
        }

        if (controller.isGrounded && targetLane < MaxLane)
        {
            targetLane++;
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    public void Jump()
    {
        if (IsStun())
        {
            return;
        }

        if (controller.isGrounded)
        {
            moveDirection.y = speed_Jump;

            animator.SetTrigger("jump");
        }
    }

    /// <summary>
    /// CharacterController�ɏՓ˔��肪�������Ƃ�����
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun())
        {
            return;
        }

        if(hit.gameObject.tag=="Robo")
        {
            // ���C�t�����炵�ċC���ԂɈȍ~
            life--;
            recoverTime = StunDirection;

            // �_���[�W�g���K�[��ݒ�
            animator.SetTrigger("damage");

            // �q�b�g�����I�u�W�F�N�g�̍폜
            Destroy(hit.gameObject);
        }
    }
}
