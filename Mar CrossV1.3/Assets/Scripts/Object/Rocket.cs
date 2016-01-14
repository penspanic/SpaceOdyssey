using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    public PlanetBase mainPlanet;
    public PlanetBase emigrationTarget;

    UIManager uiMgr;
    Player player;
    void Awake()
    {
        uiMgr = GameObject.FindObjectOfType<UIManager>();
        player = GameObject.FindObjectOfType<Player>();
    }

	void Update () 
    {
       
	}
    
    public void ChangePlanet()
    {

    }
    public void Emigration(PlanetBase target)
    {
        gameObject.SetActive(true);
        transform.localScale = new Vector3(1, 1, 1)*(1f/transform.parent.localScale.x);
        emigrationTarget = target;
        uiMgr.isEmigrationEnd = false;
        StartCoroutine(CoroutineUtil.LerpMove(this.gameObject, this.gameObject.transform.position,
            target.transform.position, 1, true, false, this.gameObject, "MoveEnd"));
        Vector3 t = target.transform.position - transform.position;
        float angle = Mathf.Rad2Deg*Mathf.Atan2(t.y, t.x);
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, 0, -90+angle));
    }
    public void MoveEnd()
    {
        this.transform.SetParent(emigrationTarget.transform, false);
        transform.localPosition = Vector3.zero;
        uiMgr.isEmigrationEnd = true;
        player.Emigration(emigrationTarget);
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.SetActive(false);
    }
}
