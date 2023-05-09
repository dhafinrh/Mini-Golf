using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] cameraController cameraController;
    [SerializeField] GameObject finishWindow;
    [SerializeField] TMP_Text finishText;
    [SerializeField] TMP_Text shootCountText;
    bool isBallOutside;
    bool isGoal;
    bool isBallTeleporting;
    Vector3 currentBallPos;

    private void OnEnable()
    {
        ballController.OnBallShoot.AddListener(UpdateShootCount);
    }

    private void OnDisable()
    {
        ballController.OnBallShoot.RemoveListener(UpdateShootCount);
    }
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

    public void onBallGoalEnter()
    {
        isGoal = true;
        ballController.enabled = false;

        finishWindow.SetActive(true);
        finishText.text = "GOAAL\n Shoot Count : " + ballController.ShootCount;
    }

    public void OnBallOutside()
    {
        if (isGoal)
            return;

        if (!isBallTeleporting)
            if (isBallOutside == false)
            {
                isBallTeleporting = true;
                Invoke("TeleportBallLastPosition", 1);
            }

        ballController.enabled = false;
        isBallOutside = true;
    }
    public void TeleportBallLastPosition()
    {
        TeleportBall(currentBallPos);
    }

    public void TeleportBall(Vector3 value)
    {
        var rb = ballController.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballController.transform.position = value;
        rb.isKinematic = false;

        ballController.enabled = true;
        isBallOutside = false;
        isBallTeleporting = false;
    }

    public void UpdateShootCount(int value)
    {
        shootCountText.text = value.ToString();
    }
}
