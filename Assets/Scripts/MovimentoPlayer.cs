using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velPlayerHor;
    [SerializeField] private float velPlayerVer;
    private Vector2 direcaoInput;
    private Vector2 velMov;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direcaoInput.x = Input.GetAxisRaw("Horizontal");
        direcaoInput.y = Input.GetAxisRaw("Vertical");
        direcaoInput.Normalize();

        velMov.x = direcaoInput.x * velPlayerHor;
        velMov.y = direcaoInput.y * velPlayerVer;
        
        rb.linearVelocity = velMov;

    }
}
