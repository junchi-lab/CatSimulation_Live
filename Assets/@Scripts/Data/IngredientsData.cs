using System;
using System.Collections.Generic;

[Serializable]
public class IngredientsData
{
    public int IngredientID;
    public string NameTextID;
    public string DescriptionTextID;
    public string IconImageID;
    public int Price;         // 구매 가격
    public int Storage;       // 보관 기간 (일)
}

[Serializable]
public class IngredientsDataLoader : IDataLoader<int, IngredientsData>
{
    public List<IngredientsData> items = new List<IngredientsData>();

    public Dictionary<int, IngredientsData> MakeDict()
    {
        Dictionary<int, IngredientsData> dict = new Dictionary<int, IngredientsData>();
        foreach (var item in items)
            dict.Add(item.IngredientID, item);
        return dict;
    }

    public bool Validate()
    {
        return items != null && items.Count > 0;
    }
}
