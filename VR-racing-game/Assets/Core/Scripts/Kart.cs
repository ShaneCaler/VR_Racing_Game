namespace VRTK.Examples
{
    using UnityEngine;

    public class Kart : MonoBehaviour
    {
        public float maxAcceleration = 3f;
        public float jumpPower = 10f;

        private float acceleration = 0.05f;
        private float movementSpeed = 0f;
        private float rotationSpeed = 180f;
        private bool isJumping = false;
        private Vector2 touchAxis;
        private float triggerAxis;
        private Rigidbody rb;
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;

        public float speed = 6.0f;
        public float gravity = 20.0f;

        private Vector3 moveDirection = Vector3.zero;
        private CharacterController controller;

        void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        public void SetTouchAxis(Vector2 data)
        {
            touchAxis = data;
        }

        public void SetTriggerAxis(float data)
        {
            triggerAxis = data;
        }

        public void ResetCar()
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            defaultPosition = transform.position;
            defaultRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            if (isJumping)
            {
                touchAxis.x = 0f;
            }
            CalculateSpeed();
            Move();
            //Turn();
            //Jump();
        }

        private void CalculateSpeed()
        {
            Debug.Log("Calculating Speed...");
            if (touchAxis.y != 0f)
            {
                movementSpeed += (acceleration * touchAxis.y);
                movementSpeed = Mathf.Clamp(movementSpeed, -maxAcceleration, maxAcceleration);
            }
            else
            {
                Decelerate();
            }
        }

        private void Decelerate()
        {
            Debug.Log("Decelerating w/ a movement speed of " + movementSpeed);
            if (movementSpeed > 0)
            {
                movementSpeed -= Mathf.Lerp(acceleration, maxAcceleration, 0f);
            }
            else if (movementSpeed < 0)
            {
                movementSpeed += Mathf.Lerp(acceleration, -maxAcceleration, 0f);
            }
            else
            {
                movementSpeed = 0;
            }
        }

        private void Move()
        {
            Debug.Log("moving at movement speed of " + movementSpeed);
            //Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
            //rb.MovePosition(rb.position + movement);
            moveDirection = new Vector3(touchAxis.x, 0.0f, touchAxis.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * movementSpeed;

            // Apply gravity
            moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

            // Move the controller
            controller.Move(moveDirection * Time.deltaTime);
        }

        private void Turn()
        {
            Debug.Log("Turning at rotation speed of " + rotationSpeed);
            float turn = touchAxis.x * rotationSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        private void Jump()
        {
            Debug.Log("Jumping...");
            if (!isJumping && triggerAxis > 0)
            {
                float jumpHeight = (triggerAxis * jumpPower);
                rb.AddRelativeForce(Vector3.up * jumpHeight);
                triggerAxis = 0f;
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            isJumping = false;
        }

        private void OnTriggerExit(Collider collider)
        {
            isJumping = true;
        }
    }
}