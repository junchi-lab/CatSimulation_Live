using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Tilemap의 Grid 위치를 시각화하는 헬퍼 클래스
/// </summary>
public class TilemapHelper : MonoBehaviour
{
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap _tilemap;
    
    [Header("Display Settings")]
    [SerializeField] private bool _showGridPositions = true;
    [SerializeField] private Color _textColor = Color.yellow;
    [SerializeField] private float _textSize = 0.3f;
    [SerializeField] private Vector2 _textOffset = new Vector2(0, 0);
    
    private void OnValidate()
    {
        if (_tilemap == null)
        {
            _tilemap = GetComponent<Tilemap>();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_tilemap == null || !_showGridPositions)
            return;

        DrawGridPositions();
    }

    /// <summary>
    /// Tilemap의 모든 타일 위치에 좌표 표시
    /// </summary>
    private void DrawGridPositions()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                
                // 해당 위치에 타일이 있는지 확인
                if (_tilemap.HasTile(cellPosition))
                {
                    // Grid 좌표를 타일의 중앙 World 좌표로 변환
                    Vector3 worldPosition = _tilemap.GetCellCenterWorld(cellPosition);
                    worldPosition += new Vector3(_textOffset.x, _textOffset.y, 0);
                    
                    // 좌표 텍스트 표시
                    string positionText = $"({x},{y})";
                    DrawString(positionText, worldPosition, _textColor, _textSize);
                }
            }
        }
    }

    /// <summary>
    /// Gizmos를 이용한 텍스트 그리기
    /// </summary>
    private void DrawString(string text, Vector3 worldPosition, Color color, float size)
    {
        UnityEditor.Handles.BeginGUI();
        
        Vector3 screenPosition = UnityEditor.HandleUtility.WorldToGUIPoint(worldPosition);
        
        GUIStyle style = new GUIStyle();
        style.normal.textColor = color;
        style.fontSize = Mathf.RoundToInt(size * 50);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        
        // 배경 그림자 효과
        GUIStyle shadowStyle = new GUIStyle(style);
        shadowStyle.normal.textColor = Color.black;
        
        Vector2 labelSize = style.CalcSize(new GUIContent(text));
        Vector2 position = new Vector2(screenPosition.x - labelSize.x / 2, screenPosition.y - labelSize.y / 2);
        
        // 그림자
        GUI.Label(new Rect(position.x + 1, position.y + 1, labelSize.x, labelSize.y), text, shadowStyle);
        // 텍스트
        GUI.Label(new Rect(position.x, position.y, labelSize.x, labelSize.y), text, style);
        
        UnityEditor.Handles.EndGUI();
    }
#endif
}

