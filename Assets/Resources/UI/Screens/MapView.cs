using System;
using UnityEngine.UIElements;
using UnityEngine;

public class MapView : VisualElement {
    private VisualElement  mapContainer;
    private Image mapImage;

    private int zoomStep;
    private const int MaxZoomSteps = 5;
    private const float ZoomBase = 1.25f;
    
    private Vector2 dragStart;
    private Vector2 currentOffset = Vector2.zero;
    private bool isDragging;
    
    private float worldMinX = -2000f;
    private float worldMaxX = 2000f;
    private float worldMinZ = -2000f;
    private float worldMaxZ = 2000f;

    public MapView() {
        style.flexGrow = 1;
        style.overflow = Overflow.Hidden;

        // 컨테이너 생성
        mapContainer = new VisualElement {
            style = {
                position = Position.Absolute,
                left = 0,
                top = 0,
                width = Length.Percent(100),
                height = Length.Percent(100),
                transformOrigin = new TransformOrigin(0, 0)
            }
        };

        mapImage = new Image {
            image = Resources.Load<Texture2D>("Map/Terrain2"),
            scaleMode = ScaleMode.ScaleToFit ,
        };

        mapContainer.Add(mapImage);
        Add(mapContainer);

        RegisterCallback<PointerDownEvent>(OnPointerDown);
        RegisterCallback<PointerMoveEvent>(OnPointerMove);
        
        RegisterCallback<PointerUpEvent>(OnPointerUp);
        RegisterCallback<PointerLeaveEvent>(OnPointerUp);
        
        
        RegisterCallback<WheelEvent>(OnWheel);

        mapContainer.style.translate = new Translate(0, 0, 0);

    }
    
    private void OnPointerDown(PointerDownEvent evt) {
        isDragging = true;
        dragStart = evt.position;
    }

    private void OnPointerMove(PointerMoveEvent evt) {
        if (!isDragging) return;
        Vector2 delta = new Vector2(evt.position.x - dragStart.x, evt.position.y - dragStart.y);
        dragStart = evt.position;

        currentOffset += delta;
        mapContainer.style.translate = new Translate(currentOffset.x, currentOffset.y, 0);
    }


    private void OnWheel(WheelEvent evt) {
        int direction = evt.delta.y > 0 ? -1 : 1;
        int newStep = Mathf.Clamp(zoomStep + direction, -MaxZoomSteps, MaxZoomSteps);
        if (newStep == zoomStep) return;

        float prevScale = Mathf.Pow(ZoomBase, zoomStep);
        float newScale = Mathf.Pow(ZoomBase, newStep);

        Vector2 center = new Vector2(resolvedStyle.width, resolvedStyle.height) / 2f;
        Vector2 logicalCenter = (center - currentOffset) / prevScale;
        Vector2 newOffset = center - logicalCenter * newScale;

        currentOffset = newOffset;
        mapContainer.style.translate = new Translate(currentOffset.x, currentOffset.y, 0);
        mapContainer.transform.scale = new Vector3(newScale, newScale, 1);
        zoomStep = newStep;
    }
    
    private void OnPointerUp(EventBase evt) {
        isDragging = false;
    }
    
    public void SetWorldBounds(float minX, float maxX, float minZ, float maxZ) {
        worldMinX = minX;
        worldMaxX = maxX;
        worldMinZ = minZ;
        worldMaxZ = maxZ;
    }
    
    public void FocusOnWorldPosition(float unityX, float unityZ) {
        float imageWidth = mapImage.resolvedStyle.width;
        float imageHeight = mapImage.resolvedStyle.height;

        // 필드 범위
        float worldWidth = worldMaxX - worldMinX;
        float worldHeight = worldMaxZ - worldMinZ;

        // Unity 좌표 -> 이미지 좌표로 정규화
        float normX = (unityX - worldMinX) / worldWidth;
        float normY = 1f - ((unityZ - worldMinZ) / worldHeight); // Y축 반전

        float pixelX = normX * imageWidth;
        float pixelY = normY * imageHeight;

        Vector2 target = new Vector2(pixelX, pixelY);

        float scale = Mathf.Pow(ZoomBase, zoomStep);
        Vector2 viewCenter = new Vector2(resolvedStyle.width, resolvedStyle.height) / 2f;

        currentOffset = viewCenter - target * scale;
        mapContainer.style.translate = new Translate(currentOffset.x, currentOffset.y, 0);
    }

    public new class UxmlFactory : UxmlFactory<MapView, VisualElement.UxmlTraits> { }   
    
}