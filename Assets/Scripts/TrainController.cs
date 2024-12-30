using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
        // Yeterli para varsa vagon ekle
        if (GameManager.Instance.totalCoins >= GameManager.Instance.wagonCost)
        {
            Vector3 newPosition;

            if (wagons.Count > 0)
            {
                // Son vagonun pozisyonunu al ve spacing uygula
                Transform lastWagon = wagons[wagons.Count - 1].transform;
                newPosition = lastWagon.position - lastWagon.forward * 16.5f; // Di�er vagonlar i�in spacing
            }
            else
            {
                // �lk vagon i�in k���k spacing
                newPosition = wagonsParent.position - wagonsParent.forward * 2.0f; // �lk vagon i�in spacing
            }

            // Yeni vagonu olu�tur
            GameObject newWagon = Instantiate(wagonPrefab, newPosition, wagonsParent.rotation, wagonsParent);
            wagons.Add(newWagon);

            // Vagon maliyetini d��
            GameManager.Instance.totalCoins -= GameManager.Instance.wagonCost;

            // Para miktar�n� g�ncelle
            GameManager.Instance.UpdateCoinUI();
        }
        else
        {
            Debug.Log("Yeterli paran�z yok!"); // Geri bildirim mesaj�
        }
    }

    public void UpgradeWagons()
    {
        foreach (GameObject wagonObj in wagons) // E�er wagons listesi GameObject t�r�ndeyse
        {
            Wagon wagon = wagonObj.GetComponent<Wagon>(); // GameObject'ten Wagon bile�enini al
            if (wagon != null && wagon.upgradeLevel < wagon.upgradePrefabs.Length - 1)
            {
                wagon.UpgradeWagon();
                return; // Sadece bir vagonu upgrade edip ��k
            }
        }

        Debug.Log("T�m vagonlar maksimum seviyeye ula�t�!");
    }

    public void ReplaceWagon(GameObject oldWagonObject, GameObject newWagonObject)
    {
        Wagon oldWagon = oldWagonObject.GetComponent<Wagon>();
        Wagon newWagon = newWagonObject.GetComponent<Wagon>();

        if (oldWagon != null && newWagon != null)
        {
            int index = wagons.IndexOf(oldWagonObject);

            if (index != -1)
            {
                wagons[index] = newWagonObject; // Eski GameObject'i yeni GameObject ile de�i�tir
            }
        }
    }
}
