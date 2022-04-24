using System;
using System.Collections.Generic;
using System.Text;

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
