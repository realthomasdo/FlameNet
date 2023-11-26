using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{
    public static GridController instance;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private GameObject FirePrefab;
    private Vector3 lastPosition;
    private float GridSpaceSize = 2.5f;

    [SerializeField] private GameObject gridCellPrefab;
    private GridCell[,] gameGrid;
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        instance = this;
    }

    private void CreateGrid()
    {
        gameGrid = new GridCell[height, width];

        if (gridCellPrefab == null)
        {
            Debug.LogError("Grid Cell Prefab not assigned");
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x * GridSpaceSize, 0, y * GridSpaceSize), Quaternion.identity).GetComponent<GridCell>();
                gameGrid[x, y].SetPosition(x, y);
                gameGrid[x, y].transform.SetParent(transform, false);
            }
        }
    }
    public SensorInformation GetSensorInfo(Vector3 position)
    {
        if (position.x < transform.position.x ||
            position.z < transform.position.z ||
            position.x > transform.position.x + GridSpaceSize * height ||
            position.z < transform.position.z * GridSpaceSize * width)
        {
            return new SensorInformation();
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (position.x < gameGrid[x, y].transform.position.x && position.z < gameGrid[x, y].transform.position.z)
                {
                    return gameGrid[x, y].GetSensorInformation();
                }
            }
        }
        return new SensorInformation();
    }
    public void SpawnFire(Vector3 mousePos)
    {
        float delX = mousePos.x - gameGrid[0, 0].transform.position.x;
        float delZ = mousePos.z - gameGrid[0, 0].transform.position.z;
        int xSteps = (int)(Mathf.Abs(delX) / 2.5);
        int zSteps = (int)(Mathf.Abs(delZ) / 2.5);
        if (xSteps < 0 || xSteps >= height || zSteps < 0 || zSteps >= width)
        {
            return;
        }
        FireController fire = Instantiate(FirePrefab, gameGrid[xSteps, zSteps].transform.position, FirePrefab.transform.rotation).GetComponent<FireController>();
        fire.SetCells(GetCellsAround(xSteps, zSteps));
    }
    private List<(GridCell, float)> GetCellsAround(int xStart, int yStart)
    {
        List<(GridCell, float)> grids = new List<(GridCell, float)>
        {
            (gameGrid[xStart, yStart], 1)
        };
        float maxDist = 2;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(xStart, yStart));
                if (dist <= maxDist)
                {
                    grids.Add((gameGrid[x, y], dist));
                }
            }
        }
        return grids;
    }
}
