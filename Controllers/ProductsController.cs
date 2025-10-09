using Microsoft.AspNetCore.Mvc;
using InventoryWebApp.Repositories;
using InventoryWebApp.Models;
using System.Text;

namespace InventoryWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _repo = new();

        public IActionResult Index(string? search)
        {
            var products = _repo.GetAll(search);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_ProductTablePartial", products);　// Ajax検索時
            return View(products);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _repo.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _repo.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var product = _repo.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public FileResult ExportCsv()
        {
            var products = _repo.GetAll();
            var csv = new StringBuilder();
            csv.AppendLine("ID,商品名,数量,単価,在庫金額");

            foreach (var p in products)
                csv.AppendLine($"{p.Id},{p.Name},{p.Quantity},{p.Price},{p.TotalValue}");

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "products.csv");
        }
    }
}
