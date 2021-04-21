using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData 
{
    public readonly Vector2 targetPos;
    public readonly PlayerShooting playerShooting = null;

    public BulletData(Vector2 targetPos, PlayerShooting playerShooting)
    {
        this.targetPos = targetPos;
        this.playerShooting = playerShooting;
    }
}
