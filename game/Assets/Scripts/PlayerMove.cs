using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private float mouseSensitivity = 0.1f;
    //上下のカメラ
    [SerializeField] private Transform cameraTransform;
    //上下の限界角度
    [SerializeField] private float maxPitch = 85f;
    //ジャンプの高さ
    [SerializeField] private float jumpPower = 5f;
    private Rigidbody rb;   //PlayerのRigidbody
    private Vector3 inputDir;//キー入力 x=左右 z= 前後
    private float yaw;//左右の向き
    private float pitch;//上下の向き
    private bool jumpRequested;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;//カーソルを表示しない

        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        //キーボード
        float x = 0f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;

        float v = 0f;
        if (Keyboard.current.wKey.isPressed) v += 1f;
        if (Keyboard.current.sKey.isPressed) v -=1f;
        inputDir = new Vector3(x, 0f, v);

        if (Keyboard.current.spaceKey)
        
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        //マウスがどれだけ横に動いたか
        yaw += mouseDelta.x * mouseSensitivity;
        //上に動いたか
        pitch -= mouseDelta.y * mouseSensitivity;
        //Clamp(値,最小,最大)
        pitch = Mathf.Clamp(pitch, -maxPitch,maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);



    }

    private void FixedUpdate()
    {
        //yawの角度を回転に
        Quaternion lookRotation = Quaternion.Euler(0f,yaw,0f);

        rb.MoveRotation(lookRotation);

        //移動方向 向いている方向と正面で向いている方向への基準
        Vector3 moveDir = lookRotation * inputDir;

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }
}
