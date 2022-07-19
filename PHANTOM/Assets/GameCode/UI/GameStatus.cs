using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : IUserInterface
{

    private GameObject heartVolume = null;
    private List<GameObject> _bullets = null;
    private GameObject _bulletVolumeHint = null;
    private Slider _SpBar = null;
    private Slider _ExtraBar = null;
    private Text _score = null;
    private int score;

    public override void Initialize()
    {
        _RootUI = Tool.FindChildGameObject(GameResource.Canvas, "GameStatus");
        heartVolume = Tool.FindChildGameObject(_RootUI, "HeartVolume");


        GameObject heartLeft = Tool.FindChildGameObject(heartVolume, "heartLeft");
        GameObject heartRight = Tool.FindChildGameObject(heartVolume, "heartRight");
        float width = heartLeft.GetComponent<RectTransform>().sizeDelta.x * heartLeft.transform.localScale.x;
        _bullets = new List<GameObject>();
        _bullets.Add(heartLeft);
        _bullets.Add(heartRight);


        for (int i = 1; i < 3; i++)
        {
            GameObject leftHeart = GameObject.Instantiate(_bullets[0], heartVolume.transform);
            int lastIndex = _bullets.Count - 1;
            (leftHeart.transform as RectTransform).anchoredPosition = (_bullets[lastIndex].transform as RectTransform).anchoredPosition + new Vector2(width, 0f);
            _bullets.Add(leftHeart);
            
            
            GameObject leftRight = GameObject.Instantiate(_bullets[1], heartVolume.transform);
            lastIndex = _bullets.Count - 1;
            (leftRight.transform as RectTransform).anchoredPosition = (_bullets[lastIndex].transform as RectTransform).anchoredPosition + new Vector2(width, 0f);
            _bullets.Add(leftRight);
        }

        AllSourcePool.PlayerCharacter.ChangeHP+=UpdateBulletVolume;

    }


    public void UpdateBulletVolume(int value)
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (i < value) _bullets[i].SetActive(true);
            else _bullets[i].SetActive(false);
        }
    }
    public void Update(ValueType valueType, float value)
    {
        if (valueType == ValueType.SP) _SpBar.value = value;
        else if (valueType == ValueType.EP) _ExtraBar.value = value;
        else if (valueType == ValueType.Score) _score.text = $"Score:{value}";
    }
}