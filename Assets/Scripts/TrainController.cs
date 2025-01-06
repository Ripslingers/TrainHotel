using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrainController : MonoBehaviour
{
    public float speed = 2f;
    public float slowSpeed = 5f;
    private bool isSlowingDown = false;
    private bool isStopped = false;
    public GameObject wagonPrefab;
    public Transform wagonsParent;
    public float wagonSpacing = 2f;
    public Text coinText; // Unity'deki bir Text nesnesine ba�lay�n
    public List<GameObject> wagons = new List<GameObject>(); // Public hale getirildi
    public static TrainController Instance; // Singleton i�in Instance tan�m�
    private bool isOnSecondPath = false; // 2. yol kontrol�
    public Transform mainTrack; // Ana yol (1. yol) i�in referans
    public Transform secondTrack; // �kinci yol i�in referans
    private Transform currentTrack; // Tren �u anda hangi yolda


    void Start()
    {
        // Ba�lang��ta ana yol (1. yol) olacak
        currentTrack = mainTrack;
    }


    private void Awake()
    {
        // Singleton kontrol�
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Zaten bir Instance varsa, fazladan olu�turulan� yok et
        }
    }

    void Update()
    {
        if (isStopped) return;

        // Tren hareket etmeye devam etsin
        float currentSpeed = speed;

        // Tren hangi yolda ilerliyor, ona g�re hareket etsin
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
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
        foreach (GameObject wagonObject in wagons) // T�m vagon GameObject'lerini dola�
        {
            Wagon wagon = wagonObject.GetComponent<Wagon>(); // GameObject'ten Wagon bile�enini al
            if (wagon != null && wagon.upgradeLevel < wagon.upgradePrefabs.Length - 1)
            {
                wagon.UpgradeWagon();
                return; // Sadece bir vagonu upgrade edip ��k
            }
        }

        Debug.Log("T�m vagonlar maksimum seviyeye ula�t�!");
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
            Debug.LogWarning("De�i�tirilecek vagon bulunamad�.");
        }

        // Eski vagonu sahneden kald�r
        Destroy(oldWagon);
    }

    // 2. yola gitme fonksiyonu
    public void GoToSecondPath()
    {
        isOnSecondPath = true;
        currentTrack = secondTrack; // Tren ikinci yola y�nlendirilir
        Debug.Log("Tren 2. yola gitti.");
    }

    // D�z gitme fonksiyonu
    public void ContinueStraight()
    {
        isOnSecondPath = false;
        currentTrack = mainTrack; // Tren ana yolda devam eder
        Debug.Log("Tren d�z yolda devam ediyor.");
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
}