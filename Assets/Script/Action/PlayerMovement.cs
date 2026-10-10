using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    /// <summary>移動速度</summary>
    [ReadOnly(true), SerializeField] float _moveSpeed = 8f;
    /// <summary>ジャンプ速度</summary>
    [SerializeField] float _jumpSpeed = 5f;
    /// <summary>ジャンプ中にジャンプボタンを離した時の上昇速度減衰率</summary>
    [SerializeField] float _gravityDrag = .2f;
    /// <summary>プレイヤー指定</summary>
    [SerializeField] GameObject _player;
    [SerializeField] Rigidbody2D _rb = default;
    [SerializeField] SpriteRenderer _sprite = default;
    /// <summary>接地フラグ</summary>
    [SerializeField] bool _isGrounded = false;
    Vector3 _initialPosition = default;
    AudioSource _audioSource;
    [SerializeField] Animator _anim = default;

    float h;
    float v;
    public float rote;

    InputAction _moveAction;
    InputAction _jumpAction;

    /// <summary>
    /// [外部変更用]ジャンプ力上昇のレート。倍率で設定する。
    /// </summary>
    public static float highJumpRate = 1f;
    /// <summary>
    /// [外部変更用]低速落下用のレート。基本的に3か1でOK。
    /// </summary>
    public static float levitation = 3;
    /// <summary>
    /// [外部変更用]移動速度のレート。倍率で設定。
    /// </summary>
    public static float highSpeed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        InputSystem.actions.Enable();
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _initialPosition = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }


    void Movement()
    {
        _moveSpeed = 6f;

        _rb.gravityScale = 3;
        h = _moveAction.ReadValue<Vector2>().x;
        if (h > 0)
        {
            _sprite.flipX = false;
        }
        else if (h < 0)
        {
            _sprite.flipX = true;
        }
        float velocity = _rb.linearVelocity.y;   // この変数 velocity に速度を計算して、最後に Rigidbody2D.velocity に戻す

        v = _jumpAction.ReadValue<float>();
        if (v > 0 && _isGrounded)
        {
            _anim.SetBool("Jumping?", true);
            velocity = _jumpSpeed;
            _isGrounded = false;
        }
        else if (v <= 0 && velocity > 0)
        {
            // 上昇中にジャンプボタンを離したら上昇を減速する
            velocity *= _gravityDrag;
        }

        if (velocity < -1)
        {
            _anim.SetBool("Jumping?", false);
            _anim.SetBool("Falling?", true);
            _rb.gravityScale = levitation;
        }

        _rb.linearVelocity = new Vector2(h * _moveSpeed, velocity);
        if (h != 0)
        {
            _anim.SetBool("Moving?", true);
        }
        else
        {
            _anim.SetBool("Moving?", false);
        }
        if (velocity == 0)
        {
            _anim.SetBool("Falling?", false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && !_isGrounded)
        {
            _anim.SetBool("Falling?", false);
            _isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && _isGrounded)
        {
            _isGrounded = false;
        }
    }
}
