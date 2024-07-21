using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;

    private const float _speed = 3f;
    private float _tempSpeed;

    private void FixedUpdate()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        _tempSpeed = dist * _speed;

        transform.position = Vector3.MoveTowards(transform.position, target.position, _tempSpeed * Time.fixedDeltaTime);
    }

    //public void SetTarget(Transform target) => this.target = target;
}
