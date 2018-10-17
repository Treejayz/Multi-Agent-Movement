using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {

	
    public void onClick()
    {
        SceneManager.LoadScene(0);
    }
}
