using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    [RequireComponent (typeof (CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class CharacterMove : MonoBehaviour
    {
        public event Action EndGame;

        [SerializeField]
        private float _moveSpeed = 5, _rotateSpeed = 10;
        [Space]
        [SerializeField]
        private string _triggerTagForEndGame = "Finish";

        private CharacterController _characterController;
        private Animator _animator;
        private Vector3 _moveDirection = Vector3.zero;
        private bool _isEndGame = false;    

        private bool IsMove => _moveDirection.sqrMagnitude > 0 && !_isEndGame;

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
            _animator.SetBool("isRun", IsMove);

            Move();
            Rotate();
        }

        private void Move()
        {
            if (!IsMove) return;

            _characterController.Move(_moveDirection * _moveSpeed * Time.fixedDeltaTime);
        }

        private void Rotate()
        {
            if (!IsMove) return;
            
            transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg, 0));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != _triggerTagForEndGame) return;
            if(_isEndGame) return;

            _isEndGame = true;
            EndGame?.Invoke();
        }
    }
}