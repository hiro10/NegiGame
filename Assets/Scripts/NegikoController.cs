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

    // 基本Hp
    const int DefaultLife = 3;

    const float StunDirection = 0.5f;

    // ゲーム中ライフ
    int life = DefaultLife;

    float recoverTime = 0.0f;

    /// <summary>
    /// ライフ取得用関数
    /// </summary>
    /// <returns>ゲーム中ライフ</returns>
    public int Life()
    {
        return life;
    }

    /// <summary>
    /// 気絶判定
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
        // デバッグ用キー入力
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

        // スタン判定
        if (IsStun())
        {
            // 動きを止め気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speed_Z);
            //Clamp``挟み込む(SpeedZ最高速になったら)

            float rationX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = rationX * speed_X;
        }
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
    /// 左レーンに移動
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
    /// ジャンプ処理
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
    /// CharacterControllerに衝突判定が生じたとき処理
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun())
        {
            return;
        }

        if(hit.gameObject.tag=="Robo")
        {
            // ライフを減らして気絶状態に以降
            life--;
            recoverTime = StunDirection;

            // ダメージトリガーを設定
            animator.SetTrigger("damage");

            // ヒットしたオブジェクトの削除
            Destroy(hit.gameObject);
        }
    }
}
