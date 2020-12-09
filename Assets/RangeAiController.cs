using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Ludiq;
using Player;
using Projectiles.Fireball;
using Stats;
using UnityEngine;

/// <inheritdoc />
public class RangeAiController : PathFindingController
{
        private enum EDirection {
            NONE       = 0,
            HORIZONTAL = 1,
            VERTICAL   = 2
        }
 
        private EDirection m_lastDirection = EDirection.NONE;
 
        private bool m_isMovingVertically   = false;
        private bool m_isMovingHorizontally = false;
        
        private float _speed;
        private float _velocitizedSpeed;

        public Rigidbody2D rigidBody2D;
        
        public CharacterStats characterStatsController;

        private bool _projectileFireRateLock;
        
        Vector3 _start, _end;
        Vector2 _currentVelocity;
        private float _startTime;
        private float _distance;
        private float _velocity;

        public GameObject fireballPrefab;
        [SerializeField] private MoveDirection lastMovedDirection;


        public override void Update()
        {
            frames++;
            if (frames % 300 == 0) { //If the remainder of the current frame divided by 10 is 0 run the function.

                float? lowestDistance = null;
                int lowestIndex = 0;
                int index = 0;
                foreach (var target in potentialTargets)
                {
                    var distance = Vector3.Distance(transform.position, target.transform.position);
                    if (lowestDistance == null)
                    {
                        lowestDistance = distance;
                        lowestIndex = index;
                    }

                    if (distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        lowestIndex = index;
                    }

                    index += 1;
                }

                if (lowestDistance >= stopDistance && lowestDistance <= seekingDistance)
                {
                    targetLocation = potentialTargets[lowestIndex];
                    CalculatePathAndStartMovementCoroutine();
                }
                else
                {
                    if (currentRoutine != null)
                        StopCoroutine(currentRoutine);
                }
                frames = 0;
            }


            if (_projectileFireRateLock)
                return;

            _projectileFireRateLock = true;
            ShootProjectile(MoveDirection.Up);
            StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
            _speed = characterStatsController.SpeedController.baseSpeed;
        }

        void LateUpdate()
        {
        }

        void Awake()
        {
            _speed = characterStatsController.SpeedController.baseSpeed;

            rigidBody2D = GetComponent<Rigidbody2D>();
        }
        
        private IEnumerator ProjectileLock(float attackRate)
        {
            yield return new WaitForSeconds(attackRate);
            _projectileFireRateLock = false;
        }

        void ShootProjectile(MoveDirection moveDirection)
        {
            var characterControllerPosition = rigidBody2D.position;
            _velocitizedSpeed = _speed * _velocity;
            
            switch (moveDirection)
            {
                case MoveDirection.Down:
                    characterControllerPosition.y += -2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;
                case MoveDirection.DownRight:
                    characterControllerPosition.x += 2.0f;
                    characterControllerPosition.y += -2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;
                case MoveDirection.DownLeft:
                    characterControllerPosition.x += -2.0f;
                    characterControllerPosition.y += -2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;
                    
                case MoveDirection.Up:
                    characterControllerPosition.y += 2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;    
                
                case MoveDirection.UpRight:
                    characterControllerPosition.x += 4.0f;
                    characterControllerPosition.y += 2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;   
                
                case MoveDirection.UpLeft:
                    characterControllerPosition.x += -4.0f;
                    characterControllerPosition.y += 2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;

                case MoveDirection.Right:
                    characterControllerPosition.x += 2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;
                    
                case MoveDirection.Left:
                    characterControllerPosition.x += -2.0f;
                    Instantiate(fireballPrefab, characterControllerPosition, rigidBody2D.transform.rotation);
                    break;
            }

            var fireBallScript = fireballPrefab.GetComponent<ProjectileController>();
            fireBallScript.playerSpeed = _velocitizedSpeed;
            fireBallScript.moveDirection = moveDirection;
            fireBallScript.projectileOwner = this.GameObject();
        }
}
