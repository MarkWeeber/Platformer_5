using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{
    public class Health : MonoBehaviour
    {
        public float healthPoints = 100f;
        [SerializeField] private float deathTime = 1.5f;
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float staggerDamage = 20f;
        [SerializeField] private float staggerAccumulateTimer = 2f;
        [SerializeField] private UIHealthBar uIHealthBar = null;
        [SerializeField] private CanvasRenderer pauseMenu = null;
        [SerializeField] private Transform healthBar = null;
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Color[] takeDamageColors = null;
        [SerializeField] private Color[] healUpColors = null;
        [SerializeField] private GameManager gameManager = null;
        private EnemySimpleAI enemySimpleAI = null;
        private float healthBarX = 0f;
        private float staggerDamageAccumulation = 0f;
        private float staggerDamageResetTimer = 0f;
        private Animator animator = null;
        private PlayerInput playerInput = null;
        private bool isAlive = true;
        private int animateDamage = 0;
        private int animateHeal = 0;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            enemySimpleAI = GetComponent<EnemySimpleAI>();
            if(healthBar != null)
            {
                healthBarX = healthBar.localScale.x;
            }
        }

        public float HealthPoints
        {
            get {return healthPoints;}
            set {ModifyHealth(value);}
        }

        public void ModifyHealth(float delta)
        {
            healthPoints += delta;
            staggerDamageAccumulation += delta;
            staggerDamageResetTimer = staggerAccumulateTimer;
            // damage counter
            DamagePopUp.Create(this.transform.position, delta);
            //GameAssets.instance.PopUpDamageCounter(this.transform.position, delta);
            if(healthPoints > 0 && isAlive)
            {
                if (delta < 0 && enemySimpleAI != null)
                {
                    enemySimpleAI.OnTakeDamage((Mathf.Abs(staggerDamageAccumulation) > staggerDamage));
                }
            }
            if(Mathf.Abs(staggerDamageAccumulation) > staggerDamage)
            {
                staggerDamageAccumulation = 0f;
                staggerDamageResetTimer = 0f;
            }
            if(healthPoints < 0 && isAlive)
            {
                OnDeath();
            }
            if(uIHealthBar != null)
            {
                uIHealthBar.ModifyPercent( (healthPoints / maxHealth) * 100);
            }
            if(healthBar != null)
            {
                //setWidth = ( percentValue / 100 ) * initialWidth;
                healthBar.localScale = new Vector3(healthBarX * ( Mathf.Clamp(healthPoints / maxHealth, 0, 100)), healthBar.localScale.y, healthBar.localScale.z);
            }
        }

        public void OnDeath()
        {
            isAlive = false;
            Destroy(this.transform.parent.gameObject, deathTime);
            if(animator != null)
            {
                animator.SetTrigger("Death");
            }
            if(playerInput != null)
            {
                playerInput.enabled = false;
            }
            if(uIHealthBar != null && pauseMenu != null)
            {
                uIHealthBar.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(true);
            }
            if(gameManager != null)
            {
                gameManager.GameOver();
            }
            if(enemySimpleAI != null)
            {
                enemySimpleAI.OnDeath();
            }
        }

        public void TakeDamage()
        {
            animateDamage = takeDamageColors.Length;
        }

        public void HealUp()
        {
            animateHeal = healUpColors.Length;
        }

        private void FixedUpdate()
        {
            // animating damage taken
            if(animateDamage > 0)
            {
                spriteRenderer.material.color = takeDamageColors[takeDamageColors.Length - animateDamage];
                animateDamage--;
            }

            // animate heal up
            if(animateHeal > 0)
            {
                spriteRenderer.material.color = healUpColors[healUpColors.Length - animateHeal];
                animateHeal--;
            }
        }

        private void Update()
        {
            if(staggerDamageResetTimer > 0f)
            {
                staggerDamageResetTimer -= Time.deltaTime;
            }
            if(staggerDamageResetTimer < 0f)
            {
                staggerDamageAccumulation = 0f;
            }
        }
    }
}