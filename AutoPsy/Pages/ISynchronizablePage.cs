namespace AutoPsy.Pages
{
    public interface ISynchronizablePage
    {
        void SynchronizeContentPages(CustomComponents.IСustomComponent experiencePanel);
    }

    public interface ISynchronizablePageWithQuery
    {
        void SynchronizeContentPages();
    }

}
