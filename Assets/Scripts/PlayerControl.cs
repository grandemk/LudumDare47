using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    float timerValue;

    public void Restart(float timeToWait)
    {
        timerValue = timeToWait;
    }

    public void Update(float deltaTime)
    {
        timerValue -= deltaTime;
    }

    public bool Ended()
    {
        return timerValue <= 0;
    }
}

public class PlayerControl : MonoBehaviour
{
    Timer timer = new Timer();
    [SerializeField]
    float inputSeparationTime = 0.15f;
    float gridSize = 1f;

    float oldHorizontalInput = 0f;
    float oldVerticalInput = 0f;


    void Update()
    {
        timer.Update(Time.deltaTime);
        if (timer.Ended())
            Move();
    }

    float ComputeMove(float input)
    {
        if (Mathf.Abs(input) > 0.0f)
            return Mathf.Sign(input) * gridSize;
        else
            return 0f;
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0f && verticalInput == 0f)
            return;

        if (horizontalInput != 0f && verticalInput != 0f)
            verticalInput = 0f;

        Vector3 direction = new Vector3(ComputeMove(horizontalInput), ComputeMove(verticalInput), 0);
        transform.Translate(direction);
        timer.Restart(inputSeparationTime);

        oldHorizontalInput = horizontalInput;
        oldVerticalInput = verticalInput;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");
    }
}
