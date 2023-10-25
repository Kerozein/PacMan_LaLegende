using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Vector2Int enemyPosition;
    public GraphController _graphController;

    private void Start()
    {
        _graphController = GameObject.Find("Grid").GetComponent<GraphController>();
        enemyPosition = Toolbox.ConvertWorldPosToGraphPos(gameObject.transform.position, _graphController);
    }

    private void Update()
    {
        enemyPosition = Toolbox.ConvertWorldPosToGraphPos(gameObject.transform.position, _graphController);
    }

}
