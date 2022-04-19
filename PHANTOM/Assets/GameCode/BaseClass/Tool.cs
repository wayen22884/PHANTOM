
using System.Drawing;
using UnityEngine;
using UnityEngine.Networking;

public static class Tool 
{
    public static GameObject FindChildGameObject(GameObject Root,string GameObjectName)
    {
        if (Root==null) { Debug.LogError("there is no Root");return null; }

        if (Root.name == GameObjectName) return Root;
        Transform Result = null;
        Transform[] _childs=  Root.transform.GetComponentsInChildren<Transform>(true);
        foreach (var child in _childs)
        {
            if (child.name == GameObjectName)
            {
                if (Result == null)
                {
                    Result = child;
                }
                else Debug.LogError("there is more than one gameobject has the same name.");
            }  
        }
        if (Result == null) Debug.LogError($"GameObject {Root} don't have a ChildGameObject named {GameObjectName}");
        return Result?.gameObject;
    }

    public static T GetUIComponent<T>(GameObject Root,string UIName) where T:UnityEngine.Component
    {
        GameObject _childGameObject = FindChildGameObject(Root, UIName);
        if (_childGameObject == null) return null;

        T Result = _childGameObject.GetComponent<T>();
        if (Result == null) { Debug.LogError($"{UIName} don't has component in type {typeof(T)}.");return null; }
        return Result;
    }


    public static bool IsUpdateTime(ref float TimeCheckclock, float TimeCheckInterval)
    {
        if (TimeCheckclock < TimeCheckInterval) { TimeCheckclock += Time.deltaTime; return false; }
        else { TimeCheckclock = 0; return true; }
    }
    
    
    public static bool Between(float num, float a, float b)
    {
        if (a == b) return num == a;
        float min = a > b ? b : a;
        float max = a > b ? a : b;
        return (num >= min && num <= max);
    }


    public static bool GetIntersection(RectAngle firsRectAngle,RectAngle secondRectAngle)
    {
        return GetLineCross(firsRectAngle.point1,firsRectAngle.point2, secondRectAngle)
        ||GetLineCross(firsRectAngle.point2,firsRectAngle.point3, secondRectAngle)
        ||GetLineCross(firsRectAngle.point3,firsRectAngle.point4, secondRectAngle)
        ||GetLineCross(firsRectAngle.point4, firsRectAngle.point1, secondRectAngle);
    }

    private static bool GetLineCross(Vector2 point1,Vector2 point2, RectAngle secondRectAngle)
    {
        return GetIntersection(point1, point2, secondRectAngle.point1, secondRectAngle.point2)
        ||GetIntersection(point1, point2, secondRectAngle.point2, secondRectAngle.point3)
        ||GetIntersection(point1, point2, secondRectAngle.point3, secondRectAngle.point4)
        ||GetIntersection(point1, point2, secondRectAngle.point4, secondRectAngle.point1);
    }

    /// <summary>
    /// 計算兩條直線的交點(只在點之間)
    /// </summary>
    /// <param name="lineFirstStar">L1的點1坐標</param>
    /// <param name="lineFirstEnd">L1的點2坐標</param>
    /// <param name="lineSecondStar">L2的點1坐標</param>
    /// <param name="lineSecondEnd">L2的點2坐標</param>
    /// <returns></returns>
    public static bool GetIntersection(Vector2 lineFirstStar, Vector2 lineFirstEnd, Vector2 lineSecondStar, Vector2 lineSecondEnd)
    {
        var intersection = GetIntersection(Transform(lineFirstStar), Transform(lineFirstEnd), Transform(lineSecondStar), Transform(lineSecondEnd));
        var IntersectionVector2= new Vector2(intersection.X, intersection.Y);
 
        return IsOnLine(lineFirstStar, lineFirstEnd, IntersectionVector2) && IsOnLine(lineSecondStar, lineSecondEnd, IntersectionVector2); 
    }

    public static bool IsOnLine(Vector2 lineFirstStar, Vector2 lineFirstEnd, Vector2 IntersectionVector2)
    {
        var line1 = Vector2.Distance(IntersectionVector2, lineFirstStar);
        var line2 = Vector2.Distance(IntersectionVector2, lineFirstEnd);
        var line3 = Vector2.Distance(lineFirstStar, lineFirstEnd);
        var distance=line1 +
            line2 - line3;

        var isOnLine = distance < 0.001;

        return isOnLine;
    }


    private static PointF Transform(Vector2 vector2)
    {
        return new PointF(vector2.x, vector2.y);
    }
    
    /// <summary>
    /// 計算兩條直線的交點
    /// </summary>
    /// <param name="lineFirstStar">L1的點1坐標</param>
    /// <param name="lineFirstEnd">L1的點2坐標</param>
    /// <param name="lineSecondStar">L2的點1坐標</param>
    /// <param name="lineSecondEnd">L2的點2坐標</param>
    /// <returns></returns>
    private static PointF GetIntersection(PointF lineFirstStar, PointF lineFirstEnd, PointF lineSecondStar, PointF lineSecondEnd)
        {
            /*
             * L1，L2都存在斜率的情況：
             * 直線方程L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 ) 
             * => y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
             * 令 a = ( y2 - y1 ) / ( x2 - x1 )
             * 有 y = a * x - a * x1 + y1   .........1
             * 直線方程L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
             * 令 b = ( y4 - y3 ) / ( x4 - x3 )
             * 有 y = b * x - b * x3 + y3 ..........2
             * 
             * 如果 a = b，則兩直線平等，否則， 聯解方程 1,2，得:
             * x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
             * y = a * x - a * x1 + y1
             * 
             * L1存在斜率, L2平行Y軸的情況：
             * x = x3
             * y = a * x3 - a * x1 + y1
             * 
             * L1 平行Y軸，L2存在斜率的情況：
             * x = x1
             * y = b * x - b * x3 + y3
             * 
             * L1與L2都平行Y軸的情況：
             * 如果 x1 = x3，那麼L1與L2重合，否則平等
             * 
            */
            float a = 0, b = 0;
            int state = 0;
            if (lineFirstStar.X != lineFirstEnd.X)
            {
                a = (lineFirstEnd.Y - lineFirstStar.Y) / (lineFirstEnd.X - lineFirstStar.X);
                state |= 1;
            }
            if (lineSecondStar.X != lineSecondEnd.X)
            {
                b = (lineSecondEnd.Y - lineSecondStar.Y) / (lineSecondEnd.X - lineSecondStar.X);
                state |= 2;
            }
            switch (state)
            {
                case 0: //L1與L2都平行Y軸
                    {
                        if (lineFirstStar.X == lineSecondStar.X)
                        {
                            //throw new Exception("兩條直線互相重合，且平行於Y軸，無法計算交點。");
                            return new PointF(0, 0);
                        }
                        else
                        {
                            //throw new Exception("兩條直線互相平行，且平行於Y軸，無法計算交點。");
                            return new PointF(0, 0);
                        }
                    }
                case 1: //L1存在斜率, L2平行Y軸
                    {
                        float x = lineSecondStar.X;
                        float y = (lineFirstStar.X - x) * (-a) + lineFirstStar.Y;
                        return new PointF(x, y);
                    }
                case 2: //L1 平行Y軸，L2存在斜率
                    {
                        float x = lineFirstStar.X;
                        //網上有相似代碼的，這一處是錯誤的。你可以對比case 1 的邏輯 進行分析
                            //源code:lineSecondStar * x + lineSecondStar * lineSecondStar.X + p3.Y;
                        float y = (lineSecondStar.X - x) * (-b) + lineSecondStar.Y;
                        return new PointF(x, y);
                    }
                case 3: //L1，L2都存在斜率
                    {
                        if (a == b)
                        {
                            // throw new Exception("兩條直線平行或重合，無法計算交點。");
                            return new PointF(0, 0);
                        }
                        float x = (a * lineFirstStar.X - b * lineSecondStar.X - lineFirstStar.Y + lineSecondStar.Y) / (a - b);
                        float y = a * x - a * lineFirstStar.X + lineFirstStar.Y;
                        return new PointF(x, y);
                    }
            }
            // throw new Exception("不可能發生的情況");
            return new PointF(0, 0);
        }
}
public struct RectAngle
{
    
    public Vector2 point1 { get; }
    public Vector2 point2 { get; }
    public Vector2 point3 { get; }
    public Vector2 point4 { get; }

    public RectAngle(Vector2 center,Vector2 size)
    {
        point1 = center+new Vector2(size.x/2,size.y/2);
        point2 = center+new Vector2(-size.x/2,-size.y/2);
        point3 = center+new Vector2(size.x/2,-size.y/2);
        point4 = center+new Vector2(-size.x/2,size.y/2);
    }
    
}
