using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection Direction;

    [HideInInspector]
    public float StepLenght = 1.0f;

    [HideInInspector]
    public float MovementFrequency = 0.1f;

    float m_Counter;
    bool m_Move;

    [SerializeField]
    GameObject m_TailPrefab;

    List<Vector3> m_DeltaPosition;
    List<Vector3> m_DeltaRotation;

    List<Rigidbody> m_Nodes;

    Rigidbody m_MainBody;
    Rigidbody m_HeadBody;
    Transform m_Transform;

    bool m_CreateNodeAtTail;

    void Awake()
    {
        m_Transform = transform;
        m_MainBody = GetComponent<Rigidbody>();

        InitSnakeNodes();
        InitPlayer();

        m_DeltaPosition = new List<Vector3>()
        {
            new Vector3(-StepLenght, 0f), // -x .. LEFT
            new Vector3(0f, StepLenght), // y .. UP
            new Vector3(StepLenght, 0f), // x .. RIGHT
            new Vector3(0f, -StepLenght) // -y .. DOWN
        };

        m_DeltaRotation = new List<Vector3>()
        {
            new Vector3(0f, 0f, -90.0f), // LEFT
            new Vector3(0f, 0f, 180.0f), // UP
            new Vector3(0f, 0f, 90.0f), // RIGHT
            new Vector3(0f, 0f, 0f) // DOWN
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequency();
    }

    void FixedUpdate()
    {
        if ((m_Move) && (!GameplayController.GameIsOver))
        {
            m_Move = false;

            Move();
        }
    }

    void InitSnakeNodes()
    {
        m_Nodes = new List<Rigidbody>();

        m_Nodes.Add(m_Transform.GetChild(0).GetComponent<Rigidbody>());
        m_Nodes.Add(m_Transform.GetChild(1).GetComponent<Rigidbody>());
        m_Nodes.Add(m_Transform.GetChild(2).GetComponent<Rigidbody>());

        m_HeadBody = m_Nodes[0];
    }

    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        Direction = (PlayerDirection)dirRandom;
    }

    void InitPlayer()
    {
        SetDirectionRandom();

        switch (Direction)
        {
            case PlayerDirection.RIGHT:
                m_Nodes[1].position = m_Nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                m_Nodes[2].position = m_Nodes[0].position - new Vector3(Metrics.NODE * 2.0f, 0f, 0f);

                break;

            case PlayerDirection.LEFT:
                m_Nodes[1].position = m_Nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                m_Nodes[2].position = m_Nodes[0].position + new Vector3(Metrics.NODE * 2.0f, 0f, 0f);

                break;

            case PlayerDirection.UP:
                m_Nodes[1].position = m_Nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                m_Nodes[2].position = m_Nodes[0].position - new Vector3(0f, Metrics.NODE * 2.0f, 0f);

                break;

            case PlayerDirection.DOWN:
                m_Nodes[1].position = m_Nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                m_Nodes[2].position = m_Nodes[0].position + new Vector3(0f, Metrics.NODE * 2.0f, 0f);

                break;
        }
    }

    void Move()
    {
        Vector3 deltaPosition = m_DeltaPosition[(int)Direction];

        Vector3 parentPosition = m_HeadBody.position;
        Vector3 previousPosition;

        m_MainBody.position = m_MainBody.position + deltaPosition;
        m_HeadBody.position = m_HeadBody.position + deltaPosition;

        Vector3 deltaRotation = m_DeltaRotation[(int)Direction];

        Quaternion parentRotation = m_HeadBody.rotation;
        Quaternion previousRotation;

        m_MainBody.rotation = Quaternion.Euler(deltaRotation);
        m_HeadBody.rotation = Quaternion.Euler(deltaRotation);

        for (int i = 1; i < m_Nodes.Count; i++)
        {
            previousPosition = m_Nodes[i].position;
            previousRotation = m_Nodes[i].rotation;

            m_Nodes[i].position = parentPosition;
            m_Nodes[i].rotation = parentRotation;

            parentPosition = previousPosition;
            parentRotation = previousRotation;
        }

        if (m_CreateNodeAtTail)
        {
            m_CreateNodeAtTail = false;

            GameObject newNode = Instantiate(m_TailPrefab, m_Nodes[m_Nodes.Count - 1].position, m_Nodes[m_Nodes.Count - 1].rotation);

            newNode.transform.SetParent(m_Transform, true);
            m_Nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency()
    {
        m_Counter += Time.deltaTime;

        if (m_Counter >= MovementFrequency)
        {
            m_Counter = 0f;
            m_Move = true;
        }
    }

    public void SetInputDirection(PlayerDirection direction)
    {
        if (direction == PlayerDirection.UP && Direction == PlayerDirection.DOWN ||
            direction == PlayerDirection.DOWN && Direction == PlayerDirection.UP ||
            direction == PlayerDirection.RIGHT && Direction == PlayerDirection.LEFT ||
            direction == PlayerDirection.LEFT && Direction == PlayerDirection.RIGHT)
        {
            return;
        }

        Direction = direction;

        ForceMove();
    }

    void ForceMove()
    {
        m_Counter = 0;
        m_Move = false;
        Move();
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.FRUIT)
        {
            target.gameObject.SetActive(false);

            m_CreateNodeAtTail = true;

            GameplayController.Instance.IncreaseScore();

            AudioManager.Instance.PlayPickUpSound();
        }

        if (target.tag == Tags.WALL || target.tag == Tags.BOMB || target.tag == Tags.TAIL)
        {
            AudioManager.Instance.PlayDeadSound();

            GameplayController.Instance.GameOver();
        }
    }
}
