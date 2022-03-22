using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingCameraEntity : MonoBehaviour
{
    public CarEntity targetObject;
    public float MOVING_THRESHOLD = 10f;
    Camera m_Camera;
    float m_OrthographicSize;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = this.GetComponent<Camera>();
        m_OrthographicSize = m_Camera.orthographicSize;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Delayed camera//
        //Vector3 deltaPos = targetObject.transform.position - this.transform.position;
        //Vector3 position = deltaPos * 0.9f * Time.deltaTime;

        //this.transform.position += new Vector3(position.x, position.y, 0);

        //Camera window//

        Vector2 deltaPos = this.transform.position - targetObject.transform.position;

        if (deltaPos.magnitude > MOVING_THRESHOLD)
        {
            deltaPos.Normalize();
            Vector2 newPosition = new Vector2(targetObject.transform.position.x,
                targetObject.transform.position.y) + deltaPos * MOVING_THRESHOLD;
            this.transform.position = new Vector3(newPosition.x, newPosition.y, this.transform.position.z);

            m_Camera.orthographicSize = m_OrthographicSize + targetObject.Velocity * 0.2f;
        }
    }
}
