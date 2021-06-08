using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    // the value here is irrelevant. We just need it to pass as a reference to the SmoothDampAngle function.
    // Why is this necessary, then? Does the value usually matter?
    float turnVelocity;

    // Given a vector (x, y), this function returns the full angle (not reference angle) to that vector in radians.
    float ProperArctan(float x, float y) 
    {
        float opp;
        float adj;

        float ret = 0;
        if (y > 0 || (y == 0 && x < 0)) 
        {
            opp = Mathf.Abs(y);
            adj = Mathf.Abs(x);

            if (x < 0)
                ret = 3 * Mathf.PI / 2; 
        } else
        {
            opp = Mathf.Abs(x);
            adj = Mathf.Abs(y);

            if (x > 0)
                ret = Mathf.PI / 2;
            else
                ret = Mathf.PI;
        }

        //Debug.Log("offset " + (ret * Mathf.Rad2Deg).ToString());

        if (adj != 0)
            ret += Mathf.Atan2(opp, adj);

        return ret;
    }

    private void Start()
    {
        Debug.Log(ProperArctan(0, 1));
        Debug.Log(ProperArctan(1, 1));
        Debug.Log(ProperArctan(1, 0));
        Debug.Log(ProperArctan(1, -1));
        Debug.Log(ProperArctan(0, -1));
        Debug.Log(ProperArctan(-1, -1));
        Debug.Log(ProperArctan(-1, 0));
        Debug.Log(ProperArctan(-1, 1));
    }

    private void MakePlayerFaceCameraDirection() 
    {
        float cameraAngle = cameraTransform.eulerAngles.y;

        // This "SmoothDampAngle" function ensures that we smoothly look at the new direction instead of snapping to it.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void TryToMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float cameraAngle = cameraTransform.eulerAngles.y;

        if (horizontal != 0 || vertical != 0)
        {
            float cameraAngleRad = cameraAngle * Mathf.Deg2Rad;
            float movementDirectionalOffsetRad = ProperArctan(horizontal, vertical);
            float xMagnitude = Mathf.Sin(cameraAngleRad + movementDirectionalOffsetRad);
            float yMagnitude = Mathf.Cos(cameraAngleRad + movementDirectionalOffsetRad);

            // Ensure that the direction is relative to the camera's current direction.
            // "normalized" makes the vector's length 1 while keeping the proportions of its dimensions.
            Vector3 movement = new Vector3(xMagnitude, 0f, yMagnitude).normalized;
            controller.Move(speed * Time.deltaTime * movement);
        }
    }

    // Update is called once per frame
    void Update() 
    {
        MakePlayerFaceCameraDirection();
        TryToMove();
    }
}
