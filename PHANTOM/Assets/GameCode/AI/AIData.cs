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
            if (_character.ID == CharacterID.Enemy) return 1.5f;
            else { Debug.LogError("there is no right characterID."); return 0; }
        }
    }
    public float AttackDistance
    {
        get
        {
            if (_character.ID == CharacterID.Enemy) return 1f+(Random.Range(-10,10)*0.04f);
            else { Debug.LogError("there is no right characterID."); return 0; }
        }
    }

}
