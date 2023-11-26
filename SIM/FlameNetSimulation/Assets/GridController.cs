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

    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = GetSelectedMapPos();
            float delX = mousePos.x - gameGrid[0, 0].transform.position.x;
            float delZ = mousePos.z - gameGrid[0, 0].transform.position.z;
            int xSteps = (int) (Mathf.Abs(delX) / 2.5);
            int zSteps = (int) (Mathf.Abs(delZ) / 2.5);
            gameGrid[xSteps, zSteps].sensorInfo.temp = 100f;
        }
    }
}
