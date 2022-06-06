using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SheenScript : MonoBehaviour
{
    [SerializeField] RectTransform sheen;
    [SerializeField] Vector2 positions;
    [SerializeField] float time;
    [SerializeField] float waitTime;

    private void Start()
    {
        ResetX();
    }

    void ResetX()
    {
        sheen.localPosition = new Vector3(positions.x, 0, 0);
        Invoke(nameof(MoveX), waitTime);
    }

    void MoveX()
    {
        sheen.DOLocalMoveX(positions.y, time).OnComplete(() => ResetX());
    }
}
