using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : IUserInterface
{

    private GameObject _bulletVolume = null;
    private List<GameObject> _bullets = null;
    private GameObject _bulletVolumeHint = null;
    private Slider _SpBar = null;
    private Slider _ExtraBar = null;
    private Text _score = null;
    private int score;

    public override void Initialize()
    {
        _RootUI = Tool.FindChildGameObject(GameResource.Canvas, "GameStatus");
        _bulletVolume = Tool.FindChildGameObject(_RootUI, "BulletVolume");


        GameObject bullet = Tool.FindChildGameObject(_bulletVolume, "BulletImage");
        float width = bullet.GetComponent<RectTransform>().sizeDelta.x * bullet.transform.localScale.x;
        _bullets = new List<GameObject>();
        _bullets.Add(bullet);


        for (int i = 1; i < 10; i++)
        {
            GameObject GO = GameObject.Instantiate(_bullets[0], _bulletVolume.transform);
            int lastIndex = _bullets.Count - 1;
            (GO.transform as RectTransform).anchoredPosition = (_bullets[lastIndex].transform as RectTransform).anchoredPosition + new Vector2(width, 0f);
            _bullets.Add(GO);
        }




        _SpBar = Tool.GetUIComponent<Slider>(_RootUI, "SpBar");
        _ExtraBar = Tool.GetUIComponent<Slider>(_RootUI, "ExtralBar");
        _score = Tool.GetUIComponent<Text>(_RootUI, "Score");

        _bulletVolumeHint = GameObject.Find("ReloadHint");
        _bulletVolumeHint.SetActive(false);
    }


    public void UpdateBulletVolume(int value)
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (i < value) _bullets[i].SetActive(true);
            else _bullets[i].SetActive(false);
        }
        if (value == 0) _bulletVolumeHint.SetActive(true);
        else _bulletVolumeHint.SetActive(false);
    }
    public void Update(ValueType valueType, float value)
    {
        if (valueType == ValueType.SP) _SpBar.value = value;
        else if (valueType == ValueType.EP) _ExtraBar.value = value;
        else if (valueType == ValueType.Score) _score.text = $"Score:{value}";
    }
}