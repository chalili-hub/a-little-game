using UnityEngine;
using UnityEngine.SceneManagement;

public class AIDragonBoat : MonoBehaviour
{
    public float stopXPosition = 120f;
    public float rightSpeed = 5f;
    public float slowdownDuration = 1f;

    private float originalRightSpeed;
    private bool isSlowingDown = false;
    private float slowdownTimer = 0f;
    private bool isStopped = false;

    private void Start()
    {
        originalRightSpeed = rightSpeed;
    }

    private void Update()
    {
        if (!isStopped && transform.position.x >= stopXPosition)
        {
            isStopped = true;
            rightSpeed = 0f;
            SceneManager.LoadScene("LostScene");
            return;
        }

        if (isSlowingDown)
        {
            slowdownTimer += Time.deltaTime;
            if (slowdownTimer >= slowdownDuration)
            {
                rightSpeed = originalRightSpeed;
                isSlowingDown = false;
                slowdownTimer = 0f;
                Debug.Log("结束减速");
            }
        }

        if (!isStopped)
        {
            transform.Translate(Vector3.right * rightSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Duck") || other.CompareTag("Lotus")) && !isSlowingDown && !isStopped)
        {
            rightSpeed = originalRightSpeed * 0.5f;
            isSlowingDown = true;
            Debug.Log("开始减速");
        }
    }
}