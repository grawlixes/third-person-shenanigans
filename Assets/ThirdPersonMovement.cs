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

    // Update is called once per frame
    void Update()
    {
        /* Tasks:
         * 1. Make the camera look in the direction the player is facing.
         * 2. Make the player turn with the camera when moving with the mouse. DONE
         */

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float cameraAngle = cameraTransform.eulerAngles.y;

        // Ensure that the direction is relative to the camera's current direction.
        /* Todo: fix this, it doesn't work right now. How can we ensure that the player is always moving relative to the camera?
         * We can probably use the player's direction to help us, because it will always reflect where the camera's pointing.
         */
        Vector3 direction = new Vector3(horizontal * Mathf.Sin(cameraAngle), 0f, vertical * Mathf.Cos(cameraAngle)).normalized;

        // Make the player face the direction of the camera and current movement.
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraAngle;
        // This "SmoothDampAngle" function ensures that we smoothly look at the new direction instead of snapping to it.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
}
