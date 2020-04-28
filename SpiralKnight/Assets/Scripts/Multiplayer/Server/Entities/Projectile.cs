using System;
using System.Collections;
using Multiplayer.Client.Entities;
using UnityEngine;

namespace Multiplayer.Server.Entities
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : Destroyable
    {
        public float lifeTime = 5f;
        public float damage;
        public float speed;

        public bool ofPlayer;

        private void OnTriggerEnter(Collider _other)
        {
            Debug.Log($"{_other.name} : {_other.tag}");
            if (ofPlayer)
            {
                if (_other.CompareTag("Destructible"))
                {
                    _other.GetComponent<Destructible>().Hit();
                    base.Destroy();
                }
                else if (_other.CompareTag("Enemy"))
                {
                    _other.GetComponent<Enemy>().TakeDamage(damage);
                    base.Destroy();
                }
            }
            else
            {
                if (_other.CompareTag("Player"))
                {
                    _other.GetComponent<Player>().TakeDamage(damage);
                    base.Destroy();
                }
            }
        }

        private Vector3 movement;

        public void Initialize(Vector3 _position, Vector3 _direction, float _damage, float _speed, bool _ofPlayer)
        {
            ofPlayer = _ofPlayer;

            transform.position = _position;
            transform.forward = _direction;
            damage = _damage;
            speed = _speed;

            movement = _direction * (speed * Time.fixedDeltaTime);
            StartCoroutine(Destroy());
        }

        private new IEnumerator Destroy()
        {
            yield return new WaitForSeconds(lifeTime);
            base.Destroy();
        }

        private void FixedUpdate()
        {
            transform.position += movement;
        }
    }
}