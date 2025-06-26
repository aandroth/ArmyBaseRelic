using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController: MonoBehaviour
{
    public static GameController m_instance = null;
    public Camera m_camera = null;
    public GameObject m_player;
    public List<GameObject> m_rooms = new List<GameObject>();
    public List<MoveToRoom> m_transferPoints = new List<MoveToRoom>();

    private void Awake()
    {
        if (m_instance = null)
        {
            m_instance = this;
            m_camera = Camera.main;
        }
        else if (m_instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);
        foreach (GameObject room in m_rooms)
        {
            Debug.Log($"Room: {room.name}");
            room.GetComponent<SetWorldBounds>().m_newWorldBoundsSet = SetCameraWithNewBounds;
            if (room.GetComponent<SetWorldBounds>().m_isStartingRoom)
            {
                room.GetComponent<SetWorldBounds>().SetWorldBoundsToThisSprite();
            }
        }

        foreach (MoveToRoom transferPoint in m_transferPoints)
        {
            Debug.Log($"Transfer Point: {transferPoint.gameObject.name}");
            transferPoint.GetComponent<MoveToRoom>().m_gameControllerMoveFunction = MovePlayerToPosition;
        }
    }

    public void MovePlayerToPosition(Transform newPosition)
    {
        m_player.transform.position = newPosition.position;
    }

    public void SetCameraWithNewBounds()
    {
        m_camera.GetComponent<Camera_Behavior>().SetMinAndMaxToWorldBounds();
    }
}
