using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Field
{
    public float X { get; set; }
    public float Y { get; set; }
    public int[] Size { get; set; }
    public List<Tree> Tree { get; set; }
    public List<Wall> Wall { get; set; }
    public List<Bullet> Bullet { get; set; }
    public Dictionary<int, Item> Item { get; set; }
    public Dictionary<int, Player> Player { get; set; }
    public Circle circle { get; set; }

    public List<string> Colors { get; set; }
    public TimeSpan time { get; set; }

    public DateTime inpulse { get; set; }
}
public class Circle
{
    public float X { get; set; }
    public float Y { get; set; }
    public double[] Size { get; set; }
    public float Radius { get; set; }
    public int StartTime { get; set; }
    public float a { get; set; }
    public float b { get; set; }
    public float endX { get; set; }
    public float endY { get; set; }
    public bool go { get; set; }
}
public class Bullet
{
    public int ID { get; set; }
    public string Weapon { get; set; }

    public float X { get; set; }
    public float Y { get; set; }
    public float a { get; set; }
    public float b { get; set; }
    public DateTime time { get; set; }

    public float[] EndPos { get; set; }
    public float[] StartPos { get; set; }

    public string Color { get; set; }

}
public class Wall
{
    public float X { get; set; }
    public float Y { get; set; }
    public int[] Size { get; set; }

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
    public bool Me { get; set; }
    public float[] Start { get; set; }
    public float[] End { get; set; }
    public bool Death { get; set; }
    public Weapons Weap { get; set; }
    public Pistol P { get; set; }
    public Shotgun S { get; set; }
    public Gun G { get; set; }
    public Bomb B { get; set; }

    public string Color { get; set; }

}
public class Weapons
{
    public int CountBullets { get; set; }//количество пуль за один выстрел
    public int TakenLives { get; set; }//количество отнятых жизней за попадание
    public int CountMagazine { get; set; }
    public int MaxCountMag { get; set; }
    public bool CamShot { get; set; }
    public DateTime time { get; set; }
}

public class Bomb : Weapons
{
    public Bomb()
    {
        this.CountBullets = 4;
        this.TakenLives = 5;
        this.CountMagazine = 3;
        this.MaxCountMag = 4;
        this.CamShot = true;
        this.time = new DateTime();
        time = DateTime.Now;

    }
}
public class Gun : Weapons
{
    public Gun()
    {
        this.CountBullets = 30;
        this.TakenLives = 3;
        this.CountMagazine = 0;
        this.MaxCountMag = 30;
        this.CamShot = true;
        this.time = new DateTime();
        time = DateTime.Now;
    }
}
public class Shotgun : Weapons
{
    public Shotgun()
    {
        this.CountBullets = 7;
        this.TakenLives = 4;
        this.CountMagazine = 0;
        this.MaxCountMag = 7;
        this.CamShot = true;
        this.time = new DateTime();
        time = DateTime.Now;
    }
}
public class Pistol : Weapons
{
    public Pistol()
    {
        this.CountBullets = 5;
        this.TakenLives = 2;
        this.CountMagazine = 24;
        this.MaxCountMag = 12;
        this.CamShot = true;
        this.time = new DateTime();
        time = DateTime.Now;
    }
}