using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float PlayerHeight = 2;

    [SerializeField] Transform Orientation;

    [Header("Movement")]
    [SerializeField] float MoveSpd = 8;
    public float MoveMulti = 10;
    [SerializeField] float MaxRunSpd = 20f;
    [SerializeField] float airmulti = 0.4f;
    [Space]
    [SerializeField] float MaxSlopeAngle = 40f;

    float HorizontalMovement;
    float VerticalMovement;
    bool MoveInput;

    [Header("Jumping")]
    public float JumpHeight = 5;
    public float JumpCount = 2;
    float Jumps;

    [Header("Drag")]
    public float StaticDrag = 3;
    public float DynamicDrag = 0.5f;
    [SerializeField] float DragDelta = 0.4f;
    
    [Header("GroundDetect")]
    [SerializeField] Transform GroundCheck;
    [HideInInspector] public bool Grounded;
    [SerializeField] float CheckRadius = 0.2f;
    [SerializeField] LayerMask GroundLayer;

   

    Vector3 MoveDir;
    Vector3 slopeMoveDir;

    Rigidbody RB;
    RaycastHit SlopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position,Vector3.down, out SlopeHit,PlayerHeight * 0.5f + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);

            if (angle < MaxSlopeAngle && angle != 0)
                return true;
            else
                return false;
        }
        return false;
    }

   

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        RB.freezeRotation = true;

       

    }

    private void Update()
    {
        Grounded = Physics.CheckSphere(GroundCheck.position, CheckRadius, GroundLayer);

        PlayerInput();
        SurfaceCheck();
        CalculateDrag();

        if (Input.GetKeyDown(KeyCode.Space) && (Grounded || Jumps > 0))
            Jump();
    }

    void PlayerInput()
    {
        // Axis input Shizzle
        HorizontalMovement = Input.GetAxisRaw("Horizontal");
        VerticalMovement = Input.GetAxisRaw("Vertical");

        if (HorizontalMovement != 0 || VerticalMovement != 0)
            MoveInput = true;
        else
            MoveInput = false;

       

        MoveDir = (Orientation.forward * VerticalMovement)
                + (Orientation.right * HorizontalMovement);

        slopeMoveDir = Vector3.ProjectOnPlane(MoveDir, SlopeHit.normal);

    }

    void SurfaceCheck()
    {
        if (Grounded)        
            Jumps = JumpCount;

        if (OnSlope())
        {
            RB.useGravity = false;
            RB.AddForce(-SlopeHit.normal, ForceMode.Force);
        }
        else
            RB.useGravity = true;
       
    }

    void CalculateDrag()
    {
        if (MoveInput || !Grounded)
        {
            RB.drag = Mathf.Lerp(RB.drag, DynamicDrag, DragDelta * Time.deltaTime);
        }
        else if (!MoveInput)
        {
            RB.drag = Mathf.Lerp(RB.drag, StaticDrag, DragDelta * Time.deltaTime);
        }
    }

    void Jump()
    {

        RB.drag = DynamicDrag;

        float JumpForce = Mathf.Sqrt(-2 * Physics.gravity.y * JumpHeight);

        RB.AddForce(transform.up * (JumpForce - RB.velocity.y), ForceMode.Impulse);
        Jumps--;

       
    }    

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (RB.velocity.magnitude < MaxRunSpd)
        {
            if (Grounded && !OnSlope())
                RB.AddForce(MoveDir * MoveSpd * MoveMulti, ForceMode.Acceleration);
            else if (Grounded && OnSlope())
                RB.AddForce(slopeMoveDir * MoveSpd * MoveMulti, ForceMode.Acceleration);
            else if (!Grounded)
                RB.AddForce(MoveDir * MoveSpd * MoveMulti * airmulti, ForceMode.Acceleration);
        }
    }
}
