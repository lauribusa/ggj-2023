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
    #endregion


    #region Singleton

    #endregion
}
