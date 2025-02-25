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
    

    public void Initialize(GridController manager, int x, int y)
    {
        gameManager = manager;
        gridX = x;
        gridY = y;
        transform.position = new Vector2(gridX * gameManager.gridDistance, gridY * gameManager.gridDistance) - new Vector2((gameManager.gridWidth - 1) * gameManager.gridDistance / 2, (gameManager.gridHeight - 1) * gameManager.gridDistance / 2);
        gameManager.grid[gridX, gridY] = this;
    }

    void OnMouseDown()
    {
        if (!isMoving)
        {
            MoveToNextAvailablePosition();
        }
    }

    void MoveToNextAvailablePosition()
    {
        for (int y = gridY + 1; y < gameManager.gridHeight; y++)
        {
            if (gameManager.grid[gridX, y] == null)
            {
                gameManager.grid[gridX, gridY] = null;
                gridY = y;
                gameManager.grid[gridX, gridY] = this;
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
}
