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
        GameManager instance = FindObjectOfType<GameManager>();
        if(instance != null) Destroy(instance);
        SceneManager.LoadScene(6);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToSecondLevel()
    {
        SceneManager.LoadScene(2);
        GameManager.Instance.StartNewGame();
    }

    public void ToThirdLevel()
    {
        SceneManager.LoadScene(3);
        GameManager.Instance.StartNewGame();

    }

    public void ToFourthLevel()
    {
        SceneManager.LoadScene(4);
        GameManager.Instance.StartNewGame();

    }

    public void ToFifthLevel()
    {
        SceneManager.LoadScene(5);
        GameManager.Instance.StartNewGame();

    }
    #endregion


    #region Singleton

    #endregion
}

