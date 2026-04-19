using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIF_Builder.Data;
using NIF_Builder.Models;
using NIF_Builder.Models.ViewModels;

namespace NIF_Builder.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;

        public ProjectsController(ApplicationDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.Include(x => x.ProjectEquipments).ThenInclude(y => y.Equipment).ToListAsync());
        }

        public IActionResult AddNewEquipment(int? id)
        {
            ViewBag.equipments = new SelectList(_context.Equipments, "EquipmentID", "EquipmentName", id?.ToString() ?? "");
            return PartialView("_addNewEquipment");
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Aggregation()
        {
            var proj = _context.Projects.ToList();
            var db = _context.Projects;
            ViewBag.count = db.Count();
            ViewBag.max = db.Max(p => p.Budget);
            ViewBag.min = db.Min(p => p.Budget);
            ViewBag.sum = db.Sum(p => p.Budget);
            ViewBag.avg = db.Average(p => p.Budget);
            return View(proj);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectVM projectVM, int[] equipmentId)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project()
                {
                    ProjectName = projectVM.ProjectName,
                    StartDate = projectVM.StartDate,
                    EstimateEndDate = projectVM.EstimateEndDate,
                    Budget = projectVM.Budget,
                    WorkInProgress = projectVM.WorkInProgress
                };

                // For Document (Adapted from Image logic)
                var file = projectVM.ProjectDocumentFile;
                string webroot = _he.WebRootPath;
                string folder = "Documents";

                if (file != null)
                {
                    string ext = Path.GetExtension(projectVM.ProjectDocumentFile.FileName);
                    string docFileName = Path.GetRandomFileName() + ext;
                    string fileSave = Path.Combine(webroot, folder, docFileName);

                    using (var stream = new FileStream(fileSave, FileMode.Create))
                    {
                        await projectVM.ProjectDocumentFile.CopyToAsync(stream);
                        project.ProjectDocuments = "/" + folder + "/" + docFileName;
                    }
                }

                // For Equipment (Adapted from Skill logic)
                foreach (var item in equipmentId)
                {
                    ProjectEquipment projectEquipment = new ProjectEquipment()
                    {
                        Project = project,
                        ProjectID = project.ProjectID,
                        EquipmentID = item
                    };
                    _context.ProjectEquipments.Add(projectEquipment);
                }

                await _context.SaveChangesAsync();
                return PartialView("_success");
            }
            return PartialView("_error");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectID == id);
            if (project == null) return NotFound();

            ProjectVM projectVM = new ProjectVM()
            {
                ProjectID = project.ProjectID,
                ProjectName = project.ProjectName,
                StartDate = project.StartDate,
                EstimateEndDate = project.EstimateEndDate,
                Budget = project.Budget,
                WorkInProgress = project.WorkInProgress,
                ProjectDocuments = project.ProjectDocuments
            };

            // Equipment list
            var existingEquipment = _context.ProjectEquipments.Where(x => x.ProjectID == id).ToList();
            foreach (var item in existingEquipment)
            {
                projectVM.EquipmentList.Add(item.EquipmentID);
            }
            return View(projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectVM projectVM, int[] equipmentId)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project()
                {
                    ProjectID = projectVM.ProjectID,
                    ProjectName = projectVM.ProjectName,
                    StartDate = projectVM.StartDate,
                    EstimateEndDate = projectVM.EstimateEndDate,
                    Budget = projectVM.Budget,
                    WorkInProgress = projectVM.WorkInProgress
                };

                // Document
                var file = projectVM.ProjectDocumentFile;
                var oldDoc = projectVM.ProjectDocuments;

                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Documents";
                    string ext = Path.GetExtension(projectVM.ProjectDocumentFile.FileName);
                    string docFileName = Path.GetRandomFileName() + ext;
                    string fileSave = Path.Combine(webroot, folder, docFileName);

                    using (var stream = new FileStream(fileSave, FileMode.Create))
                    {
                        await projectVM.ProjectDocumentFile.CopyToAsync(stream);
                        project.ProjectDocuments = "/" + folder + "/" + docFileName;
                    }
                }
                else
                {
                    project.ProjectDocuments = oldDoc;
                }

                // Equipment Update
                var existEquipment = _context.ProjectEquipments.Where(x => x.ProjectID == project.ProjectID).ToList();
                foreach (var item in existEquipment)
                {
                    _context.ProjectEquipments.Remove(item);
                }

                // Add new Equipment
                foreach (var item in equipmentId)
                {
                    ProjectEquipment projectEquipment = new ProjectEquipment()
                    {
                        ProjectID = project.ProjectID,
                        EquipmentID = item
                    };
                    _context.ProjectEquipments.Add(projectEquipment);
                }

                _context.Update(project);
                await _context.SaveChangesAsync();
                return PartialView("_success");
            }
            return PartialView("_error");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var proj = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectID == id);
            var existEquipment = _context.ProjectEquipments.Where(x => x.ProjectID == id).ToList();

            foreach (var item in existEquipment)
            {
                _context.ProjectEquipments.Remove(item);
            }

            _context.Remove(proj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}