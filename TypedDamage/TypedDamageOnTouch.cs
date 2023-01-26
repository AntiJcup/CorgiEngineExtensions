using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace CorgiEngineExtensions
{
    public class TypedDamageOnTouch : DamageOnTouch
    {
        [Header("Damage type")] public DamageType DamageType;
        protected override void OnCollideWithDamageable(Health health)
        {
            // if what we're colliding with is a CorgiController, we apply a knockback force
            _colliderCorgiController = health.gameObject.MMGetComponentNoAlloc<CorgiController>();


            float randomDamage = UnityEngine.Random.Range(MinDamageCaused, Mathf.Max(MaxDamageCaused, MinDamageCaused));
            ApplyDamageCausedKnockback(randomDamage, TypedDamages);

            OnHitDamageable?.Invoke(health);

            HitDamageableFeedback?.PlayFeedbacks(this.transform.position);

            if ((FreezeFramesOnHitDuration > 0) && (Time.timeScale > 0))
            {
                MMFreezeFrameEvent.Trigger(Mathf.Abs(FreezeFramesOnHitDuration));
            }
            
            // we apply the damage to the thing we've collided with
            var typedHealth = _colliderHealth as TypedHealth;
            if (typedHealth != null)
                typedHealth.Damage((int)randomDamage, gameObject, InvincibilityDuration, InvincibilityDuration, _damageDirection, DamageType);
            else
                _colliderHealth.Damage(randomDamage, gameObject, InvincibilityDuration, InvincibilityDuration, _damageDirection);
            
            if (_colliderHealth.CurrentHealth <= 0)
            {
                OnKill?.Invoke();
            }
            
            SelfDamage(DamageTakenEveryTime + DamageTakenDamageable);
        }
    }
}
