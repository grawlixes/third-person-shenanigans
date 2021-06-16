using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public GameObject limboObject;

    private const float WALKING_SPEED = 6f;
    private const float RUNNING_SPEED = 12f;
    private const float TURN_SMOOTH_TIME = 0.1f;

    private float speed = WALKING_SPEED;
    private bool flashing = false;
    private bool teleporting = false;

    private float turnVelocity;

    void Start()
    {
        limboObject.SetActive(false);
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            flashing = true;
            limboObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            flashing = false;
            teleporting = true;
        }

        if (Input.GetButtonDown("Sprint")) 
        {
            speed = RUNNING_SPEED;
        } 
        else if (Input.GetButtonUp("Sprint"))
        {
            speed = WALKING_SPEED;
        }
    }

    private void MakePlayerFaceCameraDirection() 
    {
        float cameraAngle = cameraTransform.eulerAngles.y;

        // This "SmoothDampAngle" function ensures that we smoothly look at the new direction instead of snapping to it.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnVelocity, TURN_SMOOTH_TIME);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private bool CanMove()
    {
        return !(flashing || teleporting);
    }

    private void TryToMove()
    {
        /* Getting this input could be a part of the CheckInput function, and horizontal/vertical
         * could be class-level variables, but that would be kind of unintuitive.
         */
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float cameraAngle = cameraTransform.eulerAngles.y;

        if (horizontal != 0 || vertical != 0)
        {
            // Mathf.Atan2 already does what I was doing manually with ProperAtan. They handle the discontinuity for me.
            // This is the angle we are moving towards, calculated by the movement vector and offset by the camera.
            float movementAngleDeg = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + cameraAngle;

            /* Using a Quaternion is more efficient than the trig functions. "Normalized" is not necessary
             * because Vector3.forward already has a length of 1 - (0, 0, 1.0). Math is cool
             */
            Vector3 moveDir = Quaternion.Euler(0f, movementAngleDeg, 0f) * (Vector3.forward);
            controller.Move(speed * Time.deltaTime * moveDir);
        }
    }

    // This is called when the user lets go of the Flash key.
    private void FlashTeleport()
    {
        /* Set the player's world position to that of the Limbo object.
         * Need to temporarily disable the controller to do this, or else it won't move.
         * As far as I know, there's no way to move the Controller's world position.
         */
        controller.enabled = false;
        controller.transform.position = limboObject.transform.position;
        controller.enabled = true;

        // Reset the Limbo object.
        limboObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        limboObject.SetActive(false);

        teleporting = false;
    }

    // This is called every frame while the user holds down the Flash key.
    private void FlashUpdate()
    {
        float newZ = Mathf.Max(0f, Input.mousePosition.y);
        limboObject.transform.localPosition = new Vector3(0f, 0f, newZ / 30f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        MakePlayerFaceCameraDirection();

        if (CanMove())
        {
            TryToMove();
        }
        else if (teleporting)
        {
            FlashTeleport();
        }
        else if (flashing)
        {
            FlashUpdate();
        } 
    }
}
