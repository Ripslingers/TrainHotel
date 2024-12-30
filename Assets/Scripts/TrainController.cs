using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrainController : MonoBehaviour
{
    public float speed = 25f;
    public float slowSpeed = 5f;
    private bool isSlowingDown = false;
    private bool isStopped = false;
    public GameObject wagonPrefab;
    public Transform wagonsParent;
    public float wagonSpacing = 2f;
    public Text coinText; // Unity'deki bir Text nesnesine ba�lay�n
    private List<GameObject> wagons = new List<GameObject>();
    public static TrainController Instance; // Singleton i�in Instance tan�m�

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
        if (isStopped)
        {
            return;
        }

        float currentSpeed = isSlowingDown ? slowSpeed : speed;
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
        int wagonCost = 50; // Bir vagonun maliyeti (�rne�in 50 coin)

        // Yeterli para kontrol�
        if (GameManager.Instance.totalCoins >= wagonCost)
        {
            // Para d��
            GameManager.Instance.totalCoins -= wagonCost;

            // Yeni vagon olu�turma i�lemleri
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

            // Para UI'sini g�ncelle
            GameManager.Instance.UpdateCoinUI();
        }
        else
        {
            Debug.Log("Yeterli paran�z yok!");
        }
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
            Destroy(oldWagon); // Eski vagonu yok et
        }
        else
        {
            Debug.LogWarning("De�i�tirilecek vagon bulunamad�.");
        }
    }
}