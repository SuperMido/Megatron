using Megatron.Models;
using Microsoft.AspNetCore.Mvc;

namespace Megatron.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _service;
        public AccountController(AccountService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View(_service.Get());
        }
        public IActionResult Details(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            else return View(b);
        }
        public IActionResult Delete(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            else return View(b);
        }

        [HttpPost]
        public IActionResult Delete(Account account)
        {
            _service.Delete(account.Id);
            _service.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            else return View(b);
        }

        [HttpPost]
        public IActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                _service.Update(account);
                _service.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }
        public IActionResult Create() => base.View(_service.Create());

        [HttpPost]
        public IActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                _service.Add(account);
                _service.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }
    }
}