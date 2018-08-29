using UnityEngine;
using System.Collections;

public class SpecialPlanet : PlanetBase
{
    protected override void Awake()
    {
        base.Awake();
        //GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = PlanetBase.q_planet;

        isSpecial = true;
        spRenderer.enabled = false;
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Explore()
    {
        isExplored = true;
        spRenderer.enabled = true;
        transform.Find("Hide Planet").GetComponent<SpriteRenderer>().enabled = false;
    }
}
