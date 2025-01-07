using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class TrainController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent; // NavMesh Agent referansý
    private Rigidbody rb;
    public float speed = 2f;
    public float slowSpeed = 5f;
    private bool isSlowingDown = false;
    private bool isStopped = false;
    public GameObject wagonPrefab;
    public Transform wagonsParent;
    public float wagonSpacing = 2f;
    public Text coinText; // Unity'deki bir Text nesnesine baðlayýn
    public List<GameObject> wagons = new List<GameObject>(); // Public hale getirildi
    public static TrainController Instance; // Singleton için Instance tanýmý
    private bool isOnSecondPath = false; // 2. yol kontrolü
    public Transform mainTrack; // Ana yol (1. yol) için referans
    public Transform secondTrack; // Ýkinci yol için referans
    private Transform currentTrack; // Tren þu anda hangi yolda
    public float tiltAngle = 15f; // Yokuþlarda eðilme açýsý
    private float smoothTiltSpeed = 2f;


    void Start()
    {
        // Baþlangýçta ana yol (1. yol) olacak
        currentTrack = mainTrack;
    }


    private void Awake()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // Singleton kontrolü
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Zaten bir Instance varsa, fazladan oluþturulaný yok et
        }
    }

    void Update()
    {
        if (isStopped) return;

        // Tren hareket etmeye devam etsin
        float currentSpeed = speed;

        // Tren hangi yolda ilerliyor, ona göre hareket etsin
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (navMeshAgent != null && rb != null)
        {
            // NavMeshAgent hýzýný Rigidbody'ye aktar
            rb.velocity = navMeshAgent.velocity;

            // Eðimi algýla ve trenin açýsýný düzenle
            AdjustTiltOnSlope();
            AdjustSpeedOnSlope();
            AdjustAgentBasedOnAreaType();
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        isSlowingDown = false;
    }

    public void SlowDown()
    {
        isSlowingDown = true;
    }

    public void Stop()
    {
        isStopped = true;
    }

    public void Resume()
    {
        isStopped = false;
        isSlowingDown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "SlowTrigger1":
                SetSpeed(20f);
                break;
            case "SlowTrigger2":
                SetSpeed(13f);
                break;
            case "SlowTrigger3":
                SetSpeed(8f);
                break;
            case "SlowTrigger4":
                SetSpeed(4f);
                break;
            case "SlowTrigger5":
                SetSpeed(2f);
                break;
            case "StartTrigger":
                SetSpeed(4f);
                break;
            case "FastTrigger1":
                SetSpeed(10f);
                break;
            case "FastTrigger2":
                SetSpeed(18f);
                break;
            case "FastTrigger3":
                SetSpeed(25f);
                break;
            case "StopTrigger":
                Stop();
                GameManager.Instance.OpenUpgradePanel();
                break;
            default:
                break;
        }
    }
    public void AddWagon()
    {
        Vector3 newPosition;
        Quaternion newRotation;

        if (wagons.Count > 0)
        {
            GameObject lastWagon = wagons[wagons.Count - 1];
            newPosition = lastWagon.transform.position - lastWagon.transform.forward * 16.5f;
            newRotation = lastWagon.transform.rotation;
        }
        else
        {
            newPosition = wagonsParent.position - wagonsParent.forward * 2.0f;
            newRotation = wagonsParent.rotation;
        }

        GameObject newWagon = Instantiate(wagonPrefab, newPosition, newRotation, wagonsParent);
        wagons.Add(newWagon);

        Debug.Log("Vagon eklendi!");
    }


    public void UpgradeWagons()
    {
        foreach (GameObject wagonObject in wagons) // Tüm vagon GameObject'lerini dolaþ
        {
            Wagon wagon = wagonObject.GetComponent<Wagon>(); // GameObject'ten Wagon bileþenini al
            if (wagon != null && wagon.upgradeLevel < wagon.upgradePrefabs.Length - 1)
            {
                wagon.UpgradeWagon();
                return; // Sadece bir vagonu upgrade edip çýk
            }
        }

        Debug.Log("Tüm vagonlar maksimum seviyeye ulaþtý!");
    }

    public void ReplaceWagon(GameObject oldWagon, GameObject newWagon)
    {
        int index = wagons.IndexOf(oldWagon); // Vagon listesinden eski vagonu bul
        if (index != -1)
        {
            wagons[index] = newWagon; // Yeni vagonu listeye ekle
        }
        else
        {
            Debug.LogWarning("Deðiþtirilecek vagon bulunamadý.");
        }

        // Eski vagonu sahneden kaldýr
        Destroy(oldWagon);
    }

    // 2. yola gitme fonksiyonu
    public void GoToSecondPath()
    {
        if (navMeshAgent != null)
        {
            // Ýkinci yolun hedef pozisyonunu ayarlayýn (örneðin bir boþ GameObject ile)
            Vector3 secondPathTarget = secondTrack.position;
            navMeshAgent.SetDestination(secondPathTarget);
        }
    }

    // Düz gitme fonksiyonu
    public void ContinueStraight()
    {
        if (navMeshAgent != null)
        {
            // Ana yol hedef pozisyonunu ayarlayýn
            Vector3 mainPathTarget = mainTrack.position;
            navMeshAgent.SetDestination(mainPathTarget);
        }
    }

    public void StopTrain()
    {
        isStopped = true;
        Debug.Log("Tren durdu.");
    }

    // Treni devam ettirme
    public void ResumeTrain()
    {
        isStopped = false;
        Debug.Log("Tren devam ediyor.");
    }

    private void AdjustTiltOnSlope()
    {
        // Zemin normali algýla
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            // Zemin normaline göre açýyý hesapla
            Vector3 slopeNormal = hit.normal;
            Vector3 forward = transform.forward;

            // Eðimi al ve sýnýrlý bir açýyla trenin dönmesini saðla
            Vector3 cross = Vector3.Cross(Vector3.up, slopeNormal);
            float slopeAngle = Vector3.SignedAngle(Vector3.up, slopeNormal, cross);

            // Tren rotasyonunu kademeli olarak eðime göre ayarla
            Quaternion targetRotation = Quaternion.Euler(slopeAngle * tiltAngle, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTiltSpeed * Time.deltaTime);
        }
    }

    private void AdjustSpeedOnSlope()
    {
        // Yokuþ eðimini hesapla
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            Vector3 slopeNormal = hit.normal;
            float slopeAngle = Vector3.Angle(Vector3.up, slopeNormal);

            // Eðer eðim belli bir açýdan büyükse, fiziksel direnç uygula
            if (slopeAngle > 5f)
            {
                rb.velocity = rb.velocity * Mathf.Clamp01(1f - (slopeAngle / 45f)); // 45 derece eðim maksimum direnç
            }
        }
    }

    private void AdjustAgentBasedOnAreaType()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
        {
            if (hit.mask == NavMesh.GetAreaFromName("Slope"))
            {
                // Eðime uygun hareket (örneðin, yavaþlama)
                rb.velocity = rb.velocity * 0.8f; // Hafif yavaþlama
            }
            else if (hit.mask == NavMesh.GetAreaFromName("Flat"))
            {
                // Düz zeminde normal hýz
                rb.velocity = navMeshAgent.velocity;
            }
        }
    }
}