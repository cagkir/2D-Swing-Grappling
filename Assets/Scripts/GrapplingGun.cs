using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Transform firePoint; // Kancanýn baþlangýç noktasý
    public float hookSpeed = 10f; // Kancanýn hýzý
    public float playerPullForce = 20f;
    public float maxHookDistance = 10f;
    public float hookRadius = 2f; // Kancanýn etkileþim alaný yarýçapý
    public LayerMask whatIsHookable; // Kancanýn takýlacaðý nesnelerin katmaný

    private LineRenderer lineRenderer; // Kablo görselleþtirici
    private Rigidbody2D rb;
    [SerializeField] GameObject player;
    private RaycastHit2D hit; // Raycast sonucunu tutacak deðiþken
    private Vector2 targetPosition; // Kancanýn hedef aldýðý pozisyon
    public bool isHooked = false; // Kancanýn bir yere takýlýp takýlmadýðýný belirten flag

    Vector2 distance = Vector2.zero;
    public float howmuch, showd;

    public float playerSwayForce = 5f; // Sallanma kuvveti

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Kablo, kancanýn baþlangýç noktasý ve kancanýn sonunda olacak þekilde iki noktadan oluþur
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
            lineRenderer.SetPosition(0, firePoint.position); // Kablo, ateþ noktasýndan baþlar
            lineRenderer.SetPosition(1, targetPosition); // Kablo, kancanýn hedef aldýðý pozisyona kadar gider

            Vector2 direction = (targetPosition - (Vector2)firePoint.transform.position).normalized; // Kancaya doðru bir vektör belirle
            showd = Vector2.Distance(targetPosition, firePoint.transform.position);
            if (showd > howmuch)
            {
                rb.AddForce(direction * playerPullForce * Time.deltaTime, ForceMode2D.Force); // Oyuncuyu kancaya doðru çek
            }
            else
            {
                // Eðer karakter kancaya çok yakýnsa, sallanma kuvveti uygula
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
        // Fare pozisyonunu ekran koordinatlarýndan dünya koordinatlarýna dönüþtür

        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, hookRadius, whatIsHookable); // Fare pozisyonuna yakýn nesneleri bul

        if (colliders.Length > 0)
        {
            float minDistance = float.MaxValue;
            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(mousePosition, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetPosition = collider.transform.position; // En yakýn nesnenin pozisyonunu kaydet
                }
            }
            isHooked = true;
        }






        /*RaycastHit2D hit = Physics2D.Raycast(firePoint.position, mousePosition - firePoint2, maxHookDistance, whatIsHookable); // Mouse pozisyonuna doðru bir raycast yap
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
    public Transform firePoint; // Kancanýn baþlangýç noktasý
    public float hookSpeed = 10f; // Kancanýn hýzý
    public float playerPullForce = 20f;
    public LayerMask whatIsHookable; // Kancanýn takýlacaðý nesnelerin katmaný

    private LineRenderer lineRenderer; // Kablo görselleþtirici
    private Rigidbody2D rb;
    [SerializeField] GameObject player;
    private RaycastHit2D hit; // Raycast sonucunu tutacak deðiþken
    private Vector2 targetPosition; // Kancanýn hedef aldýðý pozisyon
    public bool isHooked = false; // Kancanýn bir yere takýlýp takýlmadýðýný belirten flag

    Vector2 distance = Vector2.zero;
    public float howmuch, showd;

    public float playerSwayForce = 5f; // Sallanma kuvveti

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Kablo, kancanýn baþlangýç noktasý ve kancanýn sonunda olacak þekilde iki noktadan oluþur
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
            lineRenderer.SetPosition(0, firePoint.position); // Kablo, ateþ noktasýndan baþlar
            lineRenderer.SetPosition(1, targetPosition); // Kablo, kancanýn hedef aldýðý pozisyona kadar gider

            Vector2 direction = (targetPosition - (Vector2)firePoint.transform.position).normalized; // Kancaya doðru bir vektör belirle
            showd = Vector2.Distance(targetPosition, firePoint.transform.position);
            if (showd > howmuch)
            {
                rb.AddForce(direction * playerPullForce * Time.deltaTime, ForceMode2D.Force); // Oyuncuyu kancaya doðru çek
            }
            else
            {
                // Eðer karakter kancaya çok yakýnsa, sallanma kuvveti uygula
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
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Fare pozisyonunu ekran koordinatlarýndan dünya koordinatlarýna dönüþtür
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, mousePosition - firePoint2, Mathf.Infinity, whatIsHookable); // Mouse pozisyonuna doðru bir raycast yap
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