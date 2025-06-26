using UnityEngine;

public class SetWorldBounds : MonoBehaviour
{
    public bool m_isStartingRoom = false;
    public delegate void NewWorldBoundsSet();
    public NewWorldBoundsSet m_newWorldBoundsSet;

    //private void OnEnable()
    //{
    //    if (m_isStartingRoom && )
    //    {
    //        var bounds = GetComponent<SpriteRenderer>().bounds;
    //        Globals.m_worldBounds = bounds;
    //        m_newWorldBoundsSet();
    //        Debug.Log($"minX update: {bounds.min.x}");
    //        Debug.Log($"maxX update: {bounds.max.x}");
    //        Debug.Log($"minY update: {bounds.min.y}");
    //        Debug.Log($"maxY update: {bounds.max.y}");
    //    }
    //}

    public void SetWorldBoundsToThisSprite()
    {
        var bounds = GetComponent<SpriteRenderer>().bounds;
        Globals.m_worldBounds = bounds;
        m_newWorldBoundsSet();
        Debug.Log($"minX update: {bounds.min.x}");
        Debug.Log($"maxX update: {bounds.max.x}");
        Debug.Log($"minY update: {bounds.min.y}");
        Debug.Log($"maxY update: {bounds.max.y}");
    }
}
