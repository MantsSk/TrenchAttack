using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;

    void Start()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayerDied() 
    {
        StartCoroutine(RespawnPlayer());
    }

    public IEnumerator RespawnPlayer() 
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
