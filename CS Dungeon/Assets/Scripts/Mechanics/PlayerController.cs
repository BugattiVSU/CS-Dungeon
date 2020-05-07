using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public AudioSource boom;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        //Bullet
        public GameObject BulletToRight, BulletToLeft, BulletToUp, BulletToBot;
        public GameObject BoomToRight, BoomToLeft, BoomToUp, BoomToBot;
        //public GameObject boomEffect;
        Vector2 bulletPos;
        Vector2 boomPos;
        public float fireRate = 0.7f;
        public float nextFire = 0.0f;
        public Transform firePoint;
        //public int power;
        //public int maxPower;
        //public int score;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            boom = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {

            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();

            if (Input.GetKey(KeyCode.Z) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Fire();
                
                audioSource.Play();
            }

            if (Input.GetKey(KeyCode.X) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                FireBoom();
                boom.Play();


            }
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
            
    }
       
        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;
  
            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            targetVelocity = move * maxSpeed;
        }

        void Fire()
        {
            bulletPos = transform.position;
            float ver = Input.GetAxis("Vertical");
            float hor = Input.GetAxis("Horizontal");

            if (hor == 0 && ver == 0)
            {
                if (spriteRenderer.flipX == false)
                {
                    bulletPos += new Vector2(+1f, 0.00f);

                    Instantiate(BulletToRight, bulletPos, Quaternion.identity);
                }
                if (spriteRenderer.flipX == true)
                {
                    bulletPos += new Vector2(-1f, 0.00f);
                    Instantiate(BulletToLeft, bulletPos, Quaternion.identity);
                }
            }

            if (hor > 0)
            {
                bulletPos += new Vector2(+1f, 0.00f);
                Instantiate(BulletToRight, bulletPos, Quaternion.identity);
            }      
            
            if (hor < 0)
            {
                bulletPos += new Vector2(-1f, 0.00f);               
                Instantiate(BulletToLeft, bulletPos, Quaternion.identity);
                
            }

            
            if (ver < 0)
            {
                bulletPos += new Vector2(0.00f, -1f);
                Instantiate(BulletToBot, bulletPos, Quaternion.identity);
            }
            
            if (ver > 0)
            {
                bulletPos += new Vector2(0.00f, 1f);

                Instantiate(BulletToUp, bulletPos, Quaternion.identity);

            }
        }

        void FireBoom()
        {
            boomPos = transform.position;
            float ver = Input.GetAxis("Vertical");
            float hor = Input.GetAxis("Horizontal");

            if (hor == 0 && ver == 0)
            {
                if (spriteRenderer.flipX == false)
                {
                    boomPos += new Vector2(+1f, 0.00f);

                    Instantiate(BoomToRight, boomPos, Quaternion.identity);
                }
                if (spriteRenderer.flipX == true)
                {
                    boomPos += new Vector2(-1f, 0.00f);
                    Instantiate(BoomToLeft, boomPos, Quaternion.identity);
                }
            }

            if (hor > 0)
            {
                boomPos += new Vector2(+1f, 0.00f);
                Instantiate(BoomToRight, boomPos, Quaternion.identity);
            }

            if (hor < 0)
            {
                boomPos += new Vector2(-1f, 0.00f);
                Instantiate(BoomToLeft, boomPos, Quaternion.identity);

            }


            if (ver < 0)
            {
                boomPos += new Vector2(0.00f, -1f);
                Instantiate(BoomToBot, boomPos, Quaternion.identity);
            }

            if (ver > 0)
            {
                boomPos += new Vector2(0.00f, 1f);

                Instantiate(BoomToUp, boomPos, Quaternion.identity);

            }
        }


        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}
