using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody rb;
    private Vector3 inputDir;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;

        float v = 0f;
        if (Keyboard.current.wKey.isPressed) v += 1f;
        if (Keyboard.current.sKey.isPressed) v -=1f;
        inputDir = new Vector3(x, 0f, v);
        
        

    }

    private void FixedUpdate()
    {
        Vector3 velocity = inputDir * moveSpeed;
        velocity.y = 0f;
        rb.linearVelocity = velocity;
    }
}
