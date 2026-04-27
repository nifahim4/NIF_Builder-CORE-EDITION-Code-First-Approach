using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIF_Builder.Data;

namespace NIF_Builder.ViewComponents
{
    public class ActiveProjectsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ActiveProjectsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var activeCount = await _context.Projects.CountAsync(p => p.WorkInProgress);

            return View(activeCount);
        }
    }
}