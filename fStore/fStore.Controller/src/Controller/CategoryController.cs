using fStore.Business;
using fStore.Business.DTO;
using fStore.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fStore.Controller;
[ApiController]
[Route("api/v1/categories")]
public class CategoryController : BaseController<Category, CategoryReadDTO, CategoryCreateDTO, CategoryUpdateDTO>
{
    public CategoryController(ICategoryService service) : base(service)
    {
    }

    [AllowAnonymous]
    public override Task<ActionResult<IEnumerable<CategoryReadDTO>>> GetAll([FromQuery] GetAllParams options)
    {
        return base.GetAll(options);
    }

    [AllowAnonymous]
    [HttpGet("{id:Guid}")]
    public override Task<ActionResult<CategoryReadDTO>> GetById([FromRoute] Guid id)
    {
        return base.GetById(id);
    }
}
