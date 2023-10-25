using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Vector2Int playerPosition;
    private GraphController _graphController;

    private void Start()
    {
        _graphController = GameObject.Find("Grid").GetComponent<GraphController>();
    }

    private void Update()
    {
        playerPosition = Toolbox.ConvertWorldPosToGraphPos(gameObject.transform.position, _graphController);
    }
}
