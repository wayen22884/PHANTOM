using System;

public delegate void ValueEventHandler(ValueEventArgs args);

public class ValueEventArgs
{
    public ValueEventArgs(ValueType Type,int Value) { _value = Value;_type = Type; }
    private readonly int _value;
    private readonly ValueType _type;
    public int Value => _value;
    public ValueType Type => _type;
}


