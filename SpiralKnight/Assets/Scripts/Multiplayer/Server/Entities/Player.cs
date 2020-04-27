using System.Collections;
using UnityEngine;

namespace Multiplayer.Server.Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : Entity
    {
        private float Gravity = -9.81f;
        
        public string username;
        public CharacterController controller;

        public float moveSpeed = 5f;
        

        private bool[] inputs;

        private void Start()
        {
            Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
            moveSpeed *= Time.fixedDeltaTime;
        }

        public void Initialize(int _id, string _username)
        {
            id = _id;
            base.Initialize(100);
            username = _username;
            inputs = new bool[5];
        }
        
        /// <summary>Updates the player input with newly received input.</summary>
        /// <param name="_inputs">The new key inputs.</param>
        /// <param name="_direction">The facing direction</param>
        public void SetInput(bool[] _inputs, Vector3 _direction)
        {
            inputs = _inputs;
            direction = _direction;
        }

        /// <summary>Processes player input and moves the player.</summary>
        protected override void EachTick()
        {
            if (health <= 0f) return;

            Vector2 _inputDirection = Vector2.zero;
            if (inputs[0])
            {
                _inputDirection.y += 1;
            }

            if (inputs[1])
            {
                _inputDirection.y -= 1;
            }

            if (inputs[2])
            {
                _inputDirection.x -= 1;
            }

            if (inputs[3])
            {
                _inputDirection.x += 1;
            }

            Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
            _moveDirection *= moveSpeed;

            float _yVelocity = 0f;
            if (!controller.isGrounded)
                _yVelocity += Gravity;

            _moveDirection.y = _yVelocity;
            controller.Move(_moveDirection);
        }

        
        public override void Die()
        {
            health = 0f;
            controller.enabled = false;
            transform.position = new Vector3(0f, 25f, 0f);
            ServerSend.EntityTransform(this);
            StartCoroutine(Respawn());
        }

        public override void Attack(Vector3 _direction)
        {
            if (health <= 0f) return;
            ServerSend.EntityAttack(this);

            Collider[] _cols = new Collider[10];
            int _size = Physics.OverlapSphereNonAlloc(transform.position + _direction, 0.5f, _cols);
            for (int i = 0; i < _size; i++)
            {
                Collider _collider = _cols[i];
                if (_collider.CompareTag("Destructible"))
                {
                    _collider.GetComponent<Destructible>().Hit();
                }
            }
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(5f);

            health = maxHealth;
            controller.enabled = true;
            ServerSend.PlayerRespawned(this);
        }
    }
}