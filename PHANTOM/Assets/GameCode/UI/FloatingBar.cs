using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FloatingBar:ISourcePoolObj
{
    public GameObject gameObject=> GO;
    private GameObject GO;
    private IBaseAttr baseAttr;
    private Image image;
    Camera UICamera;
    Transform FollowTarget;
    
    public void Awake(GameObject gameObject)
    {
        GO = gameObject;
        UICamera = Camera.main;
        image = Tool.GetUIComponent<Image>(GO, "fill");
    }
    // Update is called once per frame
    public void Update()
    {
        GO.transform.position = UICamera.WorldToScreenPoint(FollowTarget.position);
        image.fillAmount = (float)baseAttr.HP / baseAttr.MaxHP;
        //HP>0時繼續執行，CheckHP()回傳true，HP<0則回收物件
        if (!CheckHP())
        {
            OnDisable();
            return;
        }

        if (image.fillAmount > 0.999f) GO.SetActive(false);
        else GO.SetActive(true);
    }

    public void SetFollowTarget(Transform T,IBaseAttr BaseAttr,Color? color=null)
    {
        FollowTarget = T;
        baseAttr = BaseAttr;
        image.color = color??Color.red;
    }

    bool CheckHP()
    {
        return baseAttr.HP > 0.0001f;
    }

    private void OnDisable()
    {
        GO.SetActive(false);
        AllSourcePool.RecycleFloatingBar(this);
    }




}
