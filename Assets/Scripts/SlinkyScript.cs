using DG.Tweening;
using UnityEngine;

public class SlinkyScript : MonoBehaviour
{
 private GridController gameManager;
    public float moveSpeed = 0.3f;
    public float stretchFactor = 1.5f;
    public float squishFactor = 0.8f;
    private Vector2 targetPosition;
    private bool isMoving = false;
    public int gridX, gridY;
    public SkinnedMeshRenderer skinnedMesh;
    private GridController.SlinkyData _slinkyData;
    public float slinkyBaseLength = 2f;
    public Transform bone1, bone2, bone3;
    

    public void Initialize(GridController manager, GridController.SlinkyData slinkyData)
    {
        gameManager = manager;
        _slinkyData = slinkyData;
        // transform.position = new Vector2(gridX * gameManager.gridDistance, gridY * gameManager.gridDistance) - new Vector2((gameManager.gridWidth - 1) * gameManager.gridDistance / 2, (gameManager.gridHeight - 1) * gameManager.gridDistance / 2);
        // gameManager.grid[gridX, gridY] = this;
        StretchSlinky();
    }
    public void StretchSlinky()
    {
        // 1. Başlangıç noktasını ayarla
        // transform.position = new Vector3(startGrid.x, 0, startGrid.y);

        // Mevcut mesafeyi hesapla
        float currentDistance = Vector3.Distance(gameManager.GetGridPos(_slinkyData.grid1Data), gameManager.GetGridPos(_slinkyData.grid2Data));


        // Hedef yönü (rotation etkilemeden)
        Vector3 direction = (gameManager.GetGridPos(_slinkyData.grid2Data) - transform.position).normalized;
        
        // 4. Bone başına eklenecek mesafeyi belirle (toplam ekleme / 3)
        Vector3 boneStep = (direction * (currentDistance-slinkyBaseLength)) / 3f;
        
        bone1.position += boneStep;
        bone2.position += boneStep;
        bone3.position += boneStep;
    }

    void OnMouseDown()
    {
        if (!isMoving)
        {
            // MoveToNextAvailablePosition();
        }
    }

    void MoveToNextAvailablePosition()
    {
        for (int y = gridY + 1; y < gameManager.gridHeight; y++)
        {
                // gameManager.grid[gridX, gridY] = null;
                gridY = y;
                // gameManager.grid[gridX, gridY] = this;
                targetPosition = new Vector2(gridX * gameManager.gridDistance, gridY * gameManager.gridDistance) - new Vector2((gameManager.gridWidth - 1) * gameManager.gridDistance / 2, (gameManager.gridHeight - 1) * gameManager.gridDistance / 2);
                isMoving = true;

                Sequence slinkySequence = DOTween.Sequence();
                slinkySequence.Append(transform.DOScaleX(0.9f, moveSpeed / 2))
                    .Join(transform.DOScaleY(stretchFactor, moveSpeed / 2))
                    .Append(transform.DOMove(targetPosition, moveSpeed).SetEase(Ease.InOutSine))
                    .Append(transform.DOScaleY(squishFactor, moveSpeed / 4))
                    .Append(transform.DOScaleX(1f, moveSpeed / 4))
                    .Join(transform.DOScaleY(1f, moveSpeed / 4))
                    .OnComplete(() => isMoving = false);
                break;
        }
    }
}
