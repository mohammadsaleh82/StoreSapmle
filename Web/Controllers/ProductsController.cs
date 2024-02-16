using Application.Dtos.Products;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Products;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Web.Controllers;

public class ProductsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    // GET: Product
    [Authorize(Roles = "Admin,ShowProducts")]
    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new GetAll.Query());
        if (result.IsSuccess)
            return View(result.Value);
        else
            return BadRequest(result.errors);
    }


    // GET: Product/Create
    [Authorize(Roles = "Admin,CreateProducts")]
    public async Task<IActionResult> Create()
    {
        await GenerateSelectsList(0);
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    public async Task<IActionResult> Create(Create.Command command)
    {
        var validator = new ProductValidator();
        var validationResult = await validator.ValidateAsync(command.ProductDto);
        if (!validationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            await GenerateSelectsList(command.ProductDto.CategoryId);
            return View(command);
        }

        if (command.ImageFile is null)
        { 
            await GenerateSelectsList(command.ProductDto.CategoryId);
            return View(command);
        }
        var result = await _mediator.Send(new Create.Command{ProductDto =command.ProductDto,ImageFile = command.ImageFile});
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));
        else
            ModelState.AddModelError("", result.errors);
        await GenerateSelectsList(command.ProductDto.CategoryId);


        return View(command);
    }

    // GET: Product/Edit/5
    [Authorize(Roles = "Admin,EditProducts")]
    public async Task<IActionResult> Edit(int id)
    {
        
        var result = await _mediator.Send(new Details.Query { Id = id });
        if (result.IsSuccess)
        {
            await GenerateSelectsList(result.Value.CategoryId);
            var product = _mapper.Map<ProductDto>(result.Value);
            return View(new Update.Command() { ProductDto = product });
        }
        else
            return NotFound();
    }

    // POST: Product/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Update.Command command)
    {
        if (id != command.Id)
            return NotFound();

        var validator = new ProductValidator();
        var validationResult = await validator.ValidateAsync(command.ProductDto);
        if (!validationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            await GenerateSelectsList(command.ProductDto.CategoryId);
            return View(command);
        }

        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));
        else
            ModelState.AddModelError("", result.errors);

        await GenerateSelectsList(command.ProductDto.CategoryId);
        return View(command);
    }

    private async Task GenerateSelectsList(int categoryId)
    {
        var categories = await _mediator.Send(new Application.Categories.GetAll.Query());
        ViewBag.Categories = categories.Value.Select(c => new SelectListItem
        {
            Selected = c.Id == categoryId,
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();
    }

    [Authorize(Roles = "Admin,DeleteProducts")]
    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new Details.Query { Id = id });
        if (result.IsSuccess)
            return View(result.Value);
        else
            return NotFound();
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new Delete.Command { Id = id });
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));
        else
            return BadRequest(result.errors);
    }
}