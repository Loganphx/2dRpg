using UnityEngine;

namespace Items
{
    public class CharacterItem : MonoBehaviour
    {
        public int Id;
        public string Name;
        
        public float Damage;
        public float AddedProjectiles;
        public float AttackSpeed;

        public float Health;

        public float MoveSpeed;

        public CharacterItem CreateCharacterItem(int id, string name, float damage, float addedProjeciles, float attackSpeed,
            float health, float moveSpeed)
        {
            Id = id;
            Name = name;
            Damage = damage;
            AddedProjectiles = addedProjeciles;
            AttackSpeed = attackSpeed;
            Health = health;
            MoveSpeed = moveSpeed;
            return this;
        }
    }
}