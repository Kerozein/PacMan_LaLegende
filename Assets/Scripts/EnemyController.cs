using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Vector2Int enemyPosition;
    private GraphController _graphController;

    private void Start()
    {
        _graphController = GameObject.Find("Grid").GetComponent<GraphController>();
    }

    private void Update()
    {
        enemyPosition = Toolbox.ConvertWorldPosToGraphPos(gameObject.transform.position, _graphController);
    }

}
