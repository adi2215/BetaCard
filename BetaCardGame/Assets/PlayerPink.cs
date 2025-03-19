using UnityEngine;

public class PlayerPink : MonoBehaviour
{
    private Rigidbody2D rb;

    public float jumpHeight = 4f;
    public float jumpTime = 1f;

    public bool jumping {get; private set; }
    public bool grounded {get; private set; }
    public bool ceiled {get; private set; }

    public Vector2 vel;
    private Vector2 position;

    public ParticleSystem dustEffect;

    [SerializeField] private AudioSource[] jumpSound;

    protected float jumpForce => (2f * jumpHeight) / (jumpTime / 2f);
    protected float heroGravity => (-2f * jumpHeight) / Mathf.Pow((jumpTime / 2f), 2);

    private RaycastHit2D objectHit;

    private CapsuleCollider2D _cap;

    public LayerMask layerMask;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        _cap = GetComponent<CapsuleCollider2D>();
        
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void GroundedMov()
    {
        jumping = vel.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            //dustEffect.Play();
            //jumpSound[0].Play();
            vel.y = jumpForce;
            jumping = true;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /*private void ApplyingGravity()
    {
        bool falling = vel.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 1.5f : 1f;

        vel.y += heroGravity * multiplier * Time.deltaTime;
        vel.y = Mathf.Max(vel.y, heroGravity / 1.5f);
    }

    private void FixedUpdate()
    {
        HeroPosition();
    }

    private void HeroPosition()
    {
        position = rb.position;
        position += vel * Time.fixedDeltaTime;

        rb.MovePosition(position);
    }

    public bool Raycast(Rigidbody2D rb, Vector2 direction, CapsuleCollider2D cap)
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            return false;
        }

        float distance = 0.1f;

        objectHit = Physics2D.CapsuleCast(cap.bounds.center, cap.size, cap.direction, 0,  direction.normalized, distance, layerMask);
        return objectHit.collider != null && objectHit.rigidbody != rb;
    }*/

}
