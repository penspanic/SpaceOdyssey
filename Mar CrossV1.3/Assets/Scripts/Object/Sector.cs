using UnityEngine;
using System.Collections;

public class Sector : MonoBehaviour {

    float _targetPos;

    public SectorTile nowSectorTile;

    public void SetSectorTile(SectorTile st)
    {
        nowSectorTile = st;
        nowSectorTile.onSector = this;
        if (transform.childCount == 1)
        {
            transform.GetChild(0).GetComponent<PlanetBase>().targetPos = nowSectorTile.thirdSeat;

            transform.GetChild(0).GetComponent<PlanetBase>().isMove = false;
        }
        else if (transform.childCount == 2)
        {
            transform.GetChild(0).GetComponent<PlanetBase>().targetPos = nowSectorTile.secondSeat;
            transform.GetChild(1).GetComponent<PlanetBase>().targetPos = nowSectorTile.fourthSeat;

            transform.GetChild(0).GetComponent<PlanetBase>().isMove = false;
            transform.GetChild(1).GetComponent<PlanetBase>().isMove = false;
        }
        else
        {
            transform.GetChild(0).GetComponent<PlanetBase>().targetPos = nowSectorTile.firstSeat;
            transform.GetChild(1).GetComponent<PlanetBase>().targetPos = nowSectorTile.thirdSeat;
            transform.GetChild(2).GetComponent<PlanetBase>().targetPos = nowSectorTile.fifthSeat;

            transform.GetChild(0).GetComponent<PlanetBase>().isMove = false;
            transform.GetChild(1).GetComponent<PlanetBase>().isMove = false;
            transform.GetChild(2).GetComponent<PlanetBase>().isMove = false;
        }

        //StartCoroutine(CoroutineUtil.LerpMove(this.gameObject, this.transform.position, new Vector2(nowSectorTile.x, transform.position.y), 2));
    }
}
