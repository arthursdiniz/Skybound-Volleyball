using UnityEngine;

public class BallDebug : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField] private float ballForce = 20f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rb.AddForce(Vector3.up * ballForce,ForceMode.Impulse);
        }
    }
}
