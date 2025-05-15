using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _treshhold = 0.1f;
    [SerializeField] private float _botOffsetY = 1.5f;

    public IEnumerator Move(Vector3 target)
    {
        target = new Vector3(target.x, target.y + _botOffsetY, target.z);

        while (Vector3.SqrMagnitude(target-transform.position) > _treshhold)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
