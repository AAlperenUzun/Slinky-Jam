using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

public class GridController : MonoBehaviour
{
    public GameObject slinkyPrefab;
    public GameObject gridModelPrefab;
    public int gridWidth = 5;
    public int gridHeight = 5;
    public int collectGridWidth = 7;
    public int collectGridHeight = 1;
    public float gridDistance = 1.0f;
    public Transform spawnPoint;
    public Transform collectPoint;
    public float spawnDelay = 0.5f;
    public TMPro.TextMeshProUGUI scoreText;
    private int score = 0;
    
    [System.Serializable]
    public class SlinkyData
    {
        public Vector2 grid1Data;
        public Vector2 grid2Data;
        public Colors color;
        public int orderInLayer;
    }

    [System.Serializable]
    public class ColorList
    {
        public Colors color;
        public Material mat;
        public float colorScore;
    }

    public enum Colors
    {
        yellow,
        green,
        blue,
        purple,
        red
    }
    [System.Serializable]
    public class GridSlot
    {
        public Vector3 gridPosition;
    }
    public SkinnedMeshRenderer skinnedMesh;
    public Transform slinkyParent;
    public List<SlinkyData> slinkyList = new List<SlinkyData>();
    public List<ColorList> colorLists;
    private GridSlot[,] grid=new GridSlot[0,0];
    private GridSlot[,] collectGrid=new GridSlot[0,0];
    public List<SlinkyScript> createdSlinkies = new List<SlinkyScript>();

    void Start()
    {
        GenerateGrid();
        SpawnInitialSlinkies();
        grid = new GridSlot[gridWidth, gridHeight];
    }

    public Vector3 GetGridPos(Vector2 gridNum)
    {
        return grid[(int)gridNum.x, (int)gridNum.y].gridPosition;
    }
    void GenerateGrid()
    {
        Vector2 centerOffset = new Vector2((gridWidth - 1) * gridDistance / 2, -(gridHeight - 1) * gridDistance / 2);
        grid = new GridSlot[gridWidth, gridHeight];
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 gridPosition = new Vector2(x * gridDistance, -y * gridDistance) - centerOffset;
                Vector3 lastGridPos= new Vector3(gridPosition.x, 0, gridPosition.y);
                grid[x, y] = new GridSlot { gridPosition = lastGridPos};
                Instantiate(gridModelPrefab, lastGridPos, Quaternion.identity);
            }
        }
        
        centerOffset = new Vector2((collectGridWidth - 1) * gridDistance / 2, -(collectGridHeight - 1) * gridDistance / 2);
        collectGrid = new GridSlot[collectGridWidth, collectGridHeight];
        for (int y = 0; y < collectGridHeight; y++)
        {
            for (int x = 0; x < collectGridWidth; x++)
            {
                Vector2 gridPosition = new Vector2(x * gridDistance, -y * gridDistance) - centerOffset;
                Vector3 lastGridPos= new Vector3(gridPosition.x, 0, gridPosition.y)+collectPoint.position;
                collectGrid[x, y] = new GridSlot { gridPosition = lastGridPos};
                Instantiate(gridModelPrefab, lastGridPos, Quaternion.identity);
            }
        }
        
    }
    
    public Material GetMaterialByColor(Colors targetColor)
    {
        return colorLists.FirstOrDefault(c => c.color == targetColor)?.mat;
    }
    void SpawnInitialSlinkies()
    {
        foreach (var slinky in slinkyList)
        {
            SpawnSlinky(slinky);
        }
    }

    void SpawnSlinky(SlinkyData slinkyData)
    {
        Vector3 firstPosition = grid[(int)slinkyData.grid1Data.x, (int)slinkyData.grid1Data.y].gridPosition;
        Vector2 secondPosition = grid[(int)slinkyData.grid2Data.x, (int)slinkyData.grid2Data.y].gridPosition;
            GameObject slinkyObj = Instantiate(slinkyPrefab, firstPosition, Quaternion.identity, slinkyParent);
            slinkyObj.transform.position = firstPosition;
            Vector3 targetPos = GetGridPos(slinkyData.grid2Data);
            Vector3 direction = targetPos - slinkyObj.transform.position;
            direction.y = 0; // Y eksenindeki farkı yok sayarak sadece yatay düzlemde dönmesini sağla

            if (direction != Vector3.zero)
            {
                slinkyObj.transform.rotation = Quaternion.LookRotation(direction);
            }

            SlinkyScript slinky = slinkyObj.GetComponent<SlinkyScript>();
            slinky.skinnedMesh.material=GetMaterialByColor(slinkyData.color);
            slinky.Initialize(this, slinkyData);
            // grid[gridX, nextGridY].occupiedSlinky = slinky;
            createdSlinkies.Add(slinky);
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }
}
