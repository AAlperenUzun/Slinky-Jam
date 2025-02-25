using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    public GameObject slinkyPrefab;
    public GameObject gridModelPrefab;
    public int gridWidth = 5;
    public int gridHeight = 5;
    public float gridDistance = 1.0f;
    public SlinkyScript[,] grid;
    public Transform spawnPoint;
    public float spawnDelay = 0.5f;
    public TMPro.TextMeshProUGUI scoreText;
    private int score = 0;
    
    [System.Serializable]
    public class SlinkyData
    {
        public Vector3 firstGridPosition;
        public Vector3 secondGridPosition;
        public Color slinkyColor;
        public int orderInLayer;
    }
    public SkinnedMeshRenderer skinnedMesh;
    public Transform slinkyParent;
    public List<SlinkyData> slinkyList = new List<SlinkyData>();

    void Start()
    {
        grid = new SlinkyScript[gridWidth, gridHeight];
        GenerateGrid();
        SpawnSlinky();
    }

    void GenerateGrid()
    {
        Vector2 centerOffset = new Vector2((gridWidth - 1) * gridDistance / 2, (gridHeight - 1) * gridDistance / 2);
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 gridPosition = new Vector2(x * gridDistance, y * gridDistance) - centerOffset;
                Vector3 lastGridPos= new Vector3(gridPosition.x, 0, gridPosition.y);
                Instantiate(gridModelPrefab, lastGridPos, Quaternion.identity);
            }
        }
    }

    void SpawnSlinky()
    {
        GameObject slinkyObj = Instantiate(slinkyPrefab, spawnPoint.position, Quaternion.identity);
        SlinkyScript slinky = slinkyObj.GetComponent<SlinkyScript>();
        slinky.Initialize(this, gridWidth / 2, 0); // Orta kolon başlangıcı
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }
}
