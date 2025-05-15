using System.Security.Cryptography;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool denyNextCollision;
    public Rigidbody rb;
    public float impulseForce = 5f;
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (denyNextCollision)
        {
            return;
        }

        Death deathPart = collision.gameObject.GetComponent<Death>();
        if (deathPart)
        {
            deathPart.HitDeathPart();
        }

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce , ForceMode.Impulse);

        denyNextCollision = true;
        Invoke("allowCollision", .2f);

        //GameManager.singleton.AddScore(1);
        //Debug.Log(GameManager.singleton.score);
    }

    private void allowCollision()
    {
        denyNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }
}
