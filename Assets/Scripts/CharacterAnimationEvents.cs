using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
        character.target.GetComponentInParent<Character>().SetState(Character.State.Death);
    }

    void ShootEnd()
    {
        character.SetState(Character.State.Idle);
        character.target.GetComponentInParent<Character>().SetState(Character.State.Death);
    }
}
