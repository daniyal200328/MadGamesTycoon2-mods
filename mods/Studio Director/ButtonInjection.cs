using System;
using UnityEngine;
using UnityEngine.UI;

partial class StudioDirectorPlugin
{
    private const string InjectedButtonName = "Button_StudioDirector";

    internal static publisherScript GetMenuPublisher(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null || TochterfirmaMenuPublisherField == null) return null;
        return TochterfirmaMenuPublisherField.GetValue(menu) as publisherScript;
    }

    internal static void InjectButton(Menu_Stats_Tochterfirma_Main menu, publisherScript subsidiary)
    {
        if (menu == null || subsidiary == null || menu.uiObjects == null || menu.uiObjects.Length <= 31)
            return;

        GameObject template = menu.uiObjects[31];
        if (template == null) return;

        AdjustParentLayout(template);

        GameObject buttonObject = FindInjectedButton(menu);
        if (buttonObject != null && buttonObject.GetComponent<SubsidiaryDesignButton>() == null)
        {
            UnityEngine.Object.Destroy(buttonObject);
            buttonObject = null;
        }

        if (buttonObject == null)
        {
            buttonObject = CreateFreshCircleButton(template);
            RectTransform templateRect = template.GetComponent<RectTransform>();
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            if (templateRect != null && buttonRect != null)
            {
                buttonRect.anchorMin = templateRect.anchorMin;
                buttonRect.anchorMax = templateRect.anchorMax;
                buttonRect.pivot = templateRect.pivot;
                buttonRect.sizeDelta = templateRect.sizeDelta;
                buttonRect.anchoredPosition = templateRect.anchoredPosition + new Vector2(0f, 68f);
            }
        }

        SubsidiaryDesignButton marker = buttonObject.GetComponent<SubsidiaryDesignButton>();
        if (marker == null)
            marker = buttonObject.AddComponent<SubsidiaryDesignButton>();
        marker.Target = subsidiary;

        ConfigureButtonVisual(buttonObject);

        Button button = buttonObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(marker.HandleClick);
            button.interactable = IsValidSubsidiary(subsidiary);
        }

        buttonObject.SetActive(IsValidSubsidiary(subsidiary));
    }

    internal static void RefreshButton(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null) return;

        GameObject buttonObject = FindInjectedButton(menu);
        if (buttonObject == null) return;

        publisherScript subsidiary = GetMenuPublisher(menu);
        SubsidiaryDesignButton marker = buttonObject.GetComponent<SubsidiaryDesignButton>();
        if (marker != null)
            marker.Target = subsidiary;

        bool valid = IsValidSubsidiary(subsidiary);
        buttonObject.SetActive(valid);
        Button button = buttonObject.GetComponent<Button>();
        if (button != null)
            button.interactable = valid;
    }

    private static void AdjustParentLayout(GameObject template)
    {
        if (template.transform.parent == null) return;

        HorizontalLayoutGroup hlg = template.transform.parent.GetComponent<HorizontalLayoutGroup>();
        if (hlg != null)
        {
            hlg.spacing = 10f;
            hlg.childAlignment = TextAnchor.MiddleCenter;
            return;
        }

        GridLayoutGroup glg = template.transform.parent.GetComponent<GridLayoutGroup>();
        if (glg != null)
        {
            glg.spacing = new Vector2(10f, glg.spacing.y);
            glg.childAlignment = TextAnchor.MiddleCenter;
        }
    }

    private static GameObject FindInjectedButton(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null) return null;

        if (menu.uiObjects != null && menu.uiObjects.Length > 31 && menu.uiObjects[31] != null)
        {
            Transform parentTrans = menu.uiObjects[31].transform.parent;
            if (parentTrans != null)
            {
                Transform found = parentTrans.Find(InjectedButtonName);
                if (found != null) return found.gameObject;
            }
        }

        SubsidiaryDesignButton[] markers = menu.GetComponentsInChildren<SubsidiaryDesignButton>(true);
        for (int i = 0; i < markers.Length; i++)
        {
            if (markers[i] != null && markers[i].gameObject.name == InjectedButtonName)
                return markers[i].gameObject;
        }

        return null;
    }

    private static GameObject CreateFreshCircleButton(GameObject template)
    {
        GameObject buttonObject = new GameObject(
            InjectedButtonName,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(Button),
            typeof(tooltip),
            typeof(SubsidiaryDesignButton));

        buttonObject.transform.SetParent(template.transform.parent, false);
        buttonObject.transform.SetAsLastSibling();

        Image templateImage = template.GetComponent<Image>();
        Image image = buttonObject.GetComponent<Image>();
        if (templateImage != null)
        {
            image.sprite = templateImage.sprite;
            image.type = templateImage.type;
            image.color = templateImage.color;
            image.material = templateImage.material;
        }

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.ColorTint;

        GameObject iconObject = new GameObject("Icon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        iconObject.transform.SetParent(buttonObject.transform, false);
        RectTransform iconRect = iconObject.GetComponent<RectTransform>();
        iconRect.anchorMin = new Vector2(0.18f, 0.18f);
        iconRect.anchorMax = new Vector2(0.82f, 0.82f);
        iconRect.offsetMin = Vector2.zero;
        iconRect.offsetMax = Vector2.zero;

        Image iconImage = iconObject.GetComponent<Image>();
        iconImage.preserveAspect = true;
        iconImage.raycastTarget = false;

        return buttonObject;
    }

    private static void ConfigureButtonVisual(GameObject buttonObject)
    {
        if (buttonObject == null) return;

        tooltip tip = buttonObject.GetComponent<tooltip>();
        if (tip == null)
            tip = buttonObject.AddComponent<tooltip>();
        tip.c = "Design the next game for this subsidiary.";

        Text[] texts = buttonObject.GetComponentsInChildren<Text>(true);
        for (int i = 0; i < texts.Length; i++)
            texts[i].text = "";

        Sprite icon = null;
        string dllPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string pngPath = System.IO.Path.Combine(dllPath, "Project-Director-button.png");
        if (System.IO.File.Exists(pngPath))
        {
            try
            {
                byte[] fileData = System.IO.File.ReadAllBytes(pngPath);
                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (texture.LoadImage(fileData))
                {
                    icon = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
            catch { }
        }

        if (icon != null)
        {
            Image mainImg = buttonObject.GetComponent<Image>();
            if (mainImg != null)
            {
                mainImg.sprite = icon;
                mainImg.color = Color.white;
            }

            Image[] images = buttonObject.GetComponentsInChildren<Image>(true);
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != null && images[i].gameObject != buttonObject)
                {
                    images[i].enabled = false;
                }
            }
        }
        else
        {
            games games = FindGames();
            if (games != null && games.gameTypSprites != null && games.gameTypSprites.Length > 0)
                icon = games.gameTypSprites[0];

            if (icon != null)
            {
                Image[] images = buttonObject.GetComponentsInChildren<Image>(true);
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] != null && images[i].gameObject != buttonObject)
                    {
                        images[i].sprite = icon;
                        images[i].color = Color.white;
                        images[i].enabled = true;
                        break;
                    }
                }
            }
        }
    }

    internal sealed class SubsidiaryDesignButton : MonoBehaviour
    {
        internal publisherScript Target;

        public void HandleClick()
        {
            BeginDesignForSubsidiary(Target);
        }
    }
}
