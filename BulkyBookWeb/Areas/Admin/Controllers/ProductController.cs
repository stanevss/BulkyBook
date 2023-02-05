﻿using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    //private readonly ApplicationDbContext _db;
    //private readonly IProductRepository _db;
    private readonly IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
        return View(objProductList);
    }

    // GET
    public IActionResult Create()
    {

        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);

    }


    // GET
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(
            i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            })

        };

       
        if (id == null || id == 0)
        {
            // create product
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
            return View(productVM);
        } 
        else
        {
            // update product
        }
        

        return View(productVM);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile file)
    {

        if (ModelState.IsValid)
        {
            //_unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product updated successfully";
            return RedirectToAction("Index");
        }
        return View(obj);

    }

    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var ProductFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

        if (ProductFromDbFirst == null)
        {
            return NotFound();
        }

        return View(ProductFromDbFirst);
    }


    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully";
        return RedirectToAction("Index");
    }
}

