using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using DefaultNamespace;
using Ludiq;
using Player;
using Projectiles;
using Projectiles.Fireball;
using RPGM.Gameplay;
using Scripts.Gameplay;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UIElements;

namespace RPGM.Gameplay
{
    /// <summary>
    /// A simple controller for animating a 4 directional sprite using Physics.
    /// </summary>
    public class CharacterController2D : MonoBehaviour
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
        private float _acceleration;

        [Space] 
        public Camera playerCamera;

        public CameraController cameraController;
        public Vector3 nextMoveCommand;
        public Animator animator;
        public bool flipX = false;
        public MoveDirection currentKeyPressed;
        public List<MoveDirection> keyStrokeOrder;
        public Rigidbody2D rigidBody2D;
        SpriteRenderer _spriteRenderer;
        PixelPerfectCamera _pixelPerfectCamera;
        
        public CharacterStats characterStatsController;

        private bool projectileFireRateLock;
        
        enum State
        {
            Idle, Moving
        }

        State state = State.Idle;
        Vector3 start, end;
        Vector2 currentVelocity;
        float startTime;
        float distance;
        float velocity;

        public GameObject fireballPrefab;
        [SerializeField] private MoveDirection lastMovedDirection;

        private void Start()
        { 
            cameraController = playerCamera.GetComponent<CameraController>();
            cameraController.UpdateCamera();
        }

        void IdleState()
        {
            if (nextMoveCommand != Vector3.zero)
            {
                start = transform.position;
                end = start + nextMoveCommand;
                distance = (end - start).magnitude;
                velocity = 0;
                //UpdateAnimator(nextMoveCommand);
                nextMoveCommand = Vector3.zero;
                state = State.Moving;
            }
        }

        void MoveState()
        {
            velocity = Mathf.Clamp01(velocity + Time.deltaTime * _acceleration);
            //UpdateAnimator(nextMoveCommand);
            rigidBody2D.velocity = Vector2.SmoothDamp(rigidBody2D.velocity, nextMoveCommand * _speed, ref currentVelocity, _acceleration, _speed);
            _spriteRenderer.flipX = rigidBody2D.velocity.x >= 0 ? true : false;
        }

        void UpdateAnimator(Vector3 direction)
        {
            if (animator)
            {
                animator.SetInteger("WalkX", direction.x < 0 ? -1 : direction.x > 0 ? 1 : 0);
                animator.SetInteger("WalkY", direction.y < 0 ? 1 : direction.y > 0 ? -1 : 0);
            }
        }

        void Update()
        {
            _speed = characterStatsController.SpeedController.baseSpeed;
            HandleMovement();
            // HandleMovementTwo();
            switch (state)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Moving:
                    break;
            }
        }

        void LateUpdate()
        {
            if (_pixelPerfectCamera != null)
            {
                transform.position = _pixelPerfectCamera.RoundToPixel(transform.position);
            }
        }

        void Awake()
        {
            _speed = characterStatsController.SpeedController.baseSpeed;
            _acceleration = characterStatsController.SpeedController.acceleration;

            rigidBody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _pixelPerfectCamera = GameObject.FindObjectOfType<PixelPerfectCamera>();
        }

        
        
        
        void HandleMovement()
        {
            bool moved = false;
            rigidBody2D.velocity = new Vector2(0,0);
            velocity = 0;
            animator.SetInteger("WalkY", 0);
            animator.SetInteger("WalkX", 0);
            if (Input.anyKey == false)
            {
                currentKeyPressed = MoveDirection.None;
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                if (keyStrokeOrder.Contains(MoveDirection.Up) == false)
                    keyStrokeOrder.Add(MoveDirection.Up);
                currentKeyPressed = MoveDirection.Up;
            } 
            if (Input.GetKey(KeyCode.A))
            {
                if (keyStrokeOrder.Contains(MoveDirection.Left) == false)
                    keyStrokeOrder.Add(MoveDirection.Left);
                currentKeyPressed = MoveDirection.Left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (keyStrokeOrder.Contains(MoveDirection.Right) == false)
                    keyStrokeOrder.Add(MoveDirection.Right);
                currentKeyPressed = MoveDirection.Right;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (keyStrokeOrder.Contains(MoveDirection.Down) == false)
                    keyStrokeOrder.Add(MoveDirection.Down);
                currentKeyPressed = MoveDirection.Down;
            }
            
            KeyStrokeOrderClean();

            if (keyStrokeOrder.Count > 0)
            {
                var bestMoveDirection = keyStrokeOrder[keyStrokeOrder.Count - 1];
                if (bestMoveDirection == MoveDirection.Up)
                {
                    animator.SetInteger("WalkY",1);
                    moved = MoveCharacter(Vector2.up * _speed, true, false);
                }
                else if (bestMoveDirection == MoveDirection.Left)
                {
                    animator.SetInteger("WalkX", -1);
                    moved = MoveCharacter(Vector2.left * _speed, false, false);
                }
                else if (bestMoveDirection == MoveDirection.Down)
                {
                    animator.SetInteger("WalkY", -1);
                    moved = MoveCharacter(Vector2.down * _speed, true, true);
                    currentKeyPressed = MoveDirection.Down;
                }
                else if (bestMoveDirection == MoveDirection.Right)
                {
                    animator.SetInteger("WalkX", 1);
                    moved = MoveCharacter(Vector2.right * _speed, flipX, false);
                }
            }
            HandleProjectileShooting();

            if(Input.anyKey == false)
            {
                currentKeyPressed = MoveDirection.None;
                keyStrokeOrder = new List<MoveDirection>();
                MoveState();
                state = State.Idle;
                velocity = 0;
            }
        }
        void HandleMovementTwo()
        {
            float vertical   = Input.GetAxisRaw ("Vertical");
            float horizontal = Input.GetAxisRaw ("Horizontal");
            if(vertical == 0f) {
                m_isMovingVertically = false;
            }
            if(horizontal == 0f) {
                m_isMovingHorizontally = false;
            }
            if(vertical == 0f && horizontal == 0f) {
                m_lastDirection = EDirection.NONE;
                return;
            }
 
            if(!m_isMovingVertically && vertical != 0f) {
                // The user has just pressed a vertical key
                m_lastDirection = EDirection.VERTICAL;
                //anim.SetFloat("hSpeed",0);
                m_isMovingVertically = true;
            }
            else if(!m_isMovingHorizontally && horizontal != 0f) {
                // The user has just pressed an horizontal key
                m_lastDirection = EDirection.HORIZONTAL;
                //anim.SetFloat("vSpeed",0);
                m_isMovingHorizontally = true;
            }
 
            Vector3 movement = Vector3.zero;
            switch(m_lastDirection) {
                case EDirection.VERTICAL :
                    movement = (vertical * Vector2.up).normalized;
                    transform.Translate(movement * (_speed * Time.deltaTime));
                    return;
                case EDirection.HORIZONTAL :
                    movement = (horizontal * Vector2.right).normalized;
                    transform.Translate(movement * (_speed * Time.deltaTime));
                    return;
                default :
                    break;
            }
            // HandleMovement();
        }
        private bool MoveCharacter(Vector2 velocityVector, bool flipSpriteDirection, bool ignoreFlip)
        {
            cameraController.UpdateCamera();
            if (!ignoreFlip)
            {
                if (flipSpriteDirection == false)
                {
                    _spriteRenderer.flipX = false;
                }
                if (flipSpriteDirection)
                {
                    _spriteRenderer.flipX = true;
                }
                rigidBody2D.velocity = velocityVector;
                state = State.Moving;
                return true;   
            }
            velocity = 1;
            rigidBody2D.velocity = velocityVector;
            return false;
        }
        private void KeyStrokeOrderClean()
        {
            var newList = new List<MoveDirection>();
            foreach (var moveDirection in keyStrokeOrder)
            {
                if (moveDirection == MoveDirection.Down)
                {
                    if (Input.GetKey(KeyCode.S))
                        newList.Add(moveDirection);
                }
                if (moveDirection == MoveDirection.Up)
                {
                    if (Input.GetKey(KeyCode.W))
                        newList.Add(moveDirection);
                }
                if (moveDirection == MoveDirection.Left)
                {
                    if (Input.GetKey(KeyCode.A))
                        newList.Add(moveDirection);
                }
                if (moveDirection == MoveDirection.Right)
                {
                    if (Input.GetKey(KeyCode.D))
                        newList.Add(moveDirection);
                }
            }

            if (newList.Count != 0)
                lastMovedDirection = newList[newList.Count - 1];
            else
                lastMovedDirection = MoveDirection.Right;
            
            keyStrokeOrder = newList;
        }

        
        
        /// <summary>
        /// Instantiate Character Projectile and Shoot Directionally
        /// </summary>
        void HandleProjectileShooting()
        {
            if (projectileFireRateLock != true)
            {
                bool projectileShot = false;
                MoveDirection lastKeyPress = MoveDirection.None;
                if (keyStrokeOrder.Count >= 2)
                {
                    lastKeyPress = keyStrokeOrder[keyStrokeOrder.Count - 2];
                    if (lastKeyPress == MoveDirection.Right && Input.GetKeyDown(KeyCode.UpArrow) ||
                        lastKeyPress == MoveDirection.Up && Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        ShootProjectile(MoveDirection.UpLeft);
                        projectileShot = true;
                        projectileFireRateLock = true;
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }

                    if (lastKeyPress == MoveDirection.Left && Input.GetKeyDown(KeyCode.UpArrow) ||
                        lastKeyPress == MoveDirection.Up && Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        ShootProjectile(MoveDirection.UpRight);
                        projectileShot = true;
                        projectileFireRateLock = true;
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }

                    if (lastKeyPress == MoveDirection.Right && Input.GetKeyDown(KeyCode.DownArrow) ||
                        lastKeyPress == MoveDirection.Down && Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        ShootProjectile(MoveDirection.DownLeft);
                        projectileShot = true;
                        projectileFireRateLock = true;
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }

                    if (lastKeyPress == MoveDirection.Left && Input.GetKeyDown(KeyCode.DownArrow) ||
                        lastKeyPress == MoveDirection.Down && Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        ShootProjectile(MoveDirection.DownRight);
                        projectileShot = true;
                        projectileFireRateLock = true;
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }
                }

                if (projectileShot == false)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        projectileShot = true;
                        projectileFireRateLock = true;
                        ShootProjectile(MoveDirection.Down);
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        projectileShot = true;
                        projectileFireRateLock = true;
                        ShootProjectile(MoveDirection.Right);
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }

                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        projectileShot = true;
                        projectileFireRateLock = true;
                        ShootProjectile(MoveDirection.Left);
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        projectileShot = true;
                        projectileFireRateLock = true;
                        ShootProjectile(MoveDirection.Up);
                        StartCoroutine(ProjectileLock(GetComponent<Damage>().attackRate));
                    }
                }
            }
        }
        
        private IEnumerator ProjectileLock(float attackRate)
        {
            yield return new WaitForSeconds(attackRate);
            projectileFireRateLock = false;
        }

        void ShootProjectile(MoveDirection moveDirection)
        {
            Debug.Log($"shoot projectile in {moveDirection}");
            var characterControllerPosition = rigidBody2D.position;
            _velocitizedSpeed = _speed * velocity;
            
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

            Debug.Log($"Velocity: {velocity}");
            Debug.Log($"x: {_velocitizedSpeed} @ {_acceleration}, V: {velocity}, Time {Time.deltaTime}");
        }
    }
    
}