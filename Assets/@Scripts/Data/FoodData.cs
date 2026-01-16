using System;
using System.Collections.Generic;

[Serializable]
public class FoodData
{
    public int FoodID;
    public int FoodType;      // 1: Noodles(면), 2: Rice(밥), 3: Dishes(요리)
    public string NameTextID;
    public string DescriptionTextID;
    public string IconImageID;
    public int Price;         // 가격
    public int CookTime;      // 조리 시간 (초)
    public int Popularity;    // 인기도 (0-100)
    public int AddCost;    // 도입 비용
}

[Serializable]
public class FoodDataLoader : IDataLoader<int, FoodData>
{
    public List<FoodData> items = new List<FoodData>();

    public Dictionary<int, FoodData> MakeDict()
    {
        Dictionary<int, FoodData> dict = new Dictionary<int, FoodData>();
        foreach (var item in items)
            dict.Add(item.FoodID, item);
        return dict;
    }

    public bool Validate()
    {
        return items != null && items.Count > 0;
    }
}

