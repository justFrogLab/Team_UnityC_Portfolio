using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class invenItemMove : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    Transform _targetTr; // 이동될 UI

    [SerializeField]    // 인벤토리 Rect
    RectTransform _inventotyRectransform;

    [Tooltip("아이템 버리기 갯수 질문 창")]
    [SerializeField]
    Transform _dropItemUI;

    Rect _inventoryRect;

    Vector2 _beginPoint;
    Vector2 _moveBegin;

    void Awake()
    {
        _inventoryRect = _inventotyRectransform.rect;       // 인벤토리 rect

        // 이동 대상 UI를 지정하지 않은 경우, 자동으로 부모로 초기화
        if (_targetTr == null)
            _targetTr = transform;
    }

    // 드래그 시작 위치 지정
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _targetTr.position;
        _moveBegin = eventData.position;
    }

    // 드래그 : 마우스 커서 위치로 이동
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _targetTr.position = _beginPoint + (eventData.position - _moveBegin);
    }

    // 드래그 종료시
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 endPos = _targetTr.localPosition;

        if (endPos.x > _inventoryRect.xMax || endPos.x < _inventoryRect.xMin ||
            endPos.y > _inventoryRect.yMax || endPos.y < _inventoryRect.yMin)
        {
            itemDrop();
        }


        else
        {
            Debug.Log("범위 안");
            _targetTr.position = _beginPoint;
        }
        

    }

    public void itemDrop()
    {
        Debug.Log("범위 밖");
        _targetTr.position = _beginPoint;
        int idx = transform.GetSiblingIndex();
    }
}
