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
        // todo - make a "sprintPressedDown" and "sprintLetUp" set of functions to make this more concise.
        float adjustedSpeed;
        if (Input.GetButton("Sprint"))
        {
            adjustedSpeed = speed * 2;
        } else
        {
            adjustedSpeed = speed;
        }

        float cameraAngle = cameraTransform.eulerAngles.y;

        if (horizontal != 0 || vertical != 0)
        {
            // Mathf.Atan2 already does what I was doing manually. They handle the discontinuity for me.
            // This is the angle we are moving towards, calculated by the movement vector and offset by the camera.
            float movementAngleDeg = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + cameraAngle;

            /* Using a Quaternion is more efficient than the trig functions. "Normalized" is not necessary
             * because Vector3.forward already has a length of 1 - (0, 0, 1.0). Math is cool
             */
            Vector3 moveDir = Quaternion.Euler(0f, movementAngleDeg, 0f) * (Vector3.forward);

            controller.Move(adjustedSpeed * Time.deltaTime * moveDir);
        }
    }

    // Update is called once per frame
    void Update() 
    {
        MakePlayerFaceCameraDirection();
        TryToMove();
    }
}
