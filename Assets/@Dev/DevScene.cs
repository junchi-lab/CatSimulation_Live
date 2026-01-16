using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DevScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        SceneType = Define.EScene.DevScene;

        // Resource
        ResourceManager.Instance.LoadAll();
        
        // Data
        DataManager.Instance.LoadData();
        
        //Save Data Load
        SaveManager.Instance.Load();
        // 자동저장    
        // SaveManager.Instance.StartAutoSave(); 
        
        
        
        List<Vector2Int> walkableCells = MapManager.Instance.GetWalkableCells();
        for (int i = 0; i < 5; i++)
        {
            bool placed = false;
            int attempts = 0;
            while (!placed && attempts < 100 && walkableCells.Count > 0)
            {
                int randomIndex = Random.Range(0, walkableCells.Count);
                Vector2Int spawnPos = walkableCells[randomIndex];
                walkableCells.RemoveAt(randomIndex);

                Player cat = ObjectManager.Instance.SpawnPlayer("Cat");
                if (MapManager.Instance.MoveTo(cat, spawnPos, true))
                {
                    placed = true;
                }
                attempts++;
            }
        }
        
        
    }
}
