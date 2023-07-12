using Microsoft.AspNetCore.Mvc;
using MySolution.Models.Common;

namespace MySolution.AdminApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
