using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class LoadManager
{
    /// <summary>
    /// Loads the scene according to the given scene name
    /// </summary>
    /// <param name="sceneName"></param>
    public static async Task LoadScene(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName);
            
        var loadedScene = SceneManager.GetSceneByName(sceneName);
            
        SceneManager.SetActiveScene(loadedScene);
    }
}