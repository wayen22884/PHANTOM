using UnityEngine;

public abstract class IUserInterface 
{
    protected GameObject _RootUI;
    protected bool _bActive;
    public bool IsVisable()
    {
        return _bActive;
    }
    public  virtual void RootClick()
    {
        _bActive = !_bActive;
        _RootUI.SetActive(_bActive);
    }
    public virtual void Initialize() { }
}