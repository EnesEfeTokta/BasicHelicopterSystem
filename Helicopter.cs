using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Helicopter : MonoBehaviour
{
    [Header("Force")]
    [SerializeField] private float engineForce = 350f;
    [SerializeField] private float altitudeChangeForce = 300f;

    [Header("Rotation Speed")]
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float anglesSpeed = 0.1f;

    [Header("Angle")]
    [SerializeField] private float maxTiltAngle = 30f;

    [Header("Propeller")]
    [SerializeField] private Transform propeller;
    [SerializeField] private Transform propellerDry;
    [SerializeField] private float propellerSpeed = 500f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Hareket kontrolleri
        float verticalInput = Input.GetAxis("Vertical"); // İleri-Geri hareket
        float horizontalInput = Input.GetAxis("Horizontal"); // Sağa-Sola hareket
        float altitudeInput = Input.GetAxis("Altitude"); // Yükseklik kontrolü
        float rotationInput = Input.GetAxis("Rotation"); // Y ekseni üzerinde dönme

        // İleri-geri vektör tanımlama
        Vector3 forwardForce = transform.forward * verticalInput * engineForce * Time.deltaTime;

        // Sağa-sola vektör tanımlama
        Vector3 sidewaysForce = transform.right * horizontalInput * engineForce * Time.deltaTime;

        // Yükseklik değişimi
        rb.AddForce(Vector3.up * altitudeInput * altitudeChangeForce * Time.deltaTime);
        
        // Hareket gerçekleştirme
        rb.AddForce(forwardForce + sidewaysForce);

        // Helikopter dönüşleri
        float WS_TiltAngle = verticalInput * maxTiltAngle;
        float AD_TiltAngle = horizontalInput * maxTiltAngle;
        float QE_TiltAngle = rotationInput * maxTiltAngle;

        // Dönüş kontrolleri
        if (AD_TiltAngle == 0)
        {
            Quaternion targetRotation = Quaternion.Euler(WS_TiltAngle, transform.eulerAngles.y, QE_TiltAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, anglesSpeed);

            // Kendi etrafında dönüşü
            transform.Rotate(-Vector3.up, QE_TiltAngle * rotateSpeed * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(WS_TiltAngle, transform.eulerAngles.y, -AD_TiltAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, anglesSpeed);
        }

        // Pervanelerin dönmesi
        propeller.transform.Rotate(Vector3.up, propellerSpeed * Time.deltaTime);
        propellerDry.transform.Rotate(Vector3.right, propellerSpeed * Time.deltaTime);
    }
}
