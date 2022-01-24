using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Pages
{
    NONE = 0,
    MAIN = 5,
    LEVEL = 6,
    ARCHIEVEMENT = 7,
    CUSTOMIZE = 8,
    UPGRADES = 9,
    OPTION = 10,
    LANGUAGE = 11,
    SUPPORT = 12,
}

public class MainMenu : MonoBehaviour
{
    
    [System.Serializable]
    public struct PageContent
    {
        public Pages page;
        public CanvasGroupUI content;
        public CanvasGroupUI bar;
    }

    [Header("Default Page")]
    [SerializeField]
    private Pages CurrentPage = Pages.MAIN;
    private Pages previousPage = Pages.NONE;

    [Header("Page Content")]
    [SerializeField]
    private List<PageContent> pageContents;

    private Dictionary<Pages, PageContent> pages = new Dictionary<Pages, PageContent>();

    private void Start()
    {
        InitPage();

        SelectPage(CurrentPage);
    }

    private void InitPage()
    {
        for (int i = 0; i < pageContents.Count; i++)
            pages.Add(pageContents[i].page, pageContents[i]);
    }

    private void ShowPage(PageContent pageContent)
    {
        pageContent.content.Show();
        pageContent.bar.Show();
    }

    private void ClosePage(PageContent pageContent)
    {
        pageContent.content.Hide();
        pageContent.bar.Hide();
    }

    public void SelectPage(Pages nextPage)
    {
        if (pages.ContainsKey(nextPage) == false)
            return;

        if(previousPage != Pages.NONE)
            ClosePage(pages[previousPage]);

        CurrentPage = nextPage;
        ShowPage(pages[CurrentPage]);
        previousPage = CurrentPage;
    }
}
