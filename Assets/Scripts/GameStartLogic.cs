using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartLogic : MonoBehaviour
{
    public void PlayerChoseHunter()
    {
        Debug.Log($"Player chose HUNTER");
        SceneManager.LoadScene("HunterScene");
    }
    public void PlayerChoseGuardian()
    {
        Debug.Log($"Player chose GUARDIAN");
        //SceneManager.LoadScene("HunterScene");
    }
}
