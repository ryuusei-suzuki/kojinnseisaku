using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.3f;
    [SerializeField] private float sprintSpeed = 5.6f;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform cameraTransform;   // 上下に回すカメラ
    [SerializeField] private float maxPitch = 85f;        // 上下の限界角度

    
    //LayerMaskどのレイヤーを対しょうにするか
    //insoectorで設定
    [SerializeField] private LayerMask groundLayer;
    //地面におく玉の半径
    [SerializeField] private float groundCheckRadius = 0.25f;

    //級の中心を足元からどれだけ上におくか(調整のためのもの)
    [SerializeField] private float groundChckOffset = 0.2f;

  
    [SerializeField] private float jumpPower = 8.3f;

    [SerializeField] private float gravity = 28f;

    private Rigidbody rb;
    private Vector3 inputDir;   // キー入力 x = 左右、z = 前後
    private float yaw;          // 左右の向き
    private float pitch;        // 上下の向き

    private bool isGrounded;
    private bool isSprinting;
    
    // jumpRequested【自作・変数】= 「ジャンプボタンが押された」というメモ
    private bool jumpRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        // ----- キーボード -----
        float x = 0f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;

        float v = 0f;
        if (Keyboard.current.wKey.isPressed) v += 1f;
        if (Keyboard.current.sKey.isPressed) v -= 1f;
        inputDir = new Vector3(x, 0f, v);

        isSprinting = Keyboard.current.leftShiftKey.isPressed && v >0f;
       
        // 「押された瞬間だけ true」になるプロパティを入れる
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;   // 「跳びたい」とメモしておく
        }

        // ----- マウス -----
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void FixedUpdate()
    {
        //球を置く位置
        //真上に足元からどれだけ話すかの値を× (0.1.0) * 0.2
        Vector3 checkPos = transform.position + Vector3.up * groundChckOffset;
        //(位置、半径、対象のLayer)
        isGrounded = Physics.CheckSphere(checkPos, groundCheckRadius, groundLayer);

        Quaternion lookRotation = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(lookRotation);
        Vector3 moveDir = lookRotation * inputDir.normalized;
        float currentSpeed;
        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
       
        Vector3 velocity = moveDir * currentSpeed;
        velocity.y = rb.linearVelocity.y;

        //下向きの速度 -= 重力 x 1回分の時間
        velocity.y -= gravity * Time.fixedDeltaTime;
        
        if (jumpRequested)
        {
            if (isGrounded == true)
            {
                velocity.y = jumpPower;
            }

            jumpRequested = false;
        }

        rb.linearVelocity = velocity;
    }


    private void OnDrawGizmos()
    {
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * groundChckOffset, groundCheckRadius);
    }
}