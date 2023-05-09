using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] cameraController cameraController;
    bool isBallOutside;
    Vector3 currentBallPos;
    private void Update()
    {
        if (ballController.IsMove() == false && isBallOutside == false)
        {
            currentBallPos = ballController.transform.position;
        }

        bool inputActive = Input.GetMouseButton(0)
            && ballController.IsMove() == false
            && ballController.ShootMode == false
            && isBallOutside == false;

        cameraController.SetInputActive(inputActive);
    }

    public void OnBallOutside()
    {
        if (isBallOutside == false)
            Invoke("ResetPosition", 1);

        ballController.enabled = false;
        isBallOutside = true;
    }

    public void ResetPosition()
    {
        var rb = ballController.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballController.transform.position = currentBallPos;
        rb.isKinematic = false;
        ballController.enabled = true;
        isBallOutside = false;
    }
}
