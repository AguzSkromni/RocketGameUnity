using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{

    /// <summary>
    /// This class manages collision implementation.
    /// </summary>
    [SerializeField] CollisionHandlerScript collisionHandler;

    /// <summary>
    /// This class help us to improve the movement to "up"
    /// </summary>
    [SerializeField] InputAction thrust;



    [SerializeField] InputAction rotation;


    [SerializeField] float thrustStrength = 950f;

    [SerializeField] float rotationForce = 150F;

    AudioSource audioSource;

    Rigidbody rb;

    [SerializeField] AudioClip mainEngine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void OnDisable()
    {
        thrust.Disable();
        rotation.Disable();
    }

    void FixedUpdate()
    {
        if (collisionHandler != null && collisionHandler.getGetCrashed())
        {
            // Asegurarse que los inputs estén deshabilitados y que no se apliquen fuerzas
            thrust.Disable();
            rotation.Disable();

            // Parar audio y regresar
            if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
            return;
        }

        ThurstMovement();
        RotationMovement();

        rb.constraints = RigidbodyConstraints.FreezePositionZ;


    }

    void RotationMovement()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            RotationWithForce(rotationForce);
        }
        else if (rotationInput > 0)
        {
            RotationWithForce(-rotationForce);
        }
    }

    private void RotationWithForce(float rotationForce)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationForce * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

    void ThurstMovement()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
        }
        else
        {
            audioSource.Stop();
        }
    }



}
