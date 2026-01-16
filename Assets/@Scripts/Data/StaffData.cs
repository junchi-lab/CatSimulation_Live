using System;
using System.Collections.Generic;

[Serializable]
public class StaffData
{
    public int StaffID;
    public int StaffType; // 1: Kitchen, 2: Hall
    public string NameTextID;
    public string Position;
    public int WorkSpeed;    // 작업 속도 (100 = 기본, 120 = 빠름, 80 = 느림)
    
    // 스탯 (0-100)
    public int Efficiency;   // 효율성 - 업무 처리 효율
    public int Stamina;      // 체력 - 지구력, 피로도 관련
    public int Skill;        // 숙련도 - 전문 기술 수준
    public int Experience;   // 경험치 - 전체적인 경험 수준
    
    public int Salary;       // 월급 (200만원 ~ 3000만원)
    public int HireCost;     // 고용비용 (최대 10억원)
}

[Serializable]
public class StaffDataLoader : IDataLoader<int, StaffData>
{
    public List<StaffData> items = new List<StaffData>();

    public Dictionary<int, StaffData> MakeDict()
    {
        Dictionary<int, StaffData> dict = new Dictionary<int, StaffData>();
        foreach (var item in items)
            dict.Add(item.StaffID, item);
        return dict;
    }

    public bool Validate()
    {
        return items != null && items.Count > 0;
    }

    public List<StaffData> GetKitchenStaff()
    {
        if (items == null)
            return new List<StaffData>();
        return items.FindAll(s => s.StaffType == 1);
    }

    public List<StaffData> GetHallStaff()
    {
        if (items == null)
            return new List<StaffData>();
        return items.FindAll(s => s.StaffType == 2);
    }
}

