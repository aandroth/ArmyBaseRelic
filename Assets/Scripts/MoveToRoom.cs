using System;
using System.Collections;
using UnityEngine;

public class MoveToRoom : MonoBehaviour
{
    public float m_moveToRoomDelayTime = 3.5f;
    public float m_moveToRoomDelayTimePassed = 0f;
    public IEnumerator m_moveToRoomEnumerator = null;

    public GameObject m_roomToMoveTo = null;
    public GameObject m_parentRoom = null;
    public Transform m_newPositionInRoom = null;
    public delegate void GameControllerMoveFunction(Transform t);
    public GameControllerMoveFunction m_gameControllerMoveFunction = null;
    public delegate void GameControllerMovePrep();
    public GameControllerMovePrep m_gameControllerMovePrep = null;

    public bool m_isReadyToMovePlayerOnTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (m_isReadyToMovePlayerOnTrigger)
                RoomMoveStart();
            else
                m_isReadyToMovePlayerOnTrigger = true;
        }
    }

    public void RoomMoveStart()
    {
        if(m_roomToMoveTo == null)
        {
            Debug.LogError("Room to move to has not been set!");
            gameObject.SetActive(false);
        }
        if (m_newPositionInRoom == null)
        {
            Debug.LogError("Position to move to has not been set!");
            gameObject.SetActive(false);
        }
        if (m_gameControllerMoveFunction == null)
        {
            Debug.LogError("m_gameControllerMoveFunction has not been set!");
            gameObject.SetActive(false);
        }

        m_moveToRoomEnumerator = RoomMoveCoroutine();
        StartCoroutine(m_moveToRoomEnumerator);
        m_isReadyToMovePlayerOnTrigger = false;
    }
    private IEnumerator RoomMoveCoroutine()
    {
        m_gameControllerMovePrep.Invoke();
        while (m_moveToRoomDelayTimePassed < m_moveToRoomDelayTime)
        {
            m_moveToRoomDelayTimePassed += Time.deltaTime;
            yield return null;
        }
        m_moveToRoomDelayTimePassed = 0f;
        m_roomToMoveTo.GetComponent<SetWorldBounds>().SetWorldBoundsToThisSprite();
        m_gameControllerMoveFunction.Invoke(m_newPositionInRoom);
    }
}
