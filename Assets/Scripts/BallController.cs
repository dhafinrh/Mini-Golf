using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject mark;
    [SerializeField] Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] Transform aimWorld;
    // [SerializeField] LineRenderer aimLine;
    float forceFactor;
    bool shoot;
    bool shootMode;
    Vector3 forceDirection;
    Ray ray;
    Plane plane;

    public bool ShootMode { get => shootMode; }

    private void Update()
    {
        if (shootMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                aimWorld.gameObject.SetActive(true);
                plane = new Plane(Vector3.up, this.transform.position);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector3 ballViewportPos = Camera.main.WorldToViewportPoint(this.transform.position);
                Vector3 ballScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                Vector3 pointerDirection = ballViewportPos - mouseViewportPos;
                pointerDirection.z = 0;

                //Force Direction
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                plane.Raycast(ray, out var distance);
                forceDirection = this.transform.position - ray.GetPoint(distance);
                forceDirection.Normalize();

                //Force Factor
                forceFactor = pointerDirection.magnitude * 2;

                //Aim Visual
                aimWorld.transform.position = this.transform.position;
                aimWorld.forward = forceDirection;
                aimWorld.localScale = new Vector3(1, 1, 0.5f + forceFactor);

                //Draw Aim
                // Vector3[] positions = new Vector3[] { ballScreenPos, Input.mousePosition };
                // aimLine.SetPositions(positions);
                // var aimDirection = new Vector3(pointerDirection.x, 0, pointerDirection.y);
                // aimDirection = Camera.main.transform.worldToLocalMatrix * aimDirection;
                // aimDirection.y = 0;
                // aimWorld.transform.forward = aimDirection.normalized;W
            }
            else if (Input.GetMouseButtonUp(0))
            {
                shoot = true;
                shootMode = false;
                aimWorld.gameObject.SetActive(false);
            }
        }

        // if (Input.GetMouseButton(0))
        // {
        //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //     if (Physics.Raycast(ray, out var hitInfo) && hitInfo.collider == col)
        //     {
        //         Debug.Log(hitInfo.collider.name);
        //         shoot = true;
        //     }
        // }

    }

    private void FixedUpdate()
    {
        if (shoot)
        {
            mark.SetActive(false);
            shoot = false;
            rb.AddForce(forceDirection * force * forceFactor, ForceMode.Impulse);
        }

        if (rb.velocity.sqrMagnitude < 0.075f && rb.velocity.sqrMagnitude > 0)
        {
            rb.velocity = Vector3.zero;
            mark.SetActive(true);
        }
    }

    public bool IsMove()
    {
        return rb.velocity != Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsMove())
        {
            shootMode = true;
        }
    }
}
