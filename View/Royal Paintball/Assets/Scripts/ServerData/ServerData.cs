using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Field
{
    public List<Tree> Tree { get; set; }
    public List<Wall> Wall { get; set; }
    public List<Bullet> Bullet { get; set; }
    public List<Item> Item { get; set; }
    public Dictionary<int, Player> Player { get; set; }
    public Circle circle { get; set; }
    public TimeSpan time;
}
public class Circle
{
    public float X { get; set; }
    public float Y { get; set; }
    public int[] Size { get; set; }

    public Circle()
    {
        X = 0;
        Y = 0;
        Size = new int[] { 20, 20 };
    }
}
public class Bullet
{
    public int ID { get; set; }
    public string Weapon { get; set; }
    public float[] EndPos { get; set; }
    public float[] StartPos { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float a { get; set; }
    public float b { get; set; }
    public GameObject bul { get; set; }
}
public class Wall
{
    public float X { get; set; }
    public float Y { get; set; }

}
public class Item
{
    public string Name { get; set; }
    public int Count { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public int Index { get; set; }
}
public class Tree
{
    public float X { get; set; }
    public float Y { get; set; }
    public int[] Size { get; set; }
    public string Type;

}
public class Player
{
    public int Life { get; set; }
    public int ID { get; set; }
    public string Direction { get; set; }
    public string Weapon { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float XRot { get; set; }
    public float YRot { get; set; }
    public bool Shoot { get; set; }
    public bool Reload { get; set; }
    public bool LiftItem { get; set; }
    public float[] MousePos { get; set; }
    public int[] Size { get; set; }

    public float[] Start { get; set; }
    public float[] End { get; set; }

    public Weapons Weap { get; set; }
    public Pistol P { get; set; }
    public Shotgun S { get; set; }
    public Gun G { get; set; }
    public Bomb B { get; set; }

    public string Color { get; set; }

}
public class Weapons
{
    public double Direction { get; set; }//направление полета пули(местонахождение мыши)
    public int CountBullets { get; set; }//количество пуль за один выстрел
    public int Power { get; set; }//сила удара пули
    public double FlightTime { get; set; }//время полета пули
    public double NextShootTime { get; set; }//время полета пули
    public double RechargeTime { get; set; }//время для перезарядки оружия
    public int InityalCountBul { get; set; }//начальное количество пуль
    public int Index { get; set; }//индекс оружия
    public int TakenLives { get; set; }//количество отнятых жизней за попадание
    public int CountMagazine { get; set; }
    public int MaxCountMag { get; set; }
}

public class Bomb : Weapons
{
    public Bomb()
    {
        this.CountBullets = 4;
        this.Direction = 1;//направление мыши
        this.Power = 4;
        this.FlightTime = 2;
        this.RechargeTime = 0.1;
        this.InityalCountBul = 5;
        this.Index = 4;
        this.TakenLives = 5;
        this.CountMagazine = 3;
        this.MaxCountMag = 4;

    }
}
public class Gun : Weapons
{
    public Gun()
    {
        this.CountBullets = 30;
        this.Direction = 1;//направление мыши
        this.Power = 2;
        this.FlightTime = 0.8;
        this.RechargeTime = 0.5;
        this.InityalCountBul = 10;
        this.Index = 3;
        this.TakenLives = 3;
        this.CountMagazine = 0;
        this.MaxCountMag = 30;
    }
}
public class Shotgun : Weapons
{
    public Shotgun()
    {

        this.CountBullets = 7;
        this.Direction = 1;//направление мыши
        this.Power = 3;
        this.FlightTime = 1;
        this.RechargeTime = 0.6;
        this.InityalCountBul = 20;
        this.Index = 2;
        this.TakenLives = 4;
        this.CountMagazine = 0;
        this.MaxCountMag = 7;
    }
}
public class Pistol : Weapons
{
    public Pistol()
    {
        this.CountBullets = 5;
        this.Power = 1;
        this.Direction = 1;//направление мыши
        this.FlightTime = 1.4;
        this.RechargeTime = 0.2;
        this.InityalCountBul = 13;
        this.Index = 1;
        this.TakenLives = 2;
        this.CountMagazine = 24;
        this.MaxCountMag = 12;
    }
}