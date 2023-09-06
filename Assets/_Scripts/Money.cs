using UnityEngine;

/// <summary>
/// ClickerMoney Script
/// Author: Sam Davidson
/// </summary>

// Скажешь, что взято у человека сверху

[System.Serializable]
public class Money
{
    private double _amount = 0d;

    public double Amount
    {
        get
        {
            return _amount;
        }
        set
        {
            _amount = value;
            Recalculate();
        }
    }

    public MoneyType Type { get; set; }

    public bool IsZero
    {
        get
        {
            return _amount == 0d && Type == MoneyType.Flat;
        }
    }

    public Money()
    {
        Amount = 0d;
        Type = MoneyType.Flat;
    }

    public Money(double amount, MoneyType type)
    {
        Amount = amount;
        Type = type;
    }

    public Money(double amount, int type)
    {
        _amount = amount;
        Type = (MoneyType)type;
    }

    private void Recalculate()
    {
        if (_amount == 0d) return;
        if (_amount >= 1000d)
        {
            if (Type == MoneyType.AD) return;
            _amount /= 1000d;
            Type = (MoneyType)((int)Type + 1);
            if (_amount >= 1000d) Recalculate();
        }
        else
        {          
            if (_amount < 1d)
            {
                if (Type == MoneyType.Flat) return;
                _amount *= 1000d;
                Type = (MoneyType)((int)Type - 1);
                if (_amount < 1d) Recalculate();
            }
        }
    }

    public static explicit operator double(Money m)
    {
        return m._amount * Mathf.Pow(1000, (int)m.Type);
    }

    public static Money operator +(Money a, Money b)
    {
        a.Amount += b._amount * Mathf.Pow(1000, (int)b.Type - (int)a.Type);
        return a;
    }

    public static Money operator -(Money a, Money b)
    {
        a.Amount -= b._amount * Mathf.Pow(1000, (int)b.Type - (int)a.Type);
        return a;
    }

    public static Money operator *(Money a, Money b)
    {
        a.Type = (int)a.Type + b.Type;
        a.Amount *= b.Amount;
        return a;
    }

    public static bool operator >=(Money a, Money b)
    {
        if ((int)a.Type > (int)b.Type) return true;
        if ((int)a.Type < (int)b.Type) return false;
        return a._amount >= b._amount;
    }

    public static bool operator <=(Money a, Money b)
    {
        if ((int)a.Type < (int)b.Type) return true;
        if ((int)a.Type > (int)b.Type) return false;
        return a._amount <= b._amount;
    }

    public static bool operator ==(Money a, Money b) => a.Equals(b);
    public static bool operator !=(Money a, Money b) => !a.Equals(b);

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is Money m)
        {
            return (_amount == m._amount && Type == m.Type);
        }
        return false;
    }

    public override string ToString()
    {
        if (Type == 0) return string.Format("{0:f2}", _amount);
        return string.Format("{0:f2}", _amount) + Type.ToString();
    }

    public override int GetHashCode()
    {
        return (int)(_amount * 100 * Mathf.Pow(1000, (int)Type));
    }
}
