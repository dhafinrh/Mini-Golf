using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obsctale : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] List<Transform> positions;
    int index;

    private void OnEnable()
    {
        Move();
    }

    private void Move()
    {
        Transform pos = positions[index];
        this.transform.DOMove(pos.position, duration).SetEase(Ease.InOutElastic).onComplete = Move;
        
        index += 1;
        if (index == positions.Count)
            index = 0;
    }

}
