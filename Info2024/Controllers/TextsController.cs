﻿using Info2024.Data;
using Info2024.Models;
using Info2024.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Info2024.Controllers
{
	public class TextsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TextsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Texts
		public async Task<IActionResult> Index(int PageNumber = 1)
		{
			TextIndexViewModel textIndexViewModel = new()
			{
				TextList = new()
			};

			textIndexViewModel.TextList.TextCount = _context.Texts
				.Where(t => t.Active == true)
				.Count();

			textIndexViewModel.TextList.PageNumber = PageNumber;

			textIndexViewModel.Texts = await _context.Texts
								.Include(t => t.Category)
								.Include(t => t.Author)
								.Where(t => t.Active == true)
								.OrderByDescending(t => t.AddedDate)
								.Skip((PageNumber - 1) * textIndexViewModel.TextList.PageSize)
								.Take(textIndexViewModel.TextList.PageSize)
								.ToListAsync();

			return View(textIndexViewModel);
		}

		// GET: Texts
		public async Task<IActionResult> List()
		{
			var applicationDbContext = _context.Texts.Include(t => t.Author).Include(t => t.Category);
			return View(await applicationDbContext.ToListAsync());
		}


		// GET: Texts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var text = await _context.Texts
					.Include(t => t.Author)
					.Include(t => t.Category)
					.FirstOrDefaultAsync(m => m.TextId == id);
			if (text == null)
			{
				return NotFound();
			}

			return View(text);
		}

		// GET: Texts/Create
		public IActionResult Create()
		{
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Description");
			return View();
		}

		// POST: Texts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("TextId,Title,Summary,Keywords,Content,Graphic,Active,AddedDate,CategoryId,UserId")] Text text)
		{
			if (ModelState.IsValid)
			{
				_context.Add(text);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", text.UserId);
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Description", text.CategoryId);
			return View(text);
		}

		// GET: Texts/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var text = await _context.Texts.FindAsync(id);
			if (text == null)
			{
				return NotFound();
			}
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", text.UserId);
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Description", text.CategoryId);
			return View(text);
		}

		// POST: Texts/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("TextId,Title,Summary,Keywords,Content,Graphic,Active,AddedDate,CategoryId,UserId")] Text text)
		{
			if (id != text.TextId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(text);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TextExists(text.TextId))
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
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", text.UserId);
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Description", text.CategoryId);
			return View(text);
		}

		// GET: Texts/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var text = await _context.Texts
					.Include(t => t.Author)
					.Include(t => t.Category)
					.FirstOrDefaultAsync(m => m.TextId == id);
			if (text == null)
			{
				return NotFound();
			}

			return View(text);
		}

		// POST: Texts/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var text = await _context.Texts.FindAsync(id);
			if (text != null)
			{
				_context.Texts.Remove(text);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool TextExists(int id)
		{
			return _context.Texts.Any(e => e.TextId == id);
		}
	}
}
