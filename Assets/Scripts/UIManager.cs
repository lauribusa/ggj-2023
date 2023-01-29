using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	#region Exposed
	
	#endregion

	
	#region Private And Protected
   	
	#endregion

	
	#region Unity API
	
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    
    #endregion
    

    #region Main
    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ToGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToSecondLevel()
    {
        Debug.Log($"Add level index");
    }
    #endregion


    #region Singleton

    #endregion
}

