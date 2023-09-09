using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // Horizontal Movement
    public float h_timeTillMaxSpeed;
    public float h_maxSpeed;

    // Jump
    public bool limitAirJumps;
    public int maxJumps;
    public float jumpForce;
    public float holdForce;
    public float buttonHoldTime;
    public float distanceToCollider;
    public float maxJumpSpeed;
    public float maxFallSpeed;
    public float acceptedFallSpeed;
    public float glideTime;
    [Range(-2, 2)] public float gravity;
    public LayerMask collisionLayer;

    // Weapon
    public List<WeaponTypes> weaponTypes;
    public Transform gunBarrel;
    public Transform gunRotation;

    [HideInInspector] public float acceleration;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float runTime;
    [HideInInspector] public float jumpCountDown;
    [HideInInspector] public float fallCountDown;
    [HideInInspector] public int numberOfJumpsLeft;
    [HideInInspector] public bool isJumping;


    [HideInInspector] public bool isFacingLeft;
    [HideInInspector] public bool isGrounded;

    [HideInInspector] public Collider2D col;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public InputManager input;
    [HideInInspector] public ObjectPooler objectPooler;

    [HideInInspector] public List<GameObject> currentPool = new List<GameObject>();
    [HideInInspector] public GameObject currentProjectile;
    [HideInInspector] public GameObject projectileParentFolder;
    [HideInInspector] public bool shootEnd;

    private Vector2 facingLeft;
    private PlayerBaseState currentState;


    [HideInInspector] public PlayerIdleState idelState = new PlayerIdleState();
    [HideInInspector] public PlayerJumpState jumpState = new PlayerJumpState();
    [HideInInspector] public PlayerFallState fallState = new PlayerFallState();
    [HideInInspector] public PlayerWeaponState weaponState = new PlayerWeaponState();
    
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
        foreach(WeaponTypes weapon in weaponTypes) 
        {
            GameObject newPool = new GameObject();
            projectileParentFolder = newPool;
            objectPooler.CreatePool(weapon, currentPool, projectileParentFolder);
        }
        numberOfJumpsLeft = maxJumps;
        jumpCountDown = buttonHoldTime;
        fallCountDown = glideTime;
        currentState = idelState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    protected virtual void Initialization()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<InputManager>();
        objectPooler = ObjectPooler.Instance;
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public void Flip()
    {
        if (isFacingLeft)
        {
            transform.localScale = facingLeft;
        }
        else
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    public bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
    {
        RaycastHit2D[] hits = new RaycastHit2D[10];
        int numHits = col.Cast(direction, hits, distance);
        for (int i = 0; i < numHits; i ++)
        {
            if ((1 << hits[i].collider.gameObject.layer & collision) !=  0)
            {
                return true;
            }
        }
        return false;
    }

    public bool Falling(float velocity)
    {
        if (!isGrounded && rb.velocity.y < velocity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FallSpeed(float speed)
    {
        rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y * speed));
    }

    public void GroundCheck()
    {
        if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !isJumping)
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("DoubleJump", false);
            isGrounded = true;
            numberOfJumpsLeft = maxJumps;
            fallCountDown = glideTime;
        }
        else
        {
            anim.SetBool("Grounded", false);
            isGrounded = false;
            if (Falling(0) && rb.velocity.y < maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
            }
        }
        anim.SetFloat("VerticalSpeed", rb.velocity.y);
    }

    public void Movement()
    {
        if (input.MovementPressed(this))
        {
            anim.SetBool("Moving", true);
            acceleration = h_maxSpeed / h_timeTillMaxSpeed;
            runTime += Time.deltaTime;
            currentSpeed = horizontalInput * acceleration * runTime;
            CheckDirection();
        }
        else
        {
            anim.SetBool("Moving", false);
            acceleration = 0;
            runTime = 0;
            currentSpeed = 0;
        }
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    public void CheckDirection()
    {
        if (currentSpeed > 0)
        {
            if (isFacingLeft)
            {
                isFacingLeft = false;
                Flip();
            }
            if (currentSpeed > h_maxSpeed)
            {
                currentSpeed = h_maxSpeed;
            }
        }

        if (currentSpeed < 0)
        {
            if (!isFacingLeft)
            {
                isFacingLeft = true;
                Flip();
            }
            if (currentSpeed < -h_maxSpeed)
            {
                currentSpeed = -h_maxSpeed;
            }
        }

    }



    public void FireWeapon()
    {
        currentProjectile = objectPooler.GetObject(currentPool);
        if(currentProjectile != null) 
        {
            Invoke("PlaceProjectile", .1f);
        }
    }

    public void PlaceProjectile()
    {
        currentProjectile.transform.position = gunBarrel.position;
        currentProjectile.transform.rotation = gunRotation.rotation;
        currentProjectile.SetActive(true);
        if(!isFacingLeft)
        {
            currentProjectile.GetComponent<Projectile>().left = false;
        }
        else
        {
            currentProjectile.GetComponent<Projectile>().left = true;
        }
        currentProjectile.GetComponent<Projectile>().fired = true;
        anim.SetBool("ShootLoop", false);
    }

    public void ShootEnd()
    {
        anim.SetBool("Shooting", false);
        shootEnd = true;
    }
}
