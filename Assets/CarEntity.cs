using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEntity : MonoBehaviour
{
    public GameObject wheelFrontRight;
    public GameObject wheelFrontLeft;
    public GameObject wheelBackRight;
    public GameObject wheelBackLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        {
            m_Velocity = Mathf.Min(maxVelocity,
                m_Velocity + Time.fixedDeltaTime * acceleration);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_Velocity = Mathf.Max(-maxVelocity*0.03f,
                m_Velocity - Time.fixedDeltaTime * deceleration);
        }
        m_DeltaMovement = m_Velocity * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_FrontWheelAngle = Mathf.Clamp (
                m_FrontWheelAngle + Time.fixedDeltaTime * turnAngularVelocity,
                -WHEEL_ANGLE_LIMIT,
                WHEEL_ANGLE_LIMIT);

            UpdateWheels();
            
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle - Time.fixedDeltaTime * turnAngularVelocity,
                -WHEEL_ANGLE_LIMIT,
                WHEEL_ANGLE_LIMIT);

            UpdateWheels();

        }

        this.transform.Rotate(0f, 0f,
            1 / carLength *
            Mathf.Tan(Mathf.Deg2Rad * m_FrontWheelAngle) *
            m_DeltaMovement *
            Mathf.Rad2Deg);

        this.transform.Translate(Vector3.right * m_DeltaMovement);

    }
    public float Velocity { get { return m_Velocity; }}

    float m_FrontWheelAngle = 0;
    const float WHEEL_ANGLE_LIMIT = 40f;
    public float turnAngularVelocity = 5f;

    float m_Velocity;

    float carLength = 1.15f;

    [SerializeField] SpriteRenderer[] m_Renderers = new SpriteRenderer[5];

    public float acceleration = 3f;
    public float deceleration = 5f;
    public float maxVelocity = 60f;

    float m_DeltaMovement;

    void UpdateWheels ()
    {
        Vector3 localEulerAngles = new Vector3(0f, 0f, m_FrontWheelAngle);
        wheelFrontLeft.transform.localEulerAngles = localEulerAngles;
        wheelFrontRight.transform.localEulerAngles = localEulerAngles;

    }

    void ResetColor ()
    {
        ChangeColor(Color.white);
    }
    void ChangeColor (Color color)
    {
        foreach (SpriteRenderer r in m_Renderers)
        {
            r.color = color;
        }
    }

    void Stop ()
    {
        m_Velocity = 0;
    } 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Stop();
        ChangeColor(Color.red);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ResetColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckPoint checkPoint = other.gameObject.GetComponent<CheckPoint>();

        if (checkPoint != null)
        {
            ChangeColor(Color.green);
            this.Invoke("ResetColor", 0.1f);
        }
    }
}
