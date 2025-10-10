using Microsoft.AspNetCore.Mvc;
using InventoryWebApp.Repositories;
using InventoryWebApp.Models;
using System.Text;

namespace InventoryWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _repo = new();

        // page, pageSize optional
        public IActionResult Index(string? search, int page = 1, int pageSize = 10)
        {
            var paged = _repo.GetPaged(search, page < 1 ? 1 : page, pageSize);
            // Ajaxリクエストなら部分ビュー（テーブルのみ）を返す
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductTablePartial", paged);
            }

            // フルビュー（レイアウト含む）を返す
            ViewBag.Search = search;
            return View(paged);
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            var products = _repo.GetAll();
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return PartialView("_ProductTablePartial", products); // ← 部分ビューを返すことが重要！
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(p);
                return RedirectToAction(nameof(Index));
            }
            return View(p);
        }

        public IActionResult Edit(int id)
        {
            var p = _repo.GetById(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        public IActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(p);
                return RedirectToAction(nameof(Index));
            }
            return View(p);
        }

        public IActionResult Details(int id)
        {
            var p = _repo.GetById(id);
            if (p == null) return NotFound();
            return View(p);
        }

        public IActionResult Delete(int id)
        {
            var p = _repo.GetById(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public FileResult ExportCsv()
        {
            var list = _repo.GetAll();
            var sb = new StringBuilder();
            sb.AppendLine("Id,Name,Quantity,Price,TotalValue");
            foreach (var p in list)
                sb.AppendLine($"{p.Id},{EscapeCsv(p.Name)},{p.Quantity},{p.Price},{p.TotalValue}");
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "products.csv");
        }

        private string EscapeCsv(string s)
        {
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
            {
                return $"\"{s.Replace("\"", "\"\"")}\"";
            }
            return s;
        }
    }
}
