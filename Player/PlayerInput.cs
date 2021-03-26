using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerController m_PlayerController;

    int m_Horizontal = 0, m_Vertical = 0;

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    void Awake()
    {
        m_PlayerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Horizontal = 0;
        m_Vertical = 0;

        GetKeyboardInput();

        SetMovement();
    }

    void GetKeyboardInput()
    {
        m_Horizontal = GetAxisRaw(Axis.Horizontal);
        m_Vertical = GetAxisRaw(Axis.Vertical);

        if (m_Horizontal != 0)
        {
            m_Vertical = 0;
        }
    }

    void SetMovement()
    {
        if (m_Vertical != 0)
        {
            m_PlayerController.SetInputDirection((m_Vertical == 1) ?
                                                  PlayerDirection.UP : PlayerDirection.DOWN);


        }
        else if (m_Horizontal != 0)
        {
            m_PlayerController.SetInputDirection((m_Horizontal == 1) ?
                                                  PlayerDirection.RIGHT : PlayerDirection.LEFT);
        }
    }

    int GetAxisRaw(Axis axis)
    {
        if (axis == Axis.Horizontal)
        {
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);

            if (left)
            {
                return -1;
            }

            if (right)
            {
                return 1;
            }

            return 0;
        }
        else if (axis == Axis.Vertical)
        {
            bool up = Input.GetKeyDown(KeyCode.UpArrow);
            bool down = Input.GetKeyDown(KeyCode.DownArrow);

            if (up)
            {
                return 1;
            }

            if (down)
            {
                return -1;
            }

            return 0;
        }

        return 0;
    }
}
