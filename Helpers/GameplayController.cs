using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance;

    public static bool GameIsOver = false;

    public GameObject FruitPickup, BombPickup;

    public GameObject YouLose;

    public float DeadSoundDuration = 2.0f;

    float m_MinX = -12.0f, m_MaxX = 12.0f, m_MinY = -6.0f, m_MaxY = 6.0f;
    float m_PositionZ = 0.5f;

    Text m_ScoreText;
    int m_ScoreCount;

    // Start is called before the first frame update
    void Start()
    {
        m_ScoreText = GameObject.Find("Score").GetComponent<Text>();

        Invoke("StartSpawning", 0.5f);
    }

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnPickUps());
    }

    public void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
    }

    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(FruitPickup, new Vector3(Random.Range(m_MinX, m_MaxX),
                                                 Random.Range(m_MinY, m_MaxY), m_PositionZ),
                                                 Quaternion.identity);
        }
        else
        {
            Instantiate(BombPickup, new Vector3(Random.Range(m_MinX, m_MaxX),
                                                Random.Range(m_MinY, m_MaxY), m_PositionZ),
                                                Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);
    }

    public void IncreaseScore()
    {
        m_ScoreCount++;
        m_ScoreText.text = "Score: " + m_ScoreCount;
    }

    public void GameOver()
    {
        YouLose.SetActive(true);

        Invoke("LoadMenu", DeadSoundDuration);

        GameIsOver = true;
    }

    void LoadMenu()
    {
        Debug.Log("Loading menu...");
        UnitySceneManager.LoadScene("Menu");
    }
}