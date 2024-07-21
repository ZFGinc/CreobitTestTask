using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    [RequireComponent (typeof (CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class CharacterMove : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed = 5, _rotateSpeed = 10;

        private CharacterController _characterController;
        private Animator _animator;
        private Vector3 _moveDirection = Vector3.zero;

        private bool IsMove => _moveDirection.sqrMagnitude > 0;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            _moveDirection.x = x;
            _moveDirection.z = y;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            _characterController.Move(_moveDirection * _moveSpeed * Time.fixedDeltaTime);

            _animator.SetBool("isRun", IsMove);
        }

        private void Rotate()
        {
            if (IsMove)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg, 0));
            }
        }
    }
}