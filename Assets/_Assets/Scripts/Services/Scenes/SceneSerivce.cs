using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class SceneSerivce : IInitializable
{
    private Dictionary<SceneType, string> _scenes;
    private SceneType _currentScene;
    private List<SceneType> _loadedScenes;
    
    public event Action<SceneType> OnSceneLoaded;
    public event Action<SceneType> OnSceneUnloaded;
    
    public SceneType CurrentScene => _currentScene;
    
    public void Initialize()
    {
        _scenes = new Dictionary<SceneType, string>
        {
            {SceneType.Boot, "Boot"},
            {SceneType.Main, "Main"}
        };
        
        _loadedScenes = new List<SceneType>();
        _currentScene = SceneType.Boot;
    }
    
    public void LoadScene(SceneType sceneType, LoadSceneMode loadingMode)
    {
        if (_loadedScenes.Contains(sceneType))
        {
            Debug.LogWarning($"Scene {sceneType} already loaded");
            return;
        }
        
        _loadedScenes.Add(sceneType);
        _currentScene = sceneType;
        SceneManager.LoadScene(_scenes[sceneType], loadingMode);
    }
    
    public async UniTask LoadSceneAsync(SceneType sceneType, LoadSceneMode loadingMode)
    {
        if (_loadedScenes.Contains(sceneType))
        {
            Debug.LogWarning($"Scene {sceneType} already loaded");
            return;
        }
        
        _loadedScenes.Add(sceneType);
        _currentScene = sceneType;
        await SceneManager.LoadSceneAsync(_scenes[sceneType], loadingMode);
        OnSceneLoaded?.Invoke(sceneType);
    }
    
    public void UnloadScene(SceneType sceneType)
    {
        SceneManager.UnloadScene(_scenes[sceneType]);
        _loadedScenes.Remove(sceneType);
        OnSceneUnloaded?.Invoke(sceneType);
    }
    
    public async UniTask UnloadSceneAsync(SceneType sceneType)
    {
        await SceneManager.UnloadSceneAsync(_scenes[sceneType]);
        _loadedScenes.Remove(sceneType);
        OnSceneUnloaded?.Invoke(sceneType);
    }
}
