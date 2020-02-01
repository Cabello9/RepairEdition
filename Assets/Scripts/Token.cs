using DG.Tweening;
using UnityEngine;

public class Token : MonoBehaviour
{
    public bool isPlayerOne;
    public Cell cell;

    public void MoveToPosition(Vector3 position, float duration)
    {
        transform.DOMove(position, duration);
    }

    public void LookAt(Vector3 target, float duration)
    {
        transform.DOLookAt(target, duration);
    }

    public void JumpToPosition(Vector3 position, float duration)
    {
        transform.DOJump(position,1f,1, duration);
    }

    public void CrushYAxis(float finalY,float duration)
    {
        //Los tokens tienen un padre para que solo se escalen en una sola dirección
        transform.parent.transform.DOScaleY(finalY, duration);
    }
}
