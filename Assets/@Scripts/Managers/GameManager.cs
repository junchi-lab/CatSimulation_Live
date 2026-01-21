using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<int> HiredStaffIDs = new List<int>(); // 고용된 직원 ID 목록
    public List<int> AddedFoodIDs = new List<int>(); // 추가된 메뉴 ID 목록/**/
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameData _gameData = new GameData();

    private void Awake()
    {
        LocalizationManager.Instance.CurrentLanguage = Define.ELanguage.KOR;
    }
    public GameData GameData
    {
        get { return _gameData; }
        set
        {
            _gameData = value;
        }
    }
    
    /// <summary>
    /// 직원 고용
    /// </summary>
    public bool HireStaff(int staffID)
    {
        if (_gameData.HiredStaffIDs.Contains(staffID))
        {
            Debug.LogWarning($"Staff {staffID} is already hired!");
            return false;
        }
        
        StaffData staffData = DataManager.Instance.StaffDict[staffID];
        if (staffData == null)
        {
            Debug.LogWarning($"Staff {staffID} not found in data!");
            return false;
        }
        
        // TODO: 비용 체크 로직 추가 (현재 자금 >= staffData.HireCost)
        // if (CurrentMoney < staffData.HireCost)
        // {
        //     Debug.LogWarning($"Not enough money to hire staff {staffID}!");
        //     return false;
        // }
        
        _gameData.HiredStaffIDs.Add(staffID);
        Debug.Log($"Hired staff {staffID} - {staffData.NameTextID}");
        
        // TODO: 비용 차감
        // CurrentMoney -= staffData.HireCost;
        
        // 저장
        SaveManager.Instance.Save();
        
        return true;
    }
    
    /// <summary>
    /// 직원 해고
    /// </summary>
    public void FireStaff(int staffID)
    {
        if (_gameData.HiredStaffIDs.Contains(staffID))
        {
            _gameData.HiredStaffIDs.Remove(staffID);
            Debug.Log($"Fired staff {staffID}");
            SaveManager.Instance.Save();
        }
    }
    
    /// <summary>
    /// 직원이 고용되어 있는지 확인
    /// </summary>
    public bool IsStaffHired(int staffID)
    {
        return _gameData.HiredStaffIDs.Contains(staffID);
    }
    
    /// <summary>
    /// 고용된 모든 직원 ID 목록
    /// </summary>
    public List<int> GetHiredStaffIDs()
    {
        return _gameData.HiredStaffIDs;
    }
    
    /// <summary>
    /// 메뉴 추가
    /// </summary>
    public bool AddFood(int foodID)
    {
        if (_gameData.AddedFoodIDs.Contains(foodID))
        {
            Debug.LogWarning($"Food {foodID} is already added!");
            return false;
        }
        
        FoodData foodData = DataManager.Instance.FoodDict[foodID];
        if (foodData == null)
        {
            Debug.LogWarning($"Food {foodID} not found in data!");
            return false;
        }
        
        // TODO: 비용 체크 로직 추가 (현재 자금 >= foodData.AddCost)
        // if (CurrentMoney < foodData.AddCost)
        // {
        //     Debug.LogWarning($"Not enough money to add food {foodID}!");
        //     return false;
        // }
        
        _gameData.AddedFoodIDs.Add(foodID);
        Debug.Log($"Added food {foodID} - {foodData.NameTextID}");
        
        // TODO: 비용 차감
        // CurrentMoney -= foodData.AddCost;
        
        // 저장
        SaveManager.Instance.Save();
        
        return true;
    }
    
    /// <summary>
    /// 메뉴 제거
    /// </summary>
    public void RemoveFood(int foodID)
    {
        if (_gameData.AddedFoodIDs.Contains(foodID))
        {
            _gameData.AddedFoodIDs.Remove(foodID);
            Debug.Log($"Removed food {foodID}");
            SaveManager.Instance.Save();
        }
    }
    
    /// <summary>
    /// 메뉴가 추가되어 있는지 확인
    /// </summary>
    public bool IsFoodAdded(int foodID)
    {
        return _gameData.AddedFoodIDs.Contains(foodID);
    }
    
    /// <summary>
    /// 추가된 모든 메뉴 ID 목록
    /// </summary>
    public List<int> GetAddedFoodIDs()
    {
        return _gameData.AddedFoodIDs;
    }
}
