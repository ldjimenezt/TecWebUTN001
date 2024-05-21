using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TecWebUTN001.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TecWebUTN001.Controllers
{
    public class ProductosController : Controller
    {
        private readonly DbContext _context;

        public ProductosController(DbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string? buscar = "")
        {
            if (string.IsNullOrEmpty(buscar))
            {
                var data = await _context.Producto.ToListAsync();

                ViewData["qty"] = CantidadProductos(data);
                ViewBag.CantidadProductos = CantidadProductos(data);
                ViewBag.CostoTotal = CostoInventario(data);
                return View(data);
            }
            else
            {
                var resultado = _context
                    .Producto
                    .Where(
                        p => p.Description.ToLower().Contains(buscar.ToLower())
                )
                    .ToList();

                ViewData["qty"] = CantidadProductos(resultado);
                ViewBag.CantidadProductos = CantidadProductos(resultado);
                ViewBag.CostoTotal = CostoInventario(resultado);
                return View(resultado);
            }
        }

        public async Task<IActionResult> PorDesabastecer()
        {
            var resultado = _context
                .Producto
                .Where(p => p.Amount <= 10) 
                .ToList();

            ViewData["qty"] = CantidadProductos(resultado);
            ViewBag.CantidadProductos = CantidadProductos(resultado);
            ViewBag.CostoTotal = CostoInventario(resultado);
            return View("index",resultado);
        }

        private int CantidadProductos( IEnumerable<Producto> productos)
        {
            var cantidad = productos.Count();
            return cantidad;
        }

        private double CostoInventario(IEnumerable<Producto> productos)
        {
            var costoInventario = productos.Sum(p => p.Amount * (double)p.Price);
            return costoInventario;
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Amount")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Amount")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }
    }
}
