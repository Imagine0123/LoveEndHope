using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D Body;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, Body.linearVelocity.y);
        
        if (Input.GetKey(KeyCode.Space))
        {
            Body.linearVelocity = new Vector2(Body.linearVelocity.x, speed);
        }
    }
}
