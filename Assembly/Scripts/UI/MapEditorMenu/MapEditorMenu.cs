﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using Characters;
using GameManagers;
using Map;
using System.Collections;
using Utility;

namespace UI
{
    class MapEditorMenu: BaseMenu
    {
        public MapEditorInspectPanel InspectPanel;
        public MapEditorHierarchyPanel HierarchyPanel;
        public MapEditorTopPanel _topPanel;
        public MapEditorAddObjectPopup AddObjectPopup;
        public MapEditorTexturePopup TexturePopup;
        public MapEditorCameraPopup CameraPopup;
        public MapEditorInfoPopup InfoPopup;
        public MapEditorSettingsPopup SettingsPopup;
        public MapEditorCustomLogicPopup CustomLogicPopup;
        public MapEditorSelectComponentPopup SelectComponentPopup;
        public Image DragImage;
        public bool IsMouseUI;

        public override void Setup()
        {
            base.Setup();
            RebuildPanels();
            _topPanel = ElementFactory.CreateHeadedPanel<MapEditorTopPanel>(transform, true);
            ElementFactory.SetAnchor(_topPanel.gameObject, TextAnchor.UpperCenter, TextAnchor.UpperCenter, new Vector2(0f, 0f));
            DragImage = ElementFactory.InstantiateAndBind(transform, "MapEditorDragImage").GetComponent<Image>();
            ElementFactory.SetAnchor(DragImage.gameObject, TextAnchor.LowerLeft, TextAnchor.LowerLeft, Vector2.zero);
            DragImage.gameObject.SetActive(false);
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            AddObjectPopup = ElementFactory.CreateDefaultPopup<MapEditorAddObjectPopup>(transform);
            TexturePopup = ElementFactory.CreateDefaultPopup<MapEditorTexturePopup>(transform);
            CameraPopup = ElementFactory.CreateDefaultPopup<MapEditorCameraPopup>(transform);
            InfoPopup = ElementFactory.CreateDefaultPopup<MapEditorInfoPopup>(transform);
            SettingsPopup = ElementFactory.CreateDefaultPopup<MapEditorSettingsPopup>(transform);
            CustomLogicPopup = ElementFactory.CreateDefaultPopup<MapEditorCustomLogicPopup>(transform);
            SelectComponentPopup = ElementFactory.CreateDefaultPopup<MapEditorSelectComponentPopup>(transform);
            _popups.Add(AddObjectPopup);
            _popups.Add(TexturePopup);
            _popups.Add(CameraPopup);
            _popups.Add(InfoPopup);
            _popups.Add(SettingsPopup);
            _popups.Add(CustomLogicPopup);
            _popups.Add(SelectComponentPopup);
        }

        public void SetDrag(bool active, Vector2 start, Vector2 end)
        {
            if (active)
            {
                if (!DragImage.gameObject.activeSelf)
                    DragImage.gameObject.SetActive(true);
                float x1 = Mathf.Min(start.x, end.x);
                float y1 = Mathf.Min(start.y, end.y);
                float x2 = Mathf.Max(start.x, end.x);
                float y2 = Mathf.Max(start.y, end.y);
                start = new Vector2(x1, y1);
                end = new Vector2(x2, y2);
                DragImage.rectTransform.anchoredPosition = start;
                DragImage.rectTransform.sizeDelta = end - start;
            }
            else
            {
                if (DragImage.gameObject.activeSelf)
                    DragImage.gameObject.SetActive(false);
            }
        }

        public void ShowInspector(MapObject obj)
        {
            HideInspector();
            InspectPanel = ElementFactory.CreateHeadedPanel<MapEditorInspectPanel>(transform);
            ElementFactory.SetAnchor(InspectPanel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-10f, -70f));
            InspectPanel.Show(obj);
        }

        public void HideInspector()
        {
            if (InspectPanel != null)
                Destroy(InspectPanel.gameObject);
        }

        public void SyncInspector()
        {
            if (InspectPanel != null)
                InspectPanel.SyncSettings();
        }

        public void ShowHierarchyPanel()
        {
            if (HierarchyPanel != null)
                Destroy(HierarchyPanel.gameObject);
            HierarchyPanel = ElementFactory.CreateHeadedPanel<MapEditorHierarchyPanel>(transform);
            ElementFactory.SetAnchor(HierarchyPanel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(10f, -70f));
            HierarchyPanel.Show();
        }

        public void SyncHierarchyPanel()
        {
            HierarchyPanel.Sync();
        }

        public void RebuildPanels()
        {
        }

        public float GetMinMouseX()
        {
            if (HierarchyPanel != null)
                return HierarchyPanel.GetPhysicalWidth() + 10f;
            return 0f;
        }

        public float GetMaxMouseX()
        {
            float max = Screen.width;
            if (InspectPanel != null && InspectPanel.gameObject.activeSelf)
                max -= (InspectPanel.GetPhysicalWidth() + 10f);
            return max;
        }

        public float GetMinMouseY()
        {
            return 0f;
        }

        public float GetMaxMouseY()
        {
            return Screen.height - 60f;
        }

        private void Update()
        {
            UpdateMouseUI();
        }

        public bool IsPopupActive()
        {
            bool popupActive = false;
            foreach (BasePopup popup in _popups)
            {
                if (popup.IsActive)
                    popupActive = true;
            }
            return popupActive;
        }

        private void UpdateMouseUI()
        {
            var position = Input.mousePosition;
            IsMouseUI = IsPopupActive() || _topPanel.IsDropdownOpen() || position.x < GetMinMouseX() || position.x > GetMaxMouseX() || 
                position.y < GetMinMouseY() || position.y > GetMaxMouseY();
        }
    }
}
