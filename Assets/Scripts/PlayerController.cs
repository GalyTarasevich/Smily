using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Animator animator;

    private Rigidbody2D _rigidBody;
    private Finish _finish;
    private LeverArm _leverArm;

    private float _horizontal = 0f;

    private bool _isFacingRight = true;
    private bool _isGround = false;
    private bool _isJump = false;
    private bool _isFinish = false;
    private bool _isLeverArm = false;

    const float speedMultipllier = 50f;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        _leverArm = FindObjectOfType<LeverArm>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal"); // -1, 1
        animator.SetFloat("speedX", Mathf.Abs(_horizontal)); 
        if (Input.GetKey(KeyCode.W) && _isGround) {
            _isJump = true;
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            if (_isFinish) {
                _finish.FinishLevel();
            } 
            if (_isLeverArm) {
                _leverArm.ActivateLeverArm();
            }
        }
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(_horizontal * speed * Time.fixedDeltaTime * speedMultipllier, _rigidBody.velocity.y);

        if (_isJump) {
            _rigidBody.AddForce(new Vector2(0f, 300f));
            _isGround = false;
            _isJump = false;
        }

        if (_horizontal > 0F && !_isFacingRight) {
            Flip();
        }
        else if (_horizontal < 0F && _isFacingRight) {
            Flip();
        }
    }
    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) {
            _isGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LeverArm leverArmTemp = other.GetComponent<LeverArm>();
        if (other.CompareTag("Finish")) {
            Debug.Log("Finish");
            _isFinish = true;
        }
        if (leverArmTemp != null) {
            _isLeverArm = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        LeverArm leverArmTemp = other.GetComponent<LeverArm>();
        if (other.CompareTag("Finish")) {
            Debug.Log("Not Finish");
            _isFinish = false;
        }
        if (leverArmTemp != null) {
            _isLeverArm = false;
        }
    }
}
