using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Pages
{
    public interface ISynchronizablePage
    {
        void SynchronizeContentPages(CustomComponents.UserExperiencePanel experiencePanel);
    }
}
