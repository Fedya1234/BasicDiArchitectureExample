using System;
using System.Collections.Generic;
using Data.Grades;
using Data.Saves;
using Managers;
using Tools;
using UnityEngine;

namespace GameSetup
{
  public class FContainer : StaticBehaviour<FContainer>
  {
    [SerializeField] private SaveData _saveData;
    [SerializeField] private GradeStaticData _gradeStaticData;

    private readonly Dictionary<Type, object> _globalContainer = new();
    private readonly Dictionary<Type, object> _sceneContainer = new();

    protected override void OnAwake()
    {
      var saveManager = new SaveManager(_saveData);
      var currencyManager = new CurrencyManager(_saveData.CurrencySaveData);
      var gradeManager = new GradeManager(_saveData.GradeSaveData, _gradeStaticData, currencyManager, saveManager);

      AddGlobal(saveManager);
      AddGlobal(currencyManager);
      AddGlobal(gradeManager);

      DontDestroyOnLoad(gameObject);
    }

    public static T Get<T>() where T : class
    {
      if (Instance._globalContainer.TryGetValue(typeof(T), out var instance))
        return (T) instance;

      if (Instance._sceneContainer.TryGetValue(typeof(T), out instance))
        return (T) instance;

      throw new Exception($"Type {typeof(T)} not registered in GameManager.");
    }

    public static void Add<T>(T instance) where T : class
    {
      var type = typeof(T);
      if (instance == null)
        throw new ArgumentNullException(nameof(instance), $"Trying to register null instance of {typeof(T)}");

      if (Instance._sceneContainer.ContainsKey(type))
        Debug.LogWarning($"Scene container already has {type}. Overwriting.");

      Instance._sceneContainer[type] = instance;
    }
    
    public static void ClearSceneContainer()
    {
      if (Instance == null)
        return;

      Instance._sceneContainer.Clear();
    }

    private void AddGlobal<T>(T instance) where T : class
    {
      _globalContainer[typeof(T)] = instance;
    }
  }
}