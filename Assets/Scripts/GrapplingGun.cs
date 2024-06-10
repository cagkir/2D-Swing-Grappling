using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Transform firePoint; // Kancan�n ba�lang�� noktas�
    public float hookSpeed = 10f; // Kancan�n h�z�
    public float playerPullForce = 20f;
    public float maxHookDistance = 10f;
    public float hookRadius = 2f; // Kancan�n etkile�im alan� yar��ap�
    public LayerMask whatIsHookable; // Kancan�n tak�laca�� nesnelerin katman�

    private LineRenderer lineRenderer; // Kablo g�rselle�tirici
    private Rigidbody2D rb;
    [SerializeField] GameObject player;
    private RaycastHit2D hit; // Raycast sonucunu tutacak de�i�ken
    private Vector2 targetPosition; // Kancan�n hedef ald��� pozisyon
    public bool isHooked = false; // Kancan�n bir yere tak�l�p tak�lmad���n� belirten flag

    Vector2 distance = Vector2.zero;
    public float howmuch, showd;

    public float playerSwayForce = 5f; // Sallanma kuvveti

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Kablo, kancan�n ba�lang�� noktas� ve kancan�n sonunda olacak �ekilde iki noktadan olu�ur
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isHooked)
        {
            FireHook();
            distance = (targetPosition - (Vector2)firePoint.transform.position).normalized;
        }
        if (Input.GetButtonUp("Fire1") && isHooked)
        {
            RetractHook();
        }

        if (isHooked)
        {
            lineRenderer.SetPosition(0, firePoint.position); // Kablo, ate� noktas�ndan ba�lar
            lineRenderer.SetPosition(1, targetPosition); // Kablo, kancan�n hedef ald��� pozisyona kadar gider

            Vector2 direction = (targetPosition - (Vector2)firePoint.transform.position).normalized; // Kancaya do�ru bir vekt�r belirle
            showd = Vector2.Distance(targetPosition, firePoint.transform.position);
            if (showd > howmuch)
            {
                rb.AddForce(direction * playerPullForce * Time.deltaTime, ForceMode2D.Force); // Oyuncuyu kancaya do�ru �ek
            }
            else
            {
                // E�er karakter kancaya �ok yak�nsa, sallanma kuvveti uygula
                rb.AddForce(new Vector2(Mathf.Sin(Time.time * 10f), Mathf.Cos(Time.time * 10f)) * playerSwayForce * Time.deltaTime, ForceMode2D.Force);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }

    void FireHook()
    {
        Vector2 firePoint2 = new Vector2(firePoint.position.x,firePoint.position.y);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Fare pozisyonunu ekran koordinatlar�ndan d�nya koordinatlar�na d�n��t�r

        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, hookRadius, whatIsHookable); // Fare pozisyonuna yak�n nesneleri bul

        if (colliders.Length > 0)
        {
            float minDistance = float.MaxValue;
            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(mousePosition, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetPosition = collider.transform.position; // En yak�n nesnenin pozisyonunu kaydet
                }
            }
            isHooked = true;
        }






        /*RaycastHit2D hit = Physics2D.Raycast(firePoint.position, mousePosition - firePoint2, maxHookDistance, whatIsHookable); // Mouse pozisyonuna do�ru bir raycast yap
        if (hit.collider != null)
        {
            isHooked = true;
            targetPosition = hit.point;
        }*/
    }

    void RetractHook()
    {
        isHooked = false;
    }
}


/*

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Transform firePoint; // Kancan�n ba�lang�� noktas�
    public float hookSpeed = 10f; // Kancan�n h�z�
    public float playerPullForce = 20f;
    public LayerMask whatIsHookable; // Kancan�n tak�laca�� nesnelerin katman�

    private LineRenderer lineRenderer; // Kablo g�rselle�tirici
    private Rigidbody2D rb;
    [SerializeField] GameObject player;
    private RaycastHit2D hit; // Raycast sonucunu tutacak de�i�ken
    private Vector2 targetPosition; // Kancan�n hedef ald��� pozisyon
    public bool isHooked = false; // Kancan�n bir yere tak�l�p tak�lmad���n� belirten flag

    Vector2 distance = Vector2.zero;
    public float howmuch, showd;

    public float playerSwayForce = 5f; // Sallanma kuvveti

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Kablo, kancan�n ba�lang�� noktas� ve kancan�n sonunda olacak �ekilde iki noktadan olu�ur
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isHooked)
        {
            FireHook();
            distance = (targetPosition - (Vector2)firePoint.transform.position).normalized;
        }
        if (Input.GetButtonUp("Fire1") && isHooked)
        {
            RetractHook();
        }

        if (isHooked)
        {
            lineRenderer.SetPosition(0, firePoint.position); // Kablo, ate� noktas�ndan ba�lar
            lineRenderer.SetPosition(1, targetPosition); // Kablo, kancan�n hedef ald��� pozisyona kadar gider

            Vector2 direction = (targetPosition - (Vector2)firePoint.transform.position).normalized; // Kancaya do�ru bir vekt�r belirle
            showd = Vector2.Distance(targetPosition, firePoint.transform.position);
            if (showd > howmuch)
            {
                rb.AddForce(direction * playerPullForce * Time.deltaTime, ForceMode2D.Force); // Oyuncuyu kancaya do�ru �ek
            }
            else
            {
                // E�er karakter kancaya �ok yak�nsa, sallanma kuvveti uygula
                rb.AddForce(new Vector2(Mathf.Sin(Time.time * 10f), Mathf.Cos(Time.time * 10f)) * playerSwayForce * Time.deltaTime, ForceMode2D.Force);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }

    void FireHook()
    {
        Vector2 firePoint2 = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Fare pozisyonunu ekran koordinatlar�ndan d�nya koordinatlar�na d�n��t�r
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, mousePosition - firePoint2, Mathf.Infinity, whatIsHookable); // Mouse pozisyonuna do�ru bir raycast yap
        if (hit.collider != null)
        {
            isHooked = true;
            targetPosition = hit.point;
        }
    }

    void RetractHook()
    {
        isHooked = false;
    }
}

*/