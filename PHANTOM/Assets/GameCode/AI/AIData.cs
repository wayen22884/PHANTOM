using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData
{
    public AIData(EnemyCharacter enemyCharacter, Transform Enemy, PlayerCharacter Player)
    {
        _character = enemyCharacter;
        _transform = Enemy;
        _player = Player;
    }
    private EnemyCharacter _character;
    private Transform _transform;
    private PlayerCharacter _player;
    private FSMSystem _system;
    public void SetFSMSystem(FSMSystem System) { _system = System; }
    public PlayerCharacter player => _player;
    public ICharacterAttr Attr => _character.Attr;
    public float Distance => Mathf.Abs(VectorDistance);
    public float VectorDistance => _player.Transform.position.x - _transform.position.x;
    public float EnemyPositionX => Mathf.Abs(_transform.position.x);
    public Transform Transform => _transform;
    public EnemyCharacter Character => _character;


    public float ChaseDistance
    {
        get
        {
            if (_character.ID == CharacterID.Enemy) return 6f;
            if (_character.ID == CharacterID.Rifle) return 7f;
            if (_character.ID == CharacterID.ShootGun) return 6f;
            else { Debug.LogError("there is no right characterID."); return 0; }
        }
    }
    public float AttackDistance
    {
        get
        {
            if (_character.ID == CharacterID.Enemy) return 4f;
            if (_character.ID == CharacterID.Rifle) return 5.5f;
            if (_character.ID == CharacterID.ShootGun) return 4f;
            else { Debug.LogError("there is no right characterID."); return 0; }
        }
    }

}
